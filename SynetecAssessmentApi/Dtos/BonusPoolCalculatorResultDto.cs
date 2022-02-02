
namespace SynetecAssessmentApi.Dtos
{
    

    public class BonusPoolCalculatorResultDto
    {
       
        public double Amount { get; set; }
        
        public EmployeeDto Employee { get; set; }
        
        public ErrorResponseDTO ErrorResponse { get; set; }
    }
}
