namespace Hao.Authentication.Common.Enums
{
    public enum SysRoleRank
    {
        /// <summary>
        /// 默认/普通用户
        /// </summary>
        _default = 1,

        /// <summary>
        /// 会员
        /// </summary>
        member = 10, 

        /// <summary>
        /// 业务员
        /// </summary>
        business = 100, 

        /// <summary>
        /// 管理员
        /// </summary>
        manager = 1000,

        /// <summary>
        /// 系统管理员
        /// </summary>
        sys_manager = 10000,

        /// <summary>
        /// 超级管理员
        /// </summary>
        super_manager = 100000,
    }
}
