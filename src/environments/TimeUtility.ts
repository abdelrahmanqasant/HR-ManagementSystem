import { IMonth } from '../app/interfaces/imonth';
export class TimeUtility {
  static formatTime(timeValue: string): string {
    if (!timeValue) {
      return '00:00:00';
    }
    const [hours, minutes] = timeValue.split(':');
    return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:00`;
  }

  static dateOnly(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  static today(): string {
    return this.dateOnly(new Date());
  }

  static formatSalaryMonth(year: number, month: number): IMonth {
    const firstDay = new Date(year, month - 1, 1);
    const lastDay = new Date(year, month, 0);
    const salaryMonth: IMonth = {
      payslipStartDate: this.dateOnly(firstDay),
      payslipEndDate: this.dateOnly(lastDay),
    };
    return salaryMonth;
  }
}
