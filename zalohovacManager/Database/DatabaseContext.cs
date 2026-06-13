using Microsoft.EntityFrameworkCore;
using zalohovacManager.Models;

namespace zalohovacManager.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<jobEntity> Jobs { get; set; }
        public DbSet<sourceEntity> Sources { get; set; }
        public DbSet<targetEntity> Targets { get; set; }
        public DbSet<assignmentEntity> Assignments { get; set; }
        public DbSet<computerEntity> Computers { get; set; }

      
    }
}
