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
                    Id = entity.ID,

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
            // 1. Zkontrolujeme, jestli úloha vůbec existuje
            var jobExists = _context.Jobs.Any(j => j.ID == id);

            // Pokud se nic nenašlo, vyhodíme naši chybu (tu chytí Kontroler a vrátí 404)
            if (!jobExists)
            {
                throw new Exception("NOT_FOUND");
            }

            // 2. Pošleme do databáze surový SQL příkaz pro smazání. 
            // Tímto obejdeme zmatený Entity Framework a smažeme to bezpečně napřímo.
            _context.Database.ExecuteSqlRaw("DELETE FROM source WHERE job_id = {0}", id);
            _context.Database.ExecuteSqlRaw("DELETE FROM target WHERE job_id = {0}", id);

            // 3. Až po smazání zdrojů a cílů můžeme smazat samotnou úlohu
            _context.Database.ExecuteSqlRaw("DELETE FROM job WHERE id = {0}", id);
        }



        public void UpdateJob(int id, BackupJob dto)
        {
            // 1. Najdeme starou úlohu v databázi
            var job = _context.Jobs.FirstOrDefault(j => j.ID == id);

            if (job == null)
            {
                throw new Exception("NOT_FOUND");
            }

            // 2. Provedeme validaci (stejnou jako u POSTu)
            if (dto.Retention.Count < 0 || dto.Retention.Size < 0)
            {
                throw new Exception("Počet a velikost retence musí být kladné číslo.");
            }

            // 3. Přepíšeme základní vlastnosti staré úlohy na nové hodnoty z DTO
            job.Timing = dto.Timing;
            job.Method = dto.Method.ToString().ToLower();
            job.RetentionCount = dto.Retention.Count;
            job.RetentionSize = dto.Retention.Size;

            // 4. Bezpečně smažeme staré zdroje a cíle přes RAW SQL (jako u DELETE)
            _context.Database.ExecuteSqlRaw("DELETE FROM source WHERE job_id = {0}", id);
            _context.Database.ExecuteSqlRaw("DELETE FROM target WHERE job_id = {0}", id);

            // 5. Vytvoříme a přidáme NOVÉ zdroje podle toho, co nám přišlo
            foreach (var sourceDir in dto.Sources)
            {
                var newSource = new sourceEntity
                {
                    Directory = sourceDir,
                    JobID = id // Použijeme ID úlohy, kterou právě upravujeme
                };
                _context.Sources.Add(newSource);
            }

            // 6. Vytvoříme a přidáme NOVÉ cíle
            foreach (var targetDir in dto.Targets)
            {
                var newTarget = new targetEntity
                {
                    Directory = targetDir,
                    JobID = id
                };
                _context.Targets.Add(newTarget);
            }

            // 7. Zápis do databáze (Entity Framework pozná, co se změnilo, a pošle UPDATE a INSERT dotazy)
            _context.SaveChanges();
        }







    }
}

              

