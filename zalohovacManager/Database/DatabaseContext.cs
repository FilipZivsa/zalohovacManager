using Microsoft.EntityFrameworkCore;

namespace zalohovacManager.Database
{
    public class DatabaseContext : DbContext
    {
        //public DbSet<Car> Cars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=3a1_zivsafilip_db1;user=zivsafilip;password=654321;charset=utf8;");
        }
    }
}
