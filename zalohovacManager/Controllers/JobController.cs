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
    }
}
