import { DaysOffService } from './../../services/days-off.service';
import { CommonModule } from '@angular/common';
import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { catchError, throwError } from 'rxjs';
import { FilterPipe } from '../../pipes/filter.pipe';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-official-days',
  standalone: true,
  imports: [
    NgxPaginationModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FilterPipe,
  ],
  templateUrl: './official-days.component.html',
  styleUrls: ['./official-days.component.css'],
})
export class OfficialDaysComponent implements OnInit {
  modalRef?: BsModalRef;
  formDataAdd: any = { name: '', date: '' };
  formDataUpdate: any = { name: '', date: '' };
  daysOffList: any[] = [];
  validationHolidays: FormGroup;
  deleDay: Date = new Date('2000-01-01');
  selectedRowIndex: number = 1;
  isUpdateFormVisible: boolean = false;
  totalLength: any;
  page: number = 1;
  searchText: any;

  constructor(
    private daysOffService: DaysOffService,
    private fb: FormBuilder,
    private modalService: BsModalService
  ) {
    this.validationHolidays = this.fb.group({
      name: ['', Validators.required],
      date: ['', Validators.required],
    });
  }

  get name() {
    return this.validationHolidays.get('name');
  }
  get date() {
    return this.validationHolidays.get('date');
  }

  ngOnInit(): void {
    this.closeForm();
    this.refreshDaysOffList();
  }

  onAddSubmit() {
    if (this.validationHolidays.invalid) return;

    this.daysOffService.addDayOff(this.formDataAdd).subscribe({
      next: () => {
        this.refreshDaysOffList();
        this.resetAddForm();
      },
      error: (error) => console.error('Error Adding DayOff:', error),
    });
  }

  onUpdateSubmit() {
    if (!this.formDataUpdate.name) return;

    this.daysOffService.updateDayOff(this.formDataUpdate).subscribe({
      next: () => {
        this.refreshDaysOffList();
        this.closeForm();
      },
      error: (error) => console.error('Error Updating DayOff:', error),
    });
    this.selectedRowIndex = -1;
    this.isUpdateFormVisible = false;
  }

  resetAddForm() {
    this.formDataAdd = { name: '', date: '' };
    this.validationHolidays.reset();
    this.validationHolidays.markAsPristine();
    this.validationHolidays.markAsUntouched();
  }

  deleteDayOff(dayoff: any) {
    this.daysOffService.deleteDayOff(dayoff).subscribe({
      next: () => this.refreshDaysOffList(),
      error: (error) => console.error('Error Deleting DayOff:', error),
    });
    this.decline();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  refreshDaysOffList() {
    this.daysOffService
      .getAllDaysOff()
      .pipe(
        catchError((error) => {
          console.error('Fetching Data Error:', error);
          return throwError(() => new Error('Something went wrong'));
        })
      )
      .subscribe((data) => (this.daysOffList = data ?? []));
  }

  showUpdateForm(item: any) {
    this.formDataUpdate = { ...item };
    const popupForm = document.getElementById('popupForm') as HTMLElement;
    popupForm.style.display = 'block';
  }

  closeForm() {
    const popupForm = document.getElementById('popupForm') as HTMLElement;
    popupForm.style.display = 'none';
  }

  openModal(template: TemplateRef<void>, date: Date) {
    this.deleDay = date;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
}
