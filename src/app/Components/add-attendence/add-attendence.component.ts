import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AttendenceService } from '../../services/attendence.service';
import { IAttendence } from '../../interfaces/iattendence';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-attendence',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-attendence.component.html',
  styleUrls: ['./add-attendence.component.css'],
})
export class AddAttendenceComponent implements OnInit {
  attendenceForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private attendenceService: AttendenceService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.attendenceForm = this.fb.group({
      empId: [0, Validators.required],
      day: [this.formatDate(new Date()), Validators.required],
      arrival: ['', Validators.required],
      departure: ['', Validators.required],
      status: [0, Validators.required],
      deptName: ['', Validators.required],
      empName: ['', Validators.required],
    });
  }

  formatDate(date: string | Date): string {
    const d = new Date(date);
    const month = ('0' + (d.getMonth() + 1)).slice(-2);
    const day = ('0' + d.getDate()).slice(-2);
    const year = d.getFullYear();
    return `${year}-${month}-${day}`;
  }

  ngOnInit(): void {}

  addAttendence() {
    if (this.attendenceForm.invalid) {
      this.toastr.error('Please fill all required fields');
      return;
    }

    const formValue = this.attendenceForm.value;

    const payload: IAttendence = {
      empId: Number(formValue.empId),
      day: this.formatDate(formValue.day),
      arrival: formValue.arrival ? formValue.arrival + ':00' : null,
      departure: formValue.departure ? formValue.departure + ':00' : null,
      status: Number(formValue.status),
      deptName: formValue.deptName,
      empName: formValue.empName,
    };

    console.log('Payload Sent To API:', payload);

    console.log(' Payload Sent to API:', payload);

    this.attendenceService.addAttendence(payload).subscribe({
      next: () => {
        this.toastr.success('Attendance added successfully!');

        this.attendenceForm.reset({
          empId: 0,
          day: this.formatDate(new Date()),
          arrival: '',
          departure: '',
          status: 0,
          deptName: '',
          empName: '',
        });

        this.attendenceService.refreshData();
        this.router.navigate(['/display-attendences']);
      },
      error: (err) => {
        console.error(' API Error:', err);
        this.toastr.error('Failed to add attendance');
      },
    });
  }
}
