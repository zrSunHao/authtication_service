namespace Hao.Authentication.Common.Enums
{
    public enum ConstraintCategory
    {
        /// <summary>
        /// 账号->所有系统
        /// </summary>
        customer_all_system = 1,

        /// <summary>
        /// 账号->某系统
        /// </summary>
        customer_one_system = 2,

        /// <summary>
        /// 程序->功能
        /// </summary>
        program_function = 3,

        /// <summary>
        /// 系统->角色
        /// </summary>
        system_role = 4,
    }

    public enum ConstraintMethod
    {
        /// <summary>
        /// 禁用
        /// </summary>
        forbid = 1,

        /// <summary>
        /// 锁定
        /// </summary>
        _lock = 2,
    }
}
