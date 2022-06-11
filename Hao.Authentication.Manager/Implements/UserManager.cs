using AutoMapper;
using Hao.Authentication.Common.Enums;
using Hao.Authentication.Domain.Consts;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Manager.Providers;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;
using Hao.Authentication.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hao.Authentication.Manager.Implements
{
    public class UserManager : BaseManager, IUserManager
    {
        private readonly ILogger _logger;
        private readonly IPrivilegeManager _privilege;
        public UserManager(PlatFormDbContext dbContext,
            IMapper mapper,
            ICacheProvider cache,
            IMyLogProvider myLog,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IPrivilegeManager privilege,
            ILogger<UserManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor, cache, myLog)
        {
            _logger = logger;
            _privilege = privilege;
        }

        public async Task<ResponseResult<AuthResultM>> Login(LoginM model)
        {
            var res = new ResponseResult<AuthResultM>();
            try
            {
                var sysCode = model.SysCode;
                if (string.IsNullOrEmpty(sysCode)) throw new MyCustomException("系统标识为空！");
                var pgmCode = model.PgmCode;
                if (string.IsNullOrEmpty(pgmCode)) throw new MyCustomException("程序标识为空！");

                var entity = await _dbContext.Customer.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == model.UserName && !x.Deleted);
                if (entity == null) throw new MyCustomException("账号或密码不正确！");
                var valid = HashHandler.VerifyHash(model.Password, entity.Password, entity.PasswordSalt);
                if (!valid) throw new MyCustomException("账号或密码不正确！");
                CtmView? ctm = await _dbContext.CtmView
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (ctm == null) throw new MyCustomException("未查询到账号数据！");

                var sysId = await this.GetCurrentSysId(sysCode);
                var pgmId = await this.GetCurrentPgmId(sysId, pgmCode);
                await this.CtmCtts(ctm.Id, sysId);

                var role = await this.GetCtmRole(entity.Id, sysId);
                var record = await this.UpdateRecord(entity.Id, sysId, pgmId, role.Id);
                var sects = await _privilege.GetPgmSectCodes(role.Id, pgmCode);
                var functs = await _privilege.GetPgmFunctCodes(role.Id, pgmCode);

                ctm.Avatar = this.BuilderFileUrl(ctm.Avatar, record.LoginId.ToString());
                var result = new AuthResultM();
                result.Customer = _mapper.Map<AuthCtmM>(ctm);
                result.Role = _mapper.Map<AuthRoleM>(role);
                result.SectCodes = sects;
                result.FunctCodes = functs;
                result.LoginId = record.LoginId.ToString();
                result.Token = this.BuilderToken(record.LoginId.ToString(), sysCode, role.Code, entity.Name, record.ExpiredAt);
                res.Data = result;

                var r = _mapper.Map<UserLastLoginRecordM>(record);
                _myLog.Add(r, "登录", $"无", RemoteIpAddress);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户【{model.UserName}登录失败");
            }
            return res;
        }

        private static object _lock = new object();
        public async Task<ResponseResult<bool>> Register(RegisterM model)
        {
            var res = new ResponseResult<bool>();
            try
            {
                lock (_lock)
                {
                    var exist = _dbContext.Customer.Any(x => x.Name == model.UserName);
                    if (exist) throw new MyCustomException("账号已存在！");
                }

                var ctm = new Customer
                {
                    Name = model.UserName,
                    Nickname = model.NickName,
                    Remark = "无",
                    CreatedAt = DateTime.Now,
                };
                ctm.Id = ctm.GetId(MachineCode);
                HashHandler.CreateHash(model.Password, out var hash, out var salt);
                ctm.Password = hash;
                ctm.PasswordSalt = salt;
                await _dbContext.AddAsync(ctm);

                var info = _mapper.Map<CustomerInformation>(model);
                info.CustomerId = ctm.Id;
                await _dbContext.AddAsync(info);
                await _dbContext.SaveChangesAsync();

                var r = new UserLastLoginRecordM()
                {
                    Id = 0,
                    LoginId = Guid.Empty,
                    CustomerId = ctm.Id,
                    SysId = "",
                    PgmId = "",
                    RoleId = "",
                    CreatedAt = DateTime.Now,
                    ExpiredAt = DateTime.Now
                };
                _myLog.Add(r, "注册", $"无", RemoteIpAddress);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户【{model.UserName}注册失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> ResetPsd(string oldPsd, string newPsd)
        {
            var res = new ResponseResult<bool>();
            try
            {
                var entity = await _dbContext.Customer
                    .FirstOrDefaultAsync(x => x.Id == CurrentUserId);
                if (entity == null) throw new MyCustomException("未查询到客户数据！");
                var valid = HashHandler.VerifyHash(oldPsd, entity.Password, entity.PasswordSalt);
                if (!valid) throw new MyCustomException("原密码不正确！");

                HashHandler.CreateHash(newPsd, out var hash, out var salt);
                entity.Password = hash;
                entity.PasswordSalt = salt;
                entity.LastModifiedAt = DateTime.Now;
                entity.LastModifiedById = CurrentUserId;
                await _dbContext.SaveChangesAsync();

                _myLog.Add(LoginRecord, "重置密码", $"自己操作！", RemoteIpAddress);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"id为【{CurrentUserId}的用户登录失败");
            }
            return res;
        }

        public async Task<ResponseResult<bool>> Logout()
        {
            var res = new ResponseResult<bool>();
            try
            {
                var loginId = this.CurrentLoginId;
                var entity = await _dbContext.UserLastLoginRecord.FirstOrDefaultAsync(x => x.LoginId == loginId);
                if (entity != null)
                {
                    entity.ExpiredAt = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                _myLog.Add(LoginRecord, "登出", $"无", RemoteIpAddress);
                _cache.Remove(loginId.ToString());
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"用户登出失败");
            }
            return res;
        }

        public void AddLog(string operate, string remark = "")
        {
            _myLog.Add(LoginRecord, operate, remark, RemoteIpAddress);
        }

        public async Task<ResponsePagingResult<UserLogM>> GetLogList(PagingParameter<UserLogFilter> param)
        {
            var res = new ResponsePagingResult<UserLogM>();
            try
            {
                var filter = param.Filter;
                var query = _dbContext.CustomerLogView.AsNoTracking().AsQueryable();
                var role = await GetCurrentUserRole();
                if (string.IsNullOrEmpty(filter?.SysId))
                {
                    query = query.Where(x => x.CtmId == CurrentUserId);
                }
                else if (role.Rank == SysRoleRank.manager || role.Rank == SysRoleRank.business)
                {
                    var sr = await _dbContext.SysCtmView.AsNoTracking()
                        .Where(x => x.Id == CurrentUserId && x.SysId == filter.SysId)
                        .FirstOrDefaultAsync();
                    if (sr == null || sr.RoleRank < SysRoleRank.business)
                        query = query.Where(x => x.CtmId == CurrentUserId && x.SysId == filter.SysId);
                    else query = query.Where(x => x.RoleRank != null && x.RoleRank < sr.RoleRank && x.SysId == filter.SysId);
                }
                if (role.Rank > SysRoleRank.manager)
                    query = query.Where(x => x.SysId == filter.SysId);

                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                        query = query.Where(x => x.CtmName.Contains(filter.Name)
                        || x.CtmNickname.Contains(filter.Name));
                    if (!string.IsNullOrEmpty(filter.OperateOrRemark))
                        query = query.Where(x => x.Operate.Contains(filter.OperateOrRemark)
                        || x.Remark.Contains(filter.OperateOrRemark));
                    if (filter.StartAt.HasValue)
                        query = query.Where(x => x.CreatedAt >= filter.StartAt.Value);
                    if (filter.EndAt.HasValue)
                        query = query.Where(x => x.CreatedAt <= filter.EndAt.Value.AddDays(1).AddSeconds(-1));
                }

                query = query.OrderByDescending(x => x.CreatedAt);
                if (param.Sort != null && param.Sort.ToLower() == "asc")
                    query = query.OrderBy(x => x.CreatedAt);

                res.RowsCount = await query.CountAsync();
                query = query.AsPaging(param.PageIndex, param.PageSize);
                var data = await query.ToListAsync();
                res.Data = _mapper.Map<List<UserLogM>>(data);
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"获取日志列表失败");
            }
            return res;
        }



        private async Task<bool> CtmCtts(string ctmId, string sysId)
        {
            var ctts = await _dbContext.CttView
                .Where(x => x.CtmId == ctmId && (x.SysId == null || x.SysId == sysId))
                .ToListAsync();
            if (ctts.Any())
            {
                var msgs = new List<string>();
                ctts.ForEach(x =>
                {
                    if (x.Category == ConstraintCategory.customer_all_system)
                    {
                        if (x.Method == ConstraintMethod.forbid) msgs.Add("账号已被禁用！");
                        else if (x.Method == ConstraintMethod._lock) msgs.Add($"账号已被锁定至{x.ExpiredAt?.ToString("F")}！");
                    }
                    if (x.Category == ConstraintCategory.customer_one_system)
                    {
                        if (x.Method == ConstraintMethod.forbid) msgs.Add("账号已被该系统禁用！");
                        else if (x.Method == ConstraintMethod._lock) msgs.Add($"账号已被该系统锁定至{x.ExpiredAt?.ToString("F")}！");
                    }
                });
                throw new MyCustomException(string.Join(' ', msgs));
            }

            return true;
        }

        private async Task<SysRoleView> GetCtmRole(string ctmId, string sysId)
        {
            SysRoleView? role = await (from cr in _dbContext.CustomerRoleRelation
                                       join r in _dbContext.SysRoleView on cr.RoleId equals r.Id
                                       where cr.CustomerId == ctmId && r.SysId == sysId
                                       select r).FirstOrDefaultAsync();
            if (role == null)
            {
                role = await _dbContext.SysRoleView
                         .FirstOrDefaultAsync(x => x.Rank == SysRoleRank._default && x.SysId == sysId);
                if (role != null)
                {
                    var rl = new CustomerRoleRelation()
                    {
                        CustomerId = ctmId,
                        RoleId = role.Id,
                        SysId = sysId,
                        CreatedAt = DateTime.Now,
                        CreatedById = ctmId,
                        Remark = "登陆时自动赋予！"
                    };
                    await _dbContext.AddAsync(rl);
                }
            }
            if (role == null) throw new MyCustomException("您没有权限访问该系统，如有问题，请联系管理员！");
            else if (role.CttMethod.HasValue) throw new MyCustomException($"系统角色【{role.Name}】已被禁用，如有问题，请联系管理员！");
            return role;
        }

        private async Task<UserLastLoginRecord> UpdateRecord(string ctmId, string sysId, string pgmId, string roleId)
        {
            var record = await _dbContext.UserLastLoginRecord
                    .FirstOrDefaultAsync(x => x.CustomerId == ctmId && x.SysId == sysId && x.PgmId == pgmId);
            if (record != null)
            {
                _cache.Remove(record.LoginId.ToString());
                record.LoginId = Guid.NewGuid();
                record.RoleId = roleId;
                record.CreatedAt = DateTime.Now;
                record.ExpiredAt = DateTime.Now.AddDays(3);
            }
            else
            {
                record = new UserLastLoginRecord()
                {
                    LoginId = Guid.NewGuid(),
                    CustomerId = ctmId,
                    RoleId = roleId,
                    PgmId = pgmId,
                    SysId = sysId,
                    CreatedAt = DateTime.Now,
                    ExpiredAt = DateTime.Now.AddDays(2),
                };
            }
            _cache.Save(record.LoginId.ToString(), record);
            await _dbContext.SaveChangesAsync();
            return record;
        }

        private async Task<string> GetCurrentSysId(string code)
        {
            string? sysId = await _dbContext.Sys.AsNoTracking()
                .Where(x => x.Code == code && !x.Deleted)
                .Select(y => y.Id)
                .FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(sysId)) throw new MyCustomException("未查询到当前系统信息！");
            return sysId;
        }

        private async Task<string> GetCurrentPgmId(string sysId, string code)
        {
            string? pgmId = await (from sp in _dbContext.SysProgramRelation
                                   join p in _dbContext.Program on sp.ProgramId equals p.Id
                                   where !sp.Deleted && sp.SysId == sysId && p.Code == code && !p.Deleted
                                   select p.Id).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(pgmId)) throw new MyCustomException("未查询到当前程序信息！");
            return pgmId;
        }

        private string BuilderToken(string recordId, string sysCode, string roleCode, string userName, DateTime expiredAt)
        {
            var key = GetConfiguration(CfgConsts.PLATFORM_KEY);
            var issuer = GetConfiguration(CfgConsts.PLATFORM_ISSUER);
            var msg = new TokenMsg
            {
                Id = recordId,
                System = sysCode,
                Role = roleCode,
                Name = userName,
                ExpiredAt = expiredAt,
                Key = key,
                Issuer = issuer,
            };
            var handler = new TokenHandler();
            return handler.BuilderToken(msg);
        }
    }
}
