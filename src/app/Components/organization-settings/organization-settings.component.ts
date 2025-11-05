import { Router } from '@angular/router';
import { IOrganizationSettings } from './../../interfaces/iorganization-settings';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { FilterPipe } from '../../pipes/filter.pipe';

import { SettingsService } from '../../services/settings.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-organization-settings',
  standalone: true,
  imports: [
    NgxPaginationModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],

  templateUrl: './organization-settings.component.html',
  styleUrls: ['./organization-settings.component.css'],
})
export class OrganizationSettingsComponent implements OnInit {
  validationSetting: FormGroup;
  isCommissionHour = true;
  isDeductionHour = true;
  public settings: IOrganizationSettings = {
    commissionDTO: {
      type: 0,
      hours: undefined,
      amount: undefined,
    },
    deductionDTO: {
      type: 0,
      hours: undefined,
      amount: undefined,
    },
    weeklyDaysDTO: {
      days: [],
    },
  };
  public daysOfWeek: { name: string; value: number; state: boolean }[] = [
    { name: 'Saturday', value: 6, state: false },
    { name: 'Sunday', value: 0, state: false },
    { name: 'Monday', value: 1, state: false },
    { name: 'Tuesday', value: 2, state: false },
    { name: 'Wednesday', value: 3, state: false },
    { name: 'Thursday', value: 4, state: false },
    { name: 'Friday', value: 5, state: false },
  ];
  constructor(
    private organization: SettingsService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.validationSetting = this.fb.group({
      commision: ['', [Validators.required]],
      deduction: ['', Validators.required],
    });
  }
  nonNegativeValidator(
    control: AbstractControl
  ): { [key: string]: any } | null {
    const value = control.value;
    if (value < 0) return { negative: true };
    return null;
  }
  getCommission() {
    return this.validationSetting.get('commission');
  }
  getDeduction() {
    return this.validationSetting.get('deduction');
  }
  ngOnInit(): void {
    this.getSettings();
  }

  getSettings() {
    this.organization.getOrganization().subscribe(
      (data: IOrganizationSettings) => {
        if (data) {
          this.settings = data;

          if (this.settings.commissionDTO.type == 1) {
            this.isCommissionHour = false;
          }

          if (this.settings.deductionDTO.type == 1) {
            this.isDeductionHour = false;
          }

          this.setSelectedDays(data.weeklyDaysDTO.days);
        }
      },
      (error: any) => {
        console.error('Error Fetching Organization Settings', error);
      }
    );
  }

  onSubmit(): void {
    this.organization
      .updateOrganization(this.settings as IOrganizationSettings)
      .subscribe(
        (response) => {
          this.toastr.success('Successfully Updated');
          this.getSettings();
        },
        (error) => {
          console.error('error', error);
        }
      );
  }
  onCommissionTypeChange() {
    if (this.settings.commissionDTO.type == 0) {
      this.isCommissionHour = true;
      this.settings.commissionDTO.hours = undefined;
    } else {
      this.isCommissionHour = false;
      this.settings.commissionDTO.amount = undefined;
    }
  }
  onDeductionTypeChange() {
    if (this.settings.deductionDTO.type == 0) {
      this.isDeductionHour = true;
      this.settings.deductionDTO.hours = undefined;
    } else {
      this.isDeductionHour = false;
      this.settings.deductionDTO.amount = undefined;
    }
  }
  onCheckBoxChange(event: any, dayValue: number) {
    const checkBox = event.target;
    if (this.settings.weeklyDaysDTO && this.settings.weeklyDaysDTO.days) {
      if (checkBox.checked) {
        if (!Array.isArray(this.settings.weeklyDaysDTO.days)) {
          this.settings.weeklyDaysDTO.days = [];
        }
        if (this.settings.weeklyDaysDTO.days.length < 2) {
          this.settings.weeklyDaysDTO.days.push(dayValue);
        } else {
          checkBox.checked = false;
          this.toastr.error('You cannot select more than 2 days off');
        }
      } else {
        const index = this.settings.weeklyDaysDTO.days.indexOf(dayValue);
        if (index !== -1) this.settings.weeklyDaysDTO.days.splice(index, 1);
      }
    } else {
      this.settings.weeklyDaysDTO = { days: [] };
      this.settings.weeklyDaysDTO.days.push(dayValue);
    }
  }
  trackByIdx(index: number, obj: any): any {
    return index;
  }
  public getActiveDays(): number[] {
    return this.daysOfWeek.reduce((activeDays: number[], day) => {
      if (day.state) {
        activeDays.push(day.value);
      }
      return activeDays;
    }, []);
  }
  public updateStateBasedOnNumbers(numbers: number[]): void {
    this.daysOfWeek.forEach((day) => {
      if (numbers.includes(day.value)) {
        day.state = true;
      } else {
        day.state = false;
      }
    });
  }
  onCancel() {
    this.router.navigate(['/home']);
  }

  setSelectedDays(weeklyOffDays: number[]) {
    this.daysOfWeek.forEach((day) => {
      day.state = weeklyOffDays.includes(day.value);
    });
  }
}
