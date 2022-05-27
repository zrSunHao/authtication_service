namespace Hao.Authentication.Persistence.Attributes
{
    /// <summary>
    /// 实体类型设置表Id的前缀
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
    public class TablePrefixAttribute : Attribute
    {
        public string Prefix = "";

        /// <summary>
        /// 表的前缀，最长为4位
        /// </summary>
        /// <param name="prefix"></param>
        public TablePrefixAttribute(string prefix)
        {
            this.Prefix = prefix;
        }
    }
}
