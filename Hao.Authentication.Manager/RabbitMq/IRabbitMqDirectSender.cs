namespace Hao.Authentication.Manager.RabbitMq
{
    public interface IRabbitMqDirectSender
    {
        public void SendMsg<T>(RabbitMqMsg<T> msg);
    }
}
