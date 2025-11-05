import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DepartmentService } from '../../services/department.service';
import { EmployeeService } from '../../services/employee.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { IEmployee } from '../../interfaces/iemployee';
import { TimeUtility } from '../../../environments/TimeUtility';

@Component({
  selector: 'app-update-employee',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './update-employee.component.html',
  styleUrls: ['./update-employee.component.css'],
})
export class UpdateEmployeeComponent implements OnInit {
  validationUpdateEmployee: FormGroup;
  selectedEmployee!: IEmployee;
  departments: any[] = [];
  allCountries: string[] = [];

  employeeId!: number;
  modalRef?: BsModalRef;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {
    this.validationUpdateEmployee = this.fb.group({
      ssn: [
        '',
        [
          Validators.required,
          Validators.minLength(14),
          Validators.maxLength(14),
        ],
      ],
      fullName: ['', Validators.required],
      address: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern('[0-9]{11}')]],
      gender: [0, Validators.required],
      nationality: ['', Validators.required],
      birthDate: ['', Validators.required],
      contractDate: ['', Validators.required],
      baseSalary: ['', Validators.required],
      arrival: ['', Validators.required],
      departure: ['', Validators.required],
      departmentName: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.selectedEmployee = history.state.employee;

    if (!this.selectedEmployee || !this.selectedEmployee.id) {
      this.toastr.error('No employee data found');
      this.router.navigate(['/Employee/display']);
      return;
    }

    this.employeeId = this.selectedEmployee.id;

  
    this.validationUpdateEmployee.patchValue({
      ssn: this.selectedEmployee.ssn,
      fullName: this.selectedEmployee.fullName,
      address: this.selectedEmployee.address,
      phoneNumber: this.selectedEmployee.phoneNumber,
      gender: this.selectedEmployee.gender,
      nationality: this.selectedEmployee.nationality,
      birthDate: this.selectedEmployee.birthDate,
      contractDate: this.selectedEmployee.contractDate,
      baseSalary: this.selectedEmployee.baseSalary,
      arrival: this.selectedEmployee.arrival,
      departure: this.selectedEmployee.departure,
      departmentName: this.selectedEmployee.departmentName,
    });

    this.departmentService.getDepartments().subscribe((data) => {
      this.departments = data as any[];
    });

    this.allCountries = this.employeeService.allCountriesList;
  }

  openModal(template: TemplateRef<void>) {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  decline() {
    this.modalRef?.hide();
  }


  UpdateEmployeeBtn() {
    if (!this.validationUpdateEmployee.valid) {
      this.toastr.error('Form is invalid');
      return;
    }

    let updatedEmployee: IEmployee = {
      id: this.employeeId,
      ...this.validationUpdateEmployee.value,
    };

    updatedEmployee.arrival = TimeUtility.formatTime(updatedEmployee.arrival);
    updatedEmployee.departure = TimeUtility.formatTime(
      updatedEmployee.departure
    );

    this.update(updatedEmployee);
    this.modalRef?.hide();
  }

  update(employee: IEmployee) {
    this.employeeService.updateEmployee(employee.id!, employee).subscribe(
      (response) => {
        this.toastr.success('Updated Successfully');
        this.router.navigate(['/Employee/display']);
      },
      (error) => {
        console.error('Error updating employee:', error);
      }
    );
  }
  get address() {
    return this.validationUpdateEmployee.get('address');
  }

  get phoneNumber() {
    return this.validationUpdateEmployee.get('phoneNumber');
  }

  get arrival() {
    return this.validationUpdateEmployee.get('arrival');
  }

  get departure() {
    return this.validationUpdateEmployee.get('departure');
  }

  get departmentName() {
    return this.validationUpdateEmployee.get('departmentName');
  }

  get fullName() {
    return this.validationUpdateEmployee.get('fullName');
  }

  get gender() {
    return this.validationUpdateEmployee.get('gender');
  }

  get nationality() {
    return this.validationUpdateEmployee.get('nationality');
  }

  get birthDate() {
    return this.validationUpdateEmployee.get('birthDate');
  }

  get contractDate() {
    return this.validationUpdateEmployee.get('contractDate');
  }

  get baseSalary() {
    return this.validationUpdateEmployee.get('baseSalary');
  }

  get ssn() {
    return this.validationUpdateEmployee.get('ssn');
  }

  onCancel() {
    this.router.navigate(['/Employee/display']);
  }
}
