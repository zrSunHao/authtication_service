using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Manager.RabbitMq;
using Hao.Authentication.Persistence.Database;
using Hao.Authentication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hao.Authentication.Manager.Providers
{
    public class MyLogProvider : IMyLogProvider
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRabbitMqDirectSender _mqSender;
        private readonly PlatFormDbContext _dbContext;
        public MyLogProvider(PlatFormDbContext dbContext,
            IMapper mapper,
            IRabbitMqDirectSender mqSender,
            ILogger<MyLogProvider> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _mqSender = mqSender;
        }

        public void Add(UserLastLoginRecordM record, string operate, string remark = "", string? remote = "")
        {
            try
            {
                if (string.IsNullOrEmpty(remark)) remark = "无";
                var log = new LogM()
                {
                    CustomerId = record.CustomerId,
                    SystemId = record.SysId,
                    RoleId = record.RoleId,
                    ProgramId = record.PgmId,
                    Operate = operate,
                    Remark = remark,
                    RemoteAddress = remote,
                    CreatedAt = DateTime.Now,
                };
                var msg = new RabbitMqMsg<LogM>()
                {
                    Category = MsgCategory.log,
                    Value = log,
                };
                _mqSender.SendMsg(msg);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"发送日志信息失败！ {record.LoginId} ---> {operate} ---> {remark}");
            }
        }

        public async Task<int> Save(LogM log)
        {
            try
            {
                //var exist = await _dbContext.CustomerLog
                //    .AnyAsync(x => x.CustomerId == log.CustomerId && x.Operate == log.Operate
                //    && x.SystemId == log.SystemId && x.ProgramId == log.ProgramId
                //    && x.RoleId == log.RoleId && x.Remark == log.Remark
                //    && x.CreatedAt > DateTime.Now.AddMinutes(-5));

                var entity = _mapper.Map<CustomerLog>(log);
                _dbContext.Add(entity);
                var count = await _dbContext.SaveChangesAsync();
                return count;
            }
            catch(Exception e)
            {
                _logger.LogError(e,$"日志保存失败  ======> {log.CustomerId} - {log.SystemId} - {log.ProgramId} - {log.RoleId}  ");
                return 1;
            }
        }
    }
}
