import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IMonth } from '../interfaces/imonth';
import { IPayslip } from '../interfaces/ipayslip';

@Injectable({
  providedIn: 'root',
})
export class SalaryService {
  private baseURL = environment.baseUrl;

  constructor(private http: HttpClient) {}
  getSalaryReport(month: IMonth): Observable<any> {
    return this.http.get(`${this.baseURL}/Salary/payslip`, {
      params: {
        payslipStartDate: month.payslipStartDate,
        payslipEndDate: month.payslipEndDate,
      },
    });
  }
  printSinglePayslip(record: IPayslip) {
    return this.http.post(`${this.baseURL}/Salary/print-single`, record, {
      responseType: 'blob',
    });
  }
}
