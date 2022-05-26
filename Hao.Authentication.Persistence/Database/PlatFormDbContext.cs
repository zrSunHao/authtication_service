using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;
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


        public DbSet<SysCtmView> CtmView { get; set; }
        public DbSet<CtmRoleView> CtmRoleView { get; set; }
        public DbSet<CttView> CtmCttView { get; set; }
        public DbSet<CtmLastLoginView> CtmLastLoginView { get; set; }

        public DbSet<SysCtmView> SysCtmView { get; set; }
        public DbSet<SysRoleFunctView> SysRoleFunctView { get; set; }
        public DbSet<SysRoleSectView> SysRoleSectView { get; set; }
        public DbSet<SysRoleView> SysRoleView { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            modelBuilder.Entity<SysCtmView>().ToTable(typeof(SysCtmView).Name, t => t.ExcludeFromMigrations());
            modelBuilder.Entity<CtmRoleView>().ToTable(typeof(CtmRoleView).Name, t => t.ExcludeFromMigrations());
            modelBuilder.Entity<CttView>().ToTable(typeof(CttView).Name, t => t.ExcludeFromMigrations());
            modelBuilder.Entity<CtmLastLoginView>().ToTable(typeof(CtmLastLoginView).Name, t => t.ExcludeFromMigrations());

            modelBuilder.Entity<SysCtmView>().ToTable(typeof(SysCtmView).Name, t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SysRoleFunctView>().ToTable(typeof(SysRoleFunctView).Name, t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SysRoleSectView>().ToTable(typeof(SysRoleSectView).Name, t => t.ExcludeFromMigrations());
            modelBuilder.Entity<SysRoleView>().ToTable(typeof(SysRoleView).Name, t => t.ExcludeFromMigrations());
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
            var addedList = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added) && e.Entity is BaseEntity)
                .ToList();
            if (addedList.Any())
            {
                addedList.ForEach(e => { ((BaseEntity)e.Entity).CreatedAt = DateTime.Now; });
            }

            var modifiedList = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Modified) && e.Entity is BaseEntity)
                .ToList();
            if (modifiedList.Any())
            {
                modifiedList.ForEach(e =>
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
}
