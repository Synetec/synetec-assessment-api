using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Services;
using System;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bonusPoolService = new BonusPoolService();

            try
            {
                return Ok(await bonusPoolService.GetEmployeesAsync());
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        /// <summary>
        /// Retrieves Bonus for the selected employee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            var bonusPoolService = new BonusPoolService();

            if (bonusPoolService.isBonusValid(request))
            {
                try
                {
                    var response = await bonusPoolService.CalculateAsync(
                    request.TotalBonusPoolAmount,
                    request.SelectedEmployeeId);

                    return Ok(response);
                }
                catch (Exception e)
                {
                    return NotFound(e.Message);
                }
            }
            else
            {
                return NotFound("Values incorrectly specified");
            }
        }
    }
}
