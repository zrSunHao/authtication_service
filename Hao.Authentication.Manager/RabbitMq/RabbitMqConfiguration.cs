namespace Hao.Authentication.Manager.RabbitMq
{
    public class RabbitMqConfiguration
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 5672;

        /// <summary>
        /// 是否启用https协议
        /// </summary>
        public bool SslEnable { get; set; } = false;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }
    }

    public class RabbitMqMsg<T>
    {
        public MsgCategory Category { get; set; }
        public T Value { get; set; }
    }

    public enum MsgCategory
    {
        log = 1,
    }
}
