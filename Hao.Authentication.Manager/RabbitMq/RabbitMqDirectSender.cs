using Hao.Authentication.Manager.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.Services
{
    /// <summary>
    /// 工作队列模式
    /// 一个交换机 -------> 一个队列 
    /// </summary>
    public class RabbitMqDirectSender : IRabbitMqDirectSender, IDisposable
    {
        private readonly ILogger<RabbitMqDirectSender> _logger;
        private readonly RabbitMqConfiguration _config;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqDirectSender(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqDirectSender> logger)
        {
            _config = options.Value;
            _logger = logger;
            CreateConnection();
            CreateChannel();
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="obj"></param>
        public void SendMsg<T>(RabbitMqMsg<T> msg)
        {
            if (ChannelExists() || _channel.IsClosed)
            {
                var json = JsonConvert.SerializeObject(msg);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    exchange: _config.ExchangeName, //交换机，如果不指定将使用默认的交换机
                    routingKey: string.Empty,       //路由key，默认为空，交换机根据路由key来将消息转发到指定队列
                    basicProperties: null,          //消息属性
                    body: body);                    //消息内容
            }
        }

        #region

        /// <summary>
        /// 创建连接
        /// </summary>
        private void CreateConnection()
        {
            try
            {
                if (_config == null) throw new Exception("RabbitMQ config is null!");

                ConnectionFactory factory = new ConnectionFactory()//连接工厂
                {
                    HostName = _config.Hostname,
                    Port = _config.Port,
                    UserName = _config.UserName,
                    Password = _config.Password,
                    VirtualHost = "/",//虚拟机，每个虚拟机相当于一个独立的mq
                };
                factory.Ssl.Enabled = _config.SslEnable;

                _connection = factory.CreateConnection();//创建连接
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
        /// 创建会话通道，并声明队列、交换机
        /// 生产者和mq服务的所有通信都在通道中完成
        /// </summary>
        private void CreateChannel()
        {
            if (ConnectionExists())
            {
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(               //声明路由（交换机）
                    exchange: _config.ExchangeName,     //名称
                    type: ExchangeType.Direct,          //交换机的模式，此处为工作模式
                    durable: true,                      //
                    autoDelete: false,                  //
                    arguments: null);                   //

                _channel.QueueDeclare(          //声明队列，如果队列不存在，则创建
                    queue: _config.QueueName,   //队列名称
                    durable: true,              //是否持久化，若是，则mq重启之后队列依旧存在
                    exclusive: false,           //是否独占连接，若是，则创建临时队列，只允许在该连接中访问，连接关闭之后队列自动删除
                    autoDelete: false,          //是否自动删除，若是，连接关闭之后队列自动删除
                    arguments: null);           //队列参数，可以设置队列的扩展参数，如队列的存活时间等

                _channel.QueueBind(                     //将队列与交换机绑定
                    queue: _config.QueueName,
                    exchange: _config.ExchangeName,
                    routingKey: string.Empty,
                    arguments: null);
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

        /// <summary>
        /// 释放资源时关闭连接
        /// </summary>
        public void Dispose()
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
            }

            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
            }
        }

        #endregion

    }
}
