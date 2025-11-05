import { Component, OnInit } from '@angular/core';
import { IPayslip } from '../../interfaces/ipayslip';
import { SalaryService } from '../../services/salary.service';
import { IMonth } from '../../interfaces/imonth';
import { TimeUtility } from '../../../environments/TimeUtility';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { FilterPipe } from '../../pipes/filter.pipe';

@Component({
  selector: 'app-salary-report',
  standalone: true,
  imports: [
    NgxPaginationModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FilterPipe,
  ],
  templateUrl: './salary-report.component.html',
  styleUrls: ['./salary-report.component.css'],
})
export class SalaryReportComponent implements OnInit {
  salaryReport: IPayslip[] = [];
  year: any;
  month: any;
  totalLength: any;
  page: number = 1;
  searchText: any;
  years: number[] = [];
  months: { value: number; name: string }[];
  constructor(private salaryService: SalaryService) {
    this.months = [
      { value: 1, name: 'January' },
      { value: 2, name: 'February' },
      { value: 3, name: 'March' },
      { value: 4, name: 'April' },
      { value: 5, name: 'May' },
      { value: 6, name: 'June' },
      { value: 7, name: 'July' },
      { value: 8, name: 'August' },
      { value: 9, name: 'September' },
      { value: 10, name: 'October' },
      { value: 11, name: 'November' },
      { value: 12, name: 'December' },
    ];
  }
  ngOnInit(): void {
    this.getYears();
  }
  getSalaryReport(month: IMonth) {
    this.salaryService.getSalaryReport(month).subscribe((data) => {
      this.salaryReport = data;
      this.totalLength = data.length;
    });
  }
  onSearch() {
    if (this.isSalaryMonth()) return;
    const salaryMonth: IMonth = TimeUtility.formatSalaryMonth(
      this.year,
      this.month
    );
    this.getSalaryReport(salaryMonth);
  }
  isSalaryMonth(): boolean {
    return !this.month || !this.year;
  }
  getYears() {
    const currentYear = new Date().getFullYear();
    for (let year = 2008; year <= currentYear; year++) this.years.push(year);
  }

  print() {
    window.print();
  }
}
