import { EmployeeService } from './../../services/employee.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { IEmployee } from '../../interfaces/iemployee';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FilterPipe } from '../../pipes/filter.pipe';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-display-employee',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FilterPipe,
    NgxPaginationModule,
  ],
  templateUrl: './display-employee.component.html',
  styleUrls: ['./display-employee.component.css'],
})
export class DisplayEmployeeComponent implements OnInit {
  employees: IEmployee[] = [];
  modalRef?: BsModalRef;
  totalLength: any;
  page: number = 1;
  searchText: any;
  deleteEmp: number = 0;
  isLoading: boolean = true;

  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }
  updateForm(employee: any) {
    this.router.navigate(['/Employee/update'], {
      state: {
      employee: employee,
      employeeId: employee.id
    }
    });
  }
  onSearch() {
  this.page = 1; 
}

  loadEmployees(): void {
    this.isLoading = true;
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data as IEmployee[];
        this.totalLength = this.employees.length;
        this.isLoading = false;
        console.log('Employees loaded:', this.employees);
      },
      error: (error) => {
        console.error(' Error loading employees:', error);
        this.isLoading = false;
      },
    });
  }

  addEmployee() {
    this.router.navigate(['Employee/add']);
  }

  deleteEmployee(id: number): void {
    this.modalRef?.hide();
    this.employeeService.deleteEmployee(id).subscribe({
      next: () => {
        this.employees = this.employees.filter((emp) => emp.id !== id);
        this.totalLength = this.employees.length;
      },
      error: (error) => {
        console.error('Error deleting employee:', error);
      },
    });
  }

  trackByFn(index: number, employee: IEmployee) {
    return employee.id;
  }

  decline() {
    this.modalRef?.hide();
  }

  openModal(template: TemplateRef<void>, id: number) {
    this.deleteEmp = id;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
}
