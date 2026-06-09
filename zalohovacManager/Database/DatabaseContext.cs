using Microsoft.EntityFrameworkCore;
using zalohovacManager.Models;

namespace zalohovacManager.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<jobEntity> Jobs { get; set; }
        public DbSet<sourceEntity> Sources { get; set; }
        public DbSet<targetEntity> Targets { get; set; }
        public DbSet<computerEntity> Assignments { get; set; }
        public DbSet<computerEntity> Computers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=3a1_zivsafilip_db1;user=zivsafilip;password=654321;charset=utf8;");
        }
    }
}
