using System;
using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SynetecAssessmentApi.Exceptions;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        private readonly IBonusPoolService _bonusPoolService;
        private readonly ILogger<BonusPoolController> _logger;
        public BonusPoolController(IBonusPoolService bonusPoolService, ILogger<BonusPoolController> logger)
        {
            _bonusPoolService = bonusPoolService ?? throw new ArgumentNullException(nameof(bonusPoolService), $"{nameof(bonusPoolService)} cannot be null");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null");
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _bonusPoolService.GetEmployeesAsync());
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(500);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            try
            {
                return Ok(await _bonusPoolService.CalculateAsync(
                request.TotalBonusPoolAmount,
                request.SelectedEmployeeId));
            }
            catch(InvalidEmployeeIdException employeeException)
            {
                _logger.LogError(employeeException.Message);
                return BadRequest(employeeException.Message);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return StatusCode(500);
            }
        }
    }
}
