import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { IDepartment } from '../../interfaces/idepartment';
import { EmployeeService } from '../../services/employee.service';
import { DepartmentService } from '../../services/department.service';
import { ToastrService } from 'ngx-toastr';
import { calculateAge } from '../../calculateAge';
import { TimeUtility } from '../../../environments/TimeUtility';
import { Router } from '@angular/router';
import { minPeriodValidator } from '../../MinimumPeriodClocked';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css'],
})
export class AddEmployeeComponent implements OnInit {
  validationEmployee: FormGroup;
  modalRef?: BsModalRef;
  employeeDTO: any = {};
  selectedDepartment: string = '';
  departments: IDepartment[] = [];
  allCountries: string[] = [];
  employees: any = [];
  isSubmitted: boolean = false;

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {
    this.validationEmployee = this.fb.group(
      {
        ssn: [
          '',
          [
            Validators.required,
            Validators.minLength(14),
            Validators.maxLength(14),
            Validators.pattern('[0-9]{14}'),
          ],
        ],
        fullName: [
          '',
          [Validators.required, Validators.pattern('^[a-zA-Z ]+$')],
        ],
        address: ['', [Validators.required]],
        phoneNumber: [
          '',
          [
            Validators.required,
            this.validatePhoneNumber,
            Validators.pattern('[0-9]{11}'),
          ],
        ],
        gender: [0, Validators.required],
        nationality: ['', Validators.required],
        birthDate: ['', [Validators.required, this.validateBirthDate]],
        contractDate: ['', [Validators.required, this.contractDateValidator]],
        baseSalary: [
          '',
          [
            Validators.required,
            Validators.pattern('[0-9]*'),
            this.BasedSalaryValidation,
          ],
        ],
        arrival: ['', Validators.required],
        departmentName: ['', Validators.required],
        departure: ['', Validators.required],
      },
      { validator: minPeriodValidator() }
    );
  }



  validatePhoneNumber(control: FormControl) {
    const phoneNumber = control.value;
    const isValidPhoneNumber = /^\d{11}$/.test(phoneNumber);
    if (!isValidPhoneNumber && isNaN(Number(phoneNumber)))
      return { invalidPhoneNumber: true };
    return null;
  }

  validateBirthDate(control: any) {
    const birthDate = new Date(control.value);
    const now = new Date();
    if (isNaN(birthDate.getTime())) return { invalidDate: true };
    const age = calculateAge(birthDate, now);
    if (age < 20) return { minAge: true };
    return null;
  }

  contractDateValidator(control: any) {
    const startDate = new Date('2008-01-01');
    const currentDate = new Date();
    const contractDate = new Date(control.value);
    if (currentDate < startDate || contractDate > currentDate)
      return { InvalidContractDate: true };
    return null;
  }

  BasedSalaryValidation(control: any) {
    const value = control.value;
    if (value === 0) return { nonZeroViolation: true };
    return null;
  }


  get ssn() { return this.validationEmployee.get('ssn'); }
  get fullName() { return this.validationEmployee.get('fullName'); }
  get address() { return this.validationEmployee.get('address'); }
  get phoneNumber() { return this.validationEmployee.get('phoneNumber'); }
  get gender() { return this.validationEmployee.get('gender'); }
  get nationality() { return this.validationEmployee.get('nationality'); }
  get birthDate() { return this.validationEmployee.get('birthDate'); }
  get contractDate() { return this.validationEmployee.get('contractDate'); }
  get baseSalary() { return this.validationEmployee.get('baseSalary'); }
  get arrival() { return this.validationEmployee.get('arrival'); }
  get departure() { return this.validationEmployee.get('departure'); }
  get departmentName() { return this.validationEmployee.get('departmentName'); }



  ngOnInit(): void {
    this.departmentService.getDepartments().subscribe((data) => {
      this.departments = data as IDepartment[];
    });

    this.allCountries = this.employeeService.allCountriesList;

    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data;
    });
  }

  onCancel() {
    this.router.navigate(['Employee/display']);
  }



  onSubmit() {
    console.log(this.validationEmployee.value);

    this.isSubmitted = true;
    if (!this.validationEmployee.valid) return;

    const formValue = this.validationEmployee.value;


    const payload = {
      ssn: String(formValue.ssn),
      fullName: formValue.fullName,
      address: formValue.address,
      phoneNumber: formValue.phoneNumber,
      gender: Number(formValue.gender),
      nationality: formValue.nationality,
      birthDate: formValue.birthDate,
      contractDate: formValue.contractDate,
      baseSalary: Number(formValue.baseSalary),
      arrival: formValue.arrival + ":00",
      departure: formValue.departure + ":00",
      departmentName: formValue.departmentName
    };

    console.log("Payload that will be sent:", payload);

    this.employeeService.addEmployee(payload).subscribe((data) => {
      this.toastr.success('Employee has been added successfully');
      this.router.navigate(['/Employee/display']);
      this.reset();
    });
  }

  // ---------------- MODAL ----------------

  openModal(template: TemplateRef<void>) {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.modalRef?.hide();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  reset() {
    this.validationEmployee.reset();
    this.employeeDTO = {};
    this.selectedDepartment = '';
  }
}
