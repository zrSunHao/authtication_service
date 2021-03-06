namespace Hao.Authentication.Domain.Consts
{
    public class CacheConsts
    {
        public static string ROLE_PROGRAM_SECTS(string roleId, string pgmCode) => $"{roleId}-{pgmCode}-sects";

        public static string ROLE_PROGRAM_FUNCTS(string roleId, string pgmCode) => $"{roleId}-{pgmCode}-functs";

        /**
         * 要缓存的数据：
         * 1、roleId => role
         * 2、roleId-programId-sects => sectCodes
         * 3、roleId-programId-functs => functCodes
         * 4、loginId => record
         * **/

        /**
         * 角色
         * 
         * 1、清除时机：
         * 1.1、角色被删除
         * 1.2、角色受到约束
         * 1.3、角色关联程序功能更改
         * 
         * 2、数据库更改：
         * 2.1、最近登录记录未过期的改为过期 (1.3除外)
         * 
         * 3、缓存清除key：
         * 3.1、角色id (1.3除外)
         * 3.2、角色id - 程序码 - sects 
         * 3.3、角色id - 程序码 - functs
         * 3.4、最近登录Id（根据登录角色筛选）(1.3除外)
         * 
         * **/

        /**
         * 程序
         * 
         * 1、清除时机
         * 1.1、删除程序
         * 1.2、模块增、删、改
         * 1.3、功能增、删、改
         * 1.4、功能受到约束/解除约束
         * 
         * 2、数据库更改：无
         * 
         * 3、缓存清除key：
         * 3.1、角色id - 程序码 - sects 
         * 3.2、角色id - 程序码 - funccts
         * 
         * 4、程序功能受到的约束会失效：
         *    手动刷新某程序功能的缓存
         *    自动每10分钟一次
         * 4.1、角色id - 程序码 - sects 
         * 4.2、角色id - 程序码 - funccts 
         * 
         * **/

        /**
         * 客户
         * 
         * 1、清除时机
         * 1.1、受到all-sys约束
         * 1.2、受到one-sys约束
         * 1.3、在某系统的角色被变更
         * 
         * 2、数据库更改：
         * 2.1、最近登录记录未过期的改为过期
         * 
         * 3、缓存清除key：
         * 3.1、最近登录Id（1.1与1.2根据用户Id筛选）
         * 3.1、最近登录Id（1.3根据登录角色筛选）
         * 
         * **/
    }

    /**
     * 敏感数据权限约束
     * 普通用户、会员无权访问
     * 业务员、管理员：客户管理、约束管理页面无权访问，系统管理、程序管理按是否关联访问
     * 系统管理员：全部数据，但不能操作超级管理员的数据
     * 超级管理员：全部
     * 
     * 系统客户管理：业务员、管理员只能管理比其在系统角色级别低的客户数据
     * 日志管理：默认查看自己的，业务员、管理员查看其关联且具有管理员角色系统的低级别数据
     * 
     * **/
}
