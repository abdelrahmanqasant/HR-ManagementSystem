using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;
using HR_ManagementSystem.Utilities;
using System;

namespace HR_ManagementSystem.ProjectProcessing
{
    public class PayrollCalculator
    {
        const int TOTAL_WORKING_DAYS_PER_MONTH = 30;
        private int[] EmpIds;
        private DateOnly PayslipStartDate;
        private DateOnly PayslipEndDate;

        private IEmployeeRepo EmployeeRepo;
        private IAttendence AttendenceRepo;
        private ICommission CommissionRepo;
        private IDeduction DeductionRepo;
        public PayrollCalculator
           (IEmployeeRepo employeeRepo,
           IAttendence attendenceRepo,
           ICommission commissionRepo,
           IDeduction deduction)
        {
            this.EmployeeRepo = employeeRepo;
            this.AttendenceRepo = attendenceRepo;
            this.CommissionRepo = commissionRepo;
            this.DeductionRepo = deduction;
        }

        private decimal BaseSalary {  get; set; }
        private decimal SalaryPerDay {  get; set; }
        private decimal SalaryPerHour {  get; set; }
        private int WorkingHours {  get; set; }
        private Employee CurrentEmployee {  get; set; }

        //payslip data

        private int AttendenceDays;
        private int OvertimeHours;
        private int AbsenceDays;
        private int LatenessHours;


        private decimal LatenessHoursPay;
        private decimal TotalDeductions;
        private decimal TotalAdditional;
        private decimal AbsenceDaysPay;
        private decimal OvertimePay;
        private decimal NetSalary;

        private decimal CalculateSalaryPerDay() { return Math.Round(BaseSalary / TOTAL_WORKING_DAYS_PER_MONTH, 2); }
        private decimal CalculateSalaryPerHour() { return Math.Round(SalaryPerDay / WorkingHours, 2); }
        private int CalculateWorkingHours() { return CurrentEmployee.Departure.Hour - CurrentEmployee.Arrival.Hour ; }
        
        private int CalculateAttendenceDays()
        {
            return AttendenceRepo.GetByPeriod(PayslipStartDate,PayslipEndDate).Where(att => att.EmpId == CurrentEmployee.Id
            && att.Status == AttendenceStatus.Present).ToList().Count();
        }
        private int CalculateAbsenceDays()
        {
            return AttendenceRepo.GetByPeriod(PayslipStartDate,PayslipEndDate).Where(att => att.EmpId == CurrentEmployee.Id
            && att.Status == AttendenceStatus.Absent).ToList().Count();
        }

        private  int CalculateOverTimeHours()
        {
            List<int> overTime = new();
            overTime = AttendenceRepo.GetByPeriod(PayslipStartDate,PayslipEndDate)
                .Where(att=> att.EmpId == CurrentEmployee.Id 
                && att.Status == AttendenceStatus.Present 
                ).Select(att => att.OvertimeInHours ?? 0)
                .ToList();
            return overTime.Sum();
        }
        private int CalculateLatenessHours ()
        {
            List<int> lateTime = new();
            lateTime = AttendenceRepo.GetByPeriod(PayslipStartDate, PayslipEndDate)
                .Where(lat => lat.EmpId == CurrentEmployee.Id 
                && 
                lat.Status == AttendenceStatus.Present).Select(lat => lat.LatetimeInHours ?? 0).ToList();
            return lateTime.Sum();  
        }

        public void SetPayrollData(DateOnly payslipStartDate, DateOnly payslipEndDate)
        {
            this.PayslipStartDate = payslipStartDate;
            this.PayslipEndDate = payslipEndDate;
        }
        public void GetEmployeeData (Employee Emp)
        {
        
            CurrentEmployee = Emp;
          
            BaseSalary = CurrentEmployee.BaseSalary;
           
            WorkingHours = CalculateWorkingHours();
          
            SalaryPerDay = CalculateSalaryPerDay();
            
            SalaryPerHour = CalculateSalaryPerHour();
          
            AttendenceDays = CalculateAttendenceDays();
            
            AbsenceDays = CalculateAbsenceDays();
          
            OvertimeHours = CalculateOverTimeHours();
            
            LatenessHours = CalculateLatenessHours();

        }
        public List<Payslip> GeneratePayslips()
        {
            List<Payslip> result = new();
            List<Employee> Employees = EmployeeRepo.GetAll();
            foreach(var emp in Employees)
            {
                GetEmployeeData(emp);
                if(CurrentEmployee == null)
                {
                    throw new NullReferenceException();
                }
              
                if(DeductionRepo.Get().type == Unit.Hour)
                {
                    int Hours = DeductionRepo.Get().Hours;
                    LatenessHoursPay = LatenessHours * Hours * SalaryPerHour;
                }
                else
                {
                    LatenessHoursPay = LatenessHours * DeductionRepo.Get().Amount;
                }
                AbsenceDaysPay = AbsenceDays * SalaryPerDay;
                TotalDeductions = LatenessHoursPay + AbsenceDaysPay;
                

                if(CommissionRepo.Get().type == Unit.Hour)
                {
                    int Hours = CommissionRepo.Get().Hours;
                    OvertimePay = OvertimeHours * Hours * SalaryPerHour;
                }
                else
                {
                    OvertimePay = OvertimeHours * CommissionRepo.Get().Amount;
                }
                TotalAdditional = OvertimePay;
                

                
                NetSalary = (BaseSalary + TotalAdditional) - TotalDeductions;
                if(NetSalary < (BaseSalary / 3))
                {
                    NetSalary = NetSalary / 3;
                }
                Payslip payslip = new()
                {
                    FullName = CurrentEmployee.FullName,
                    DepartmentName = CurrentEmployee.Department.Name,
                    BaseSalary = BaseSalary , 
                    AttendanceDays = AttendenceDays , 
                    AbsenceDays = AbsenceDays ,
                    OvertimeHours = OvertimeHours ,
                    LatenessHours = LatenessHours ,
                    TotalAdditional = TotalAdditional ,
                    TotalDeduction = TotalDeductions ,
                    NetSalary = NetSalary
                };
                result.Add( payslip );
            }
            return result;
        }
        
    }
}
