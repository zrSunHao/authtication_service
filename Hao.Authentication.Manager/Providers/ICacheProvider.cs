namespace Hao.Authentication.Manager.Providers
{
    public interface ICacheProvider
    {
        /// <summary>
        /// 保存 - 默认有效时间8小时
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        /// <param name="hours">小时 - 默认8小时</param>
        /// <param name="minutes">分钟 - 默认为0</param>
        public void Save(string key, object value, int hours = 8, int minutes = 0);

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T TryGetValue<T>(string key);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exist<T>(string key);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key);
    }
}
