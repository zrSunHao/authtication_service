using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Manager.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Hao.Authentication.Manager.RabbitMq
{
    public class RabbitMqDirectReceiver : BackgroundService
    {
        private readonly ILogger<RabbitMqDirectReceiver> _logger;
        private readonly RabbitMqConfiguration _config;
        private readonly IMyLogProvider _myLog;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqDirectReceiver(IOptions<RabbitMqConfiguration> options,
            ILogger<RabbitMqDirectReceiver> logger,
            IServiceScopeFactory factory)
        {
            _config = options.Value;
            _logger = logger;
            _myLog = factory.CreateScope().ServiceProvider.GetRequiredService<IMyLogProvider>();
            CreateConnection();
            CreateChannel();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            if (ChannelExists())
            {
                var consumer = new EventingBasicConsumer(_channel);     //创建消费者
                consumer.Received += async (model, ea) =>                     //消息消费方法
                {
                    var body = ea.Body;
                    string msg = Encoding.UTF8.GetString(body.ToArray());
                    await HandleMessage(msg);
                    _channel.BasicAck(                     //回复mq，确认消息已接收
                        deliveryTag: ea.DeliveryTag,       //消息id
                        multiple: true);                   //确认一个或多个已传递的消息
                };

                _channel.BasicConsume(          //监听队列
                    queue: _config.QueueName,    //队列名
                    autoAck: false,             //是否自动回复mq，若不回复，则消息一直存在于消息队列之中
                    consumer: consumer);        //消费者
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 服务正常停止时
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
            }

            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
            }

            return base.StopAsync(cancellationToken);
        }

        #region 处理

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg"></param>
        private async Task<int> HandleMessage(string json)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<RabbitMqMsg<object>>(json);
                if (msg != null && msg.Category == MsgCategory.log && msg.Value != null)
                {
                    var log = JsonConvert.DeserializeObject<RabbitMqMsg<LogM>>(json);
                    if (log != null && log.Value != null)
                    {
                        var count = await _myLog.Save(log.Value);
                        Console.WriteLine($"  ===============>    {count}条日志保存成功！");
                        return count;
                    }else return 0;
                }
                else throw new Exception($"消息类别未识别：[{json}]");
            }
            catch(Exception)
            {
                _logger.LogError($"日志保存失败  ======> {json} ");
                return 0;
            }
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        #endregion

        #region 开启

        /// <summary>
        /// 创建连接
        /// </summary>
        private void CreateConnection()
        {
            try
            {
                if (_config == null) throw new Exception("RabbitMQ config is null!");

                ConnectionFactory factory = new ConnectionFactory()
                {
                    HostName = _config.Hostname,
                    Port = _config.Port,
                    UserName = _config.UserName,
                    Password = _config.Password,
                };
                factory.Ssl.Enabled = _config.SslEnable;

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
                _logger.LogError(ex, "RabbitMQ初始化失败");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }

        /// <summary>
        /// 创建频道
        /// </summary>
        private void CreateChannel()
        {
            if (ConnectionExists())
            {
                _channel = _connection.CreateModel();
                //声明队列
                _channel.QueueDeclare(queue: _config.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                //声明路由（交换机）
                _channel.ExchangeDeclare(exchange: _config.ExchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                //绑定队列、路由
                _channel.QueueBind(queue: _config.QueueName, exchange: _config.ExchangeName, routingKey: string.Empty, arguments: null);
            }
        }

        private bool ChannelExists()
        {
            if (_channel != null)
            {
                return true;
            }

            CreateChannel();

            return _channel != null;
        }

        #endregion
    }

    
}
