using zalohovacManager.Database;
using zalohovacManager.Models;
using zalohovacManager.DTOs;

namespace zalohovacManager.Services
{
    public class JobService
    {

        private DatabaseContext _context;

        public JobService(DatabaseContext context)
        {
            _context = context;
        }


        public List<BackupJob> GetAllJobs()
        {

            var resultList = new List<BackupJob>();

            //bereme entity z databaze  
            var jobEntities = _context.Jobs.ToList();



            foreach (var entity in jobEntities)
            {
                var zdroje = _context.Sources
                    .Where(s => s.JobID == entity.ID) //filtr podle ID jobu
                    .Select(s => s.Directory) //jen string cesty
                    .ToList();

                var cile = _context.Targets
                                   .Where(t => t.JobID == entity.ID)
                                   .Select(t => t.Directory)
                                   .ToList();

                var dto = new BackupJob
                {
                    //mapujeme entity na DTO
                    Timing = entity.Timing,
                    Method = Enum.Parse<BackupMethod>(entity.Method, true),

                    Retention = new BackupRetention
                    {
                        Count = entity.RetentionCount,
                        Size = entity.RetentionSize
                    },
                    Sources = zdroje,
                    Targets = cile
                };
                resultList.Add(dto);
            }
            return resultList;
        }
    }
}

              

