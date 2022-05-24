using Hao.Authentication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hao.Authentication.Persistence.Database
{
    public class PlatFormDbContext : DbContext
    {
        public PlatFormDbContext(DbContextOptions<PlatFormDbContext> options) : base(options) { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerInformation> CustomerInformation { get; set; }
        public DbSet<CustomerLog> CustomerLog { get; set; }
        public DbSet<CustomerRoleRelation> CustomerRoleRelation { get; set; }


        public DbSet<Program> Program { get; set; }
        public DbSet<ProgramSection> ProgramSection { get; set; }
        public DbSet<ProgramFunction> ProgramFunction { get; set; }


        public DbSet<Sys> Sys { get; set; }
        public DbSet<SysRole> SysRole { get; set; }
        public DbSet<SysProgramRelation> SysProgramRelation { get; set; }
        public DbSet<SysRoleFuncRelation> SysRoleFuncRelation { get; set; }


        public DbSet<Constraint> Constraint { get; set; }
        public DbSet<FileResource> FileResource { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        /// <summary>
        /// 保存时检查并修改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.EntityStateCheck();

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 自定义检查
        /// </summary>
        private void EntityStateCheck()
        {
            var list = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Modified) && e.Entity is BaseEntity)
                .ToList();

            list.ForEach(e =>
            {
                ((BaseEntity)e.Entity).LastModifiedAt = DateTime.Now;
                if (((BaseEntity)e.Entity).Deleted && !((BaseEntity)e.Entity).DeletedAt.HasValue)
                {
                    ((BaseEntity)e.Entity).DeletedAt = DateTime.Now;
                }
            });
        }
    }
}
