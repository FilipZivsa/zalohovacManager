using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zalohovacManager.Models;
using zalohovacManager.Services;

namespace zalohovacManager.Controllers
{
    [ApiController]
    [Route("api/computer")]
    public class ComputerController : ControllerBase
    {
        private ComputerService _computerService;

        public ComputerController(ComputerService computerService)
        {
            _computerService = computerService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<computerEntity>> Get()
        {
            return Ok(_computerService.GetAllComputers());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create([FromBody] computerEntity newComputer)
        {
            try
            {
                _computerService.CreateComputer(newComputer);
                return Ok("Počítač úspěšně přidán.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{uuid}")]
        [Authorize]
        public ActionResult Update(Guid uuid, [FromBody] computerEntity updatedComputer)
        {
            try
            {
                _computerService.UpdateComputer(uuid, updatedComputer);
                return Ok("Počítač úspěšně upraven.");
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT_FOUND") return NotFound("Počítač nenalezen.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{uuid}")]
        [Authorize]
        public ActionResult Delete(Guid uuid)
        {
            try
            {
                _computerService.DeleteComputer(uuid);
                return Ok("Počítač byl úspěšně smazán.");
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT_FOUND") return NotFound("Počítač nenalezen.");
                return BadRequest(ex.Message);
            }
        }





        [HttpPost("{uuid}/assignJob/{jobId}")]
        [Authorize]
        public ActionResult AssignJob(Guid uuid, int jobId)
        {
            try
            {
                _computerService.AssignJobToComputer(uuid, jobId);
                return Ok($"Úloha ID {jobId} byla úspěšně přiřazena počítači {uuid}.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}