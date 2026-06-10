using zalohovacManager.Database;
using zalohovacManager.Models;
using zalohovacManager.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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


        public void CreateJob(BackupJob dto)
        {
            //validace 
            if(dto.Retention.Count < 0 || dto.Retention.Size < 0)
            {
                throw new Exception("Počet a velikost retence musí být kladné číslo.");
            }

            //vytvoreni ulohy job
            var newJob = new jobEntity
            {
                Timing = dto.Timing,
                Method = dto.Method.ToString().ToLower(),// Překlad Enumu (Full) zpět na malý text pro DB ("full")
                RetentionCount = dto.Retention.Count,
                RetentionSize = dto.Retention.Size
            };

            _context.Jobs.Add(newJob);

            //EF core - uloží job do MySQL a vygeneruje nové ID, EF to ID chytne a vloží zpět do newJob.ID
            _context.SaveChanges();

            //ulozeni sources
            foreach (var sourceDir in dto.Sources)
            {
                var newSource = new sourceEntity
                {
                    Directory = sourceDir,
                    JobID = newJob.ID //nove generovane ID job
                };
                _context.Sources.Add(newSource);
            }

            //ulozeni targets
            foreach (var targetDir in dto.Targets)
            {
                var newTarget = new targetEntity
                {
                    Directory = targetDir,
                    JobID = newJob.ID
                };
                _context.Targets.Add(newTarget);
            }
            
            _context.SaveChanges();
        }



        public void DeleteJob(int id)
        {
            // 1. Najdeme úlohu v databázi podle ID
            var job = _context.Jobs.FirstOrDefault(j => j.ID == id);

            // Pokud se nic nenašlo, vyhodíme chybu
            if (job == null)
            {
                throw new Exception("NOT_FOUND");
            }

            // 2. Najdeme a smažeme všechny navázané zdroje
            var zdroje = _context.Sources.Where(s => s.JobID == id).ToList();
            _context.Sources.RemoveRange(zdroje); // RemoveRange umí smazat celý seznam najednou

            // 3. Najdeme a smažeme všechny navázané cíle
            var cile = _context.Targets.Where(t => t.JobID == id).ToList();
            _context.Targets.RemoveRange(cile);

            // 4. Teď, když je čisto, můžeme smazat samotnou hlavní úlohu
            _context.Jobs.Remove(job);

            // 5. Zápis do MySQL
            _context.SaveChanges();
        }









    }
}

              

