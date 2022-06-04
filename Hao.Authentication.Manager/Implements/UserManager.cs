using AutoMapper;
using Hao.Authentication.Common.Enums;
using Hao.Authentication.Domain.Interfaces;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Domain.Paging;
using Hao.Authentication.Manager.Basic;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;
using Hao.Authentication.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Hao.Authentication.Manager.Implements
{
    public class UserManager : BaseManager, IUserManager
    {
        private readonly ILogger _logger;
        public UserManager(PlatFormDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserManager> logger)
            : base(dbContext, mapper, configuration, httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<ResponseResult<AuthResultM>> Login(LoginM model)
        {
            var res = new ResponseResult<AuthResultM>();
            try
            {
                var entity = await _dbContext.Customer.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == model.UserName && !x.Deleted);
                if (entity == null) throw new MyCustomException("账号或密码不正确！");
                var valid = HashHandler.VerifyHash(model.Password, entity.Password, entity.PasswordSalt);
                if (!valid) throw new MyCustomException("账号或密码不正确！");
                CtmView? ctm = await _dbContext.CtmView
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (ctm == null) throw new MyCustomException("未查询到账号数据！");

                var sysCode = this.GetHeader("sys");
                if (string.IsNullOrEmpty(sysCode.Value)) throw new MyCustomException("程序标识为空！");
                var pgmCode = this.GetHeader("pgm");
                if (string.IsNullOrEmpty(pgmCode.Value))
                    throw new MyCustomException("系统或程序标识为空！");
                var sysId = await this.GetCurrentSysId(sysCode.Value);
                var pgmId = await this.GetCurrentPgmId(pgmCode.Value);

                var role = await this.GetCtmRole(entity.Id, sysId);
                var record = await this.UpdateRecord(entity.Id, sysId, role.Id);
                var sects = await _dbContext.SysRoleSectView
                    .Where(x => x.Id == role.Id)
                    .Select(x => x.SectCode)
                    .ToListAsync();
                var functs = await _dbContext.SysRoleFunctView
                    .Where(x => x.Id == role.Id && x.Limited != true)
                    .Select(x => x.FunctCode)
                    .ToListAsync();

                var result = new AuthResultM();
                result.Customer = _mapper.Map<AuthCtmM>(ctm);
                result.Role = _mapper.Map<AuthRoleM>(role);
                result.SectCodes = sects;
                result.FunctCodes = functs;
                result.LoginId = record.LoginId.ToString();
                result.Token = this.BuilderToken(record.LoginId.ToString(), sysCode.Value, role.Code, entity.Name);
                res.Data = result;

                throw new Exception("test!");
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
            }
            catch (Exception e)
            {
                res.AddError(e);
                _logger.LogError(e, $"id为【{CurrentUserId}的用户登录失败");
            }
            return res;
        }



        private async Task<SysRoleView> GetCtmRole(string ctmId,string sysId)
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

        private async Task<UserLastLoginRecord> UpdateRecord(string ctmId, string sysId,string roleId)
        {
            var record = await _dbContext.UserLastLoginRecord
                    .FirstOrDefaultAsync(x => x.CustomerId == ctmId && x.SysId == sysId);
            if (record != null)
            {
                record.LoginId = Guid.NewGuid();
                record.RoleId = roleId;
                record.CreatedAt = DateTime.Now;
                record.ExpiredAt = DateTime.Now.AddDays(2);
            }
            else
            {
                record = new UserLastLoginRecord()
                {
                    LoginId = Guid.NewGuid(),
                    CustomerId = ctmId,
                    RoleId = roleId,
                    SysId = sysId,
                    CreatedAt = DateTime.Now,
                    ExpiredAt = DateTime.Now.AddDays(2),
                };
                await _dbContext.AddAsync(record);
            }
            var log = new CustomerLog()
            {
                CustomerId = ctmId,
                ProgramId = "Pgm*220528_112419001*00001260",
                Operate = "login",
                RoleId = roleId,
                RemoteAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                CreatedAt = DateTime.Now,
                Remark = "无"
            };
            await _dbContext.AddAsync(log);
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

        private async Task<string> GetCurrentPgmId(string code)
        {
            string? pgmId = await _dbContext.Program.AsNoTracking()
                .Where(x => x.Code == code && !x.Deleted)
                .Select(y => y.Id)
                .FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(pgmId)) throw new MyCustomException("未查询到当前程序信息！");
            return pgmId;
        }

        private string BuilderToken(string recordId, string sysCode, string roleCode, string userName)
        {
            var key = GetConfiguration("Platform:Key");
            var msg = new TokenMsg
            {
                Id = recordId,
                System = sysCode,
                Role = roleCode,
                Name = userName,
                Key = key
            };
            var handler = new TokenHandler();
            return handler.BuilderToken(msg);
        }
    }
}
