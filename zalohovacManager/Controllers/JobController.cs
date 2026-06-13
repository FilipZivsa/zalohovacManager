//using Microsoft.AspNetCore.Components; delall mi probmem s ambiguity - Route existovalo ve dvou "slovnicich" namespaces ruznych
using Microsoft.AspNetCore.Mvc;
using zalohovacManager.Database;
using zalohovacManager.DTOs;
using zalohovacManager.Models;
using zalohovacManager.Services;

namespace zalohovacManager.Controllers

{
    [ApiController]
    [Route("api/job")]
 
    public class JobController : ControllerBase
    {
        //private DatabaseContext _context;

        //public JobController(DatabaseContext context)
        //{
        //    _context = context;
        //}

        private JobService _jobService;

        public JobController(JobService jobService)
        {
            _jobService = jobService;
        }



        [HttpGet]
        //vraci dto model backupjob
        public ActionResult<List<BackupJob>> Get()
        {
           // slepi data
            var tranlatedJobs = _jobService.GetAllJobs();

            //vraci http 200
            return Ok(tranlatedJobs);
        }




        [HttpGet("byComputer/{uuid}")]
        public ActionResult<List<BackupJob>> GetByComputer(Guid uuid)
        {
            try
            {
                var filteredJobs = _jobService.GetJobsByComputer(uuid);
                return Ok(filteredJobs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        public ActionResult Create([FromBody] BackupJob newJob)
        {
            try
            {
                _jobService.CreateJob(newJob);
                return Ok("Job uložen do db");
            }
            catch (Exception ex)
            {
                //if retention - 400
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _jobService.DeleteJob(id);
                return Ok($"Úloha s ID {id} byla úspěšně smazána včetně všech zdrojů a cílů.");
            }
            catch (Exception ex)
            {
                // Pokud Služba zahlásila naši speciální chybu, vrátíme HTTP 404 Not Found
                if (ex.Message == "NOT_FOUND")
                {
                    return NotFound($"Úloha s ID {id} nebyla v databázi nalezena.");
                }

                // Pro jakoukoliv jinou chybu vrátíme HTTP 400
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] BackupJob updatedJob)
        {
            try
            {
                // Předáme ID a nová data Službě ke zpracování
                _jobService.UpdateJob(id, updatedJob);
                return Ok($"Úloha s ID {id} byla úspěšně upravena.");
            }
            catch (Exception ex)
            {
                // Kontrola naší vlastní chyby pro nenalezený záznam
                if (ex.Message == "NOT_FOUND")
                {
                    return NotFound($"Úloha s ID {id} nebyla v databázi nalezena.");
                }

                // Chytání validačních a jiných chyb
                return BadRequest(ex.Message);
            }
        }



    }
}
