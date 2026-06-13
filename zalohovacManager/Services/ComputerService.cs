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
    }
}