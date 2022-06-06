namespace Hao.Authentication.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FunctionAttribute : Attribute
    {
        /// <summary>
        /// 功能编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">功能编码</param>
        public FunctionAttribute(string code)
        {
            Code = code;
        }
    }
}
