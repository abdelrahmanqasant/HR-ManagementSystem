export interface IPayslip {
  fullName: string;
  departmentName: string;
  baseSalary: number;
  attendanceDays: number;
  absenceDays: number;
  overtimeHours: number;
  latenessHours: number;
  totalAdditional: number;
  totalDeduction: number;
  netSalary: number;
}
