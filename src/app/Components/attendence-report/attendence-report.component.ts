import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { Pagination } from '../../interfaces/pagination';
import { IAttendence } from '../../interfaces/iattendence';
import { UserParams } from '../../models/userParams';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AttendenceService } from '../../services/attendence.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-attendence-report',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, PaginationModule],
  templateUrl: './attendence-report.component.html',
  styleUrls: ['./attendence-report.component.css'],
})
export class AttendenceReportComponent implements OnInit, OnDestroy {
  modalRef?: BsModalRef;

  attendenceDel: number = 1;
  attendenceDate: string = '';

  pagination: Pagination | undefined;
  validDate = true;

  attendenceReport: IAttendence[] | undefined = [];

  startDate: string = '2008-01-01';

  userParams: UserParams | undefined;

  firstLoad = false;

  constructor(
    private modalService: BsModalService,
    private attendenceService: AttendenceService,
    private toastr: ToastrService
  ) {}

  ngOnDestroy(): void {
    this.resetFilters();
  }

  ngOnInit(): void {
    this.getAttendenceReport();
  }

  getAttendenceReport() {
    if (this.userParams) {
      this.attendenceService.userParams = this.userParams;

      this.attendenceService.getAll(this.userParams).subscribe({
        next: (report) => {
          if (report) {
            this.attendenceReport = report.result;
            this.pagination = report.Pagination;
            this.firstLoad = true;
          }
        },
        error: (err) => {
          this.attendenceReport = [];
          this.toastr.error(
            err?.error?.message || err?.error || 'something went wrong'
          );
          this.pagination = undefined;
        },
      });
    }
  }

  getFilteredData() {
    if (this.userParams) {
      this.userParams.pageNumber = 1;
      this.getAttendenceReport();
    }
  }

  deleteRecord(id: number, date: string): void {
    this.attendenceService.deleteRecord(id, date).subscribe(() => {
      this.toastr.success('deleted successfully');
      this.getAttendenceReport();
    });
  }

  pageChanged(event: any) {
    if (this.userParams) {
      if (this.userParams.pageNumber !== event.page) {
        this.userParams.pageNumber = event.page;
        this.getAttendenceReport();
      }
    }
  }

  resetFilters() {
    this.attendenceService.resetUserParams();
    this.userParams = new UserParams();
    this.getAttendenceReport();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  openModal(template: TemplateRef<void>, id: number, date: string) {
    this.attendenceDate = date;
    this.attendenceDel = id;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  deleteAttendence(id: number, date: string) {
    this.attendenceDel = id;
    this.attendenceDate = date;
    this.deleteRecord(id, date);
    this.modalRef?.hide();
    this.getAttendenceReport();
  }
}
