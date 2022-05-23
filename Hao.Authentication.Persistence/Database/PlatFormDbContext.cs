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
    }
}
