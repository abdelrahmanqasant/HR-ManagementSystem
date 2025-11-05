using HR_ManagementSystem.Models;
using HR_ManagementSystem.ProjectProcessing;
using HR_ManagementSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HR_ManagementSystem.Utilities.Permission;
namespace HR_ManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SalaryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      [Authorize(Salary.View)]
        [HttpGet("payslip")]
        public IActionResult GetPayslip([FromQuery] DateOnly payslipStartDate, [FromQuery] DateOnly payslipEndDate)
        {
            PayrollCalculator payrollCalculator = new(_unitOfWork.employeeRepo, _unitOfWork.Attendence, _unitOfWork.Commission, _unitOfWork.deduction);
            payrollCalculator.SetPayrollData(payslipStartDate, payslipEndDate);
            List<Payslip> payslips = payrollCalculator.GeneratePayslips();
            return Ok(payslips);
        }
       

    }
}
