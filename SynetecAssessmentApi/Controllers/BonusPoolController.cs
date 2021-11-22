using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bonusPoolService = new BonusPoolService();

            return Ok(await bonusPoolService.GetEmployeesAsync());
        }

       

        [HttpPost()]
        [Route("GetBonusForEmployeesbyID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Ideally below two conidtions should be checked on client side application.
            if (request.SelectedEmployeeId <= 0)
                return BadRequest("Please provide correct employee ID");
            if (request.TotalBonusPoolAmount <= 0)
                return BadRequest("Please provide correct bonus pool amount");

            var bonusPoolService = new BonusPoolService();
            BonusPoolCalculatorResultDto Employee = (await bonusPoolService.CalculateAsync(request.TotalBonusPoolAmount, request.SelectedEmployeeId));

            if (Employee != null)
                return Ok(Employee);
            else
                return BadRequest("Employee ID not found");
            //return Ok(await bonusPoolService.CalculateAsync(
            //    request.TotalBonusPoolAmount,
            //    request.SelectedEmployeeId));
        }

        [HttpPost()]
        [Route("GetBonusForAllEmployees")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculateBonusForAllEmployees(int bonusPool)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Ideally below conidtion should be checked on client side application.

            if (bonusPool <= 0)
                return BadRequest("Please provide correct bonus pool amount");

            BonusPoolService bonusPoolService = new BonusPoolService();
            List<BonusPoolCalculatorResultDto> employees = await Task.Run(() => bonusPoolService.CalculateBonusAsync(bonusPool));

            if (employees != null)
                return Ok(employees);
            else
                return BadRequest("No employees found");
        }
    }
}
