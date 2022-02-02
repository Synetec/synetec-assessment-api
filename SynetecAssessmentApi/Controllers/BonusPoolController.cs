using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        private readonly IBonusPoolService _bonusPoolService;


        public BonusPoolController(IBonusPoolService bonusPoolService)
        {
            _bonusPoolService = bonusPoolService;
        }

        [HttpPost()]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            if (request.SelectedEmployeeId == 0 || request.TotalBonusPoolAmount == 0)
            {
                return BadRequest("Please provide valid employee id and bonus pool amount.");
            }
            

            var response = await _bonusPoolService.CalculateAsync(
                request.TotalBonusPoolAmount,
                request.SelectedEmployeeId);
           

                return Ok(response);
        }
    }
}
