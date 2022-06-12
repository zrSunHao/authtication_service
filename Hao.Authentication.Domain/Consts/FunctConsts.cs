namespace Hao.Authentication.Domain.Consts
{
    public class CttFunct
    {
        public const string GetList = "ctt_get_list";    // 获取约束列表数据

        public const string Cancel = "ctt_cancel";       // 取消约束
    }

    public class CtmFunct
    {
        public const string GetList = "ctm_get_list";            // 获取客户列表数据

        public const string AddRemark = "ctm_remark_add";        // 添加客户备注

        public const string ResetPsd = "ctm_psd_reset";          // 重置客户密码

        public const string GetRoleList = "ctm_role_get_list";   // 获取客户角色列表

        public const string AddRole = "ctm_role_add";            // 添加客户角色

        public const string UpdateRole = "ctm_role_update";      // 更新客户角色

        public const string DeleteRole = "ctm_role_delete";      // 删除客户角色

        public const string GetCttList = "ctm_ctt_get_list";     // 获取客户约束列表

        public const string AddCtt = "ctm_ctt_add";              // 添加客户约束

        public const string GetLogList = "ctm_log_get_list";     // 获取客户日志列表
    }

    public class PgmFunct
    {
        public const string GetList = "pgm_get_list";            // 获取程序列表数据

        public const string Add = "pgm_add";                     // 添加程序

        public const string Update = "pgm_update";               // 更新程序

        public const string Delete = "pgm_delete";               // 删除程序

        public const string GetSectList = "pgm_sect_get_list";   // 获取模块/页面的数据列表

        public const string AddSect = "pgm_sect_add";            // 添加模块/页面

        public const string UpdateSect = "pgm_sect_update";      // 更新模块/页面

        public const string DeleteSect = "pgm_sect_delete";      // 删除模块/页面

        public const string AddFunct = "pgm_funct_add";          // 添加功能

        public const string UpdateFunct = "pgm_funct_update";    // 更新功能

        public const string DeleteFunct = "pgm_funct_delete";    // 删除功能

        public const string GetFunctList = "pgm_funct_get_list"; // 获取程序功能列表

        public const string AddCtm = "pgm_ctm_add";              // 添加关联用户

        public const string DeleteCtm = "pgm_ctm_delete";        // 删除关联用户

        public const string GetOwnedCtmList = "pgm_ctm_get_owend_list";          // 获取关联用户列表数据

        public const string GetNotOwnedCtmList = "pgm_ctm_get_notowend_list";    // 获取未关联用户列表数据
    }

    public class ReportFunct
    {
        public const string GetWidgetList = "rpt_widget_get_list";           // 获取模块总量数据

        public const string GetRecentLoginCtmList = "rpt_login_get_list";    // 获取最近登录列表数据

        public const string GetRecentLogList = "rpt_log_get_list";           // 获取操作最近日志列表数据

        public const string GetCtts = "rpt_ctt_get_list";                    // 获取最近添加约束数据
    }

    public class ResourceFunct
    {
        public const string Save = "rsuc_save";                  // 上传文件

        public const string GetByCode = "rsuc_get_by_code";      // 获取文件信息
    }

    public class SysFunct
    {
        public const string Add = "sys_add";                     // 添加系统

        public const string Update = "sys_update";               // 更新系统

        public const string GetList = "sys_get_list";            // 获取系统列表数据

        public const string Delete = "sys_delete";               // 删除系统

        public const string AddPgm = "sys_pgm_add";              // 添加关联程序

        public const string DeletePgm = "sys_pgm_delete";        // 删除关联程序

        public const string GetNotOwnedPgmList = "sys_pgm_get_notowned_list";   // 获取未关联程序列表

        public const string GetOwnedPgmList = "sys_pgm_get_owned_list";         // 获取关联程序列表

        public const string GetCtms = "sys_ctm_get_list";        // 获取客户列表数据

        public const string AddCtmCtt = "sys_ctm_add";           // 添加客户约束

        public const string CancelCtmCtt = "sys_ctm_cancel";     // 取消客户约束

        public const string AddRole = "sys_role_add";            // 添加角色

        public const string UpdateRole = "sys_role_update";      // 更新角色

        public const string GetRoleList = "sys_role_get_list";   // 获取角色列表数据

        public const string DeleteRole = "sys_role_delete";      // 删除角色

        public const string GetRolePgmList = "sys_role_pgm_get_list";        // 获取角色关联程序功能列表数据

        public const string UpdateRolePgmFuncts = "sys_role_pgm_update";     // 更新角色关联程序功能
    }

    public class UserFunct
    {
        public const string GetLogList = "user_log_get_list";           // 获取用户日志列表数据

        public const string CheckPrivilege = "user_previlege_check";    // 校验用户登录状态、权限
    }
}
