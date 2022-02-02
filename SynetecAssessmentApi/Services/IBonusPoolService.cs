using System.Collections.Generic;
using System.Threading.Tasks;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;

namespace SynetecAssessmentApi.Services
{
    public interface IBonusPoolService
    {
        Task<BonusPoolCalculatorResultDto> CalculateAsync(decimal bonusPoolAmount, int selectedEmployeeId);
    }
}