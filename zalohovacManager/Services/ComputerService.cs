using zalohovacManager.Database;
using zalohovacManager.Models;

namespace zalohovacManager.Services
{
    public class ComputerService
    {
        private DatabaseContext _context;

        public ComputerService(DatabaseContext context)
        {
            _context = context;
        }

        // 1. ČTENÍ (GET)
        public List<computerEntity> GetAllComputers()
        {
            return _context.Computers.ToList();
        }

        // 2. ZÁPIS (POST)
        public void CreateComputer(computerEntity newComputer)
        {
            // Zkontrolujeme, jestli PC s tímto UUID už náhodou neexistuje
            if (_context.Computers.Any(c => c.UUID == newComputer.UUID))
            {
                throw new Exception("Počítač s tímto UUID už v databázi existuje.");
            }

            _context.Computers.Add(newComputer);
            _context.SaveChanges();
        }

        // 3. ÚPRAVA (PUT)
        public void UpdateComputer(Guid uuid, computerEntity updatedComputer)
        {
            var pc = _context.Computers.FirstOrDefault(c => c.UUID == uuid);
            if (pc == null) throw new Exception("NOT_FOUND");

            pc.Name = updatedComputer.Name;
            pc.Enabled = updatedComputer.Enabled;

            _context.SaveChanges();
        }

        // 4. MAZÁNÍ (DELETE)
        public void DeleteComputer(Guid uuid)
        {
            var pc = _context.Computers.FirstOrDefault(c => c.UUID == uuid);
            if (pc == null) throw new Exception("NOT_FOUND");

            // Kaskádové mazání: Pokud smažeme PC, musíme smazat i jeho přiřazené úlohy v tabulce assignment
            var assignments = _context.Assignments.Where(a => a.ComputerUUID == uuid).ToList();
            _context.Assignments.RemoveRange(assignments);

            _context.Computers.Remove(pc);
            _context.SaveChanges();
        }





        public void AssignJobToComputer(Guid computerUuid, int jobId)
        {
            // 1. Zkontrolujeme, jestli počítač vůbec existuje
            if (!_context.Computers.Any(c => c.UUID == computerUuid))
            {
                throw new Exception("Počítač s tímto UUID neexistuje.");
            }

            // 2. Zkontrolujeme, jestli existuje ta úloha
            if (!_context.Jobs.Any(j => j.ID == jobId))
            {
                throw new Exception("Úloha s tímto ID neexistuje.");
            }

            // 3. VALIDACE ZE ZADÁNÍ: Zkontrolujeme, jestli už tohle propojení neexistuje
            bool alreadyAssigned = _context.Assignments.Any(a => a.ComputerUUID == computerUuid && a.JobID == jobId);
            if (alreadyAssigned)
            {
                throw new Exception("Tato úloha je již k tomuto počítači přiřazena. Duplicity nejsou povoleny.");
            }

            // 4. Vše je v pořádku, vytvoříme vazbu
            var newAssignment = new assignmentEntity
            {
                ComputerUUID = computerUuid,
                JobID = jobId,
                AssignAt = DateTime.Now // Aktuální čas na serveru
            };

            _context.Assignments.Add(newAssignment);
            _context.SaveChanges();
        }
    }
}