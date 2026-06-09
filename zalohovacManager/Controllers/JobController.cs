//using Microsoft.AspNetCore.Components; delall mi probmem s ambiguity - Route existovalo ve dvou "slovnicich" namespaces ruznych
using Microsoft.AspNetCore.Mvc;
using zalohovacManager.Database;
using zalohovacManager.Models;

namespace zalohovacManager.Controllers

{
    [ApiController]
    [Route("api/job")]
 
    public class JobController : ControllerBase
    {
        private DatabaseContext _context;

        public JobController(DatabaseContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<List<jobEntity>> Get()
        {
            var jobs= _context.Jobs.ToList();
            return Ok(jobs);
        }

    }
}
