import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AttendenceService } from '../../services/attendence.service';
import { IAttendence } from '../../interfaces/iattendence';
import { UserParams } from '../../models/userParams';
import { Router } from '@angular/router';

@Component({
  selector: 'app-display-attendences',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './display-attendences.component.html',
  styleUrls: ['./display-attendences.component.css'],
})
export class DisplayAttendencesComponent implements OnInit {
  attendences: IAttendence[] = [];
  userParams: UserParams = new UserParams();
  totalItems: number = 0;

  constructor(
    private attendenceService: AttendenceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getAllAttendences();
  }

  AddAttendence() {
    this.router.navigate(['attendance/add']);
  }

  getAllAttendences() {
    this.attendenceService.getAll(this.userParams).subscribe({
      next: (response) => {
        this.attendences = response.result || [];
        this.totalItems =
          response.Pagination?.totalItems ?? this.attendences.length;
      },
      error: (err) => console.error(err),
    });
  }

  deleteAttendence(empId: number, day: string) {
    this.attendenceService.deleteRecord(empId, day).subscribe({
      next: () => {
        this.attendences = this.attendences.filter(
          (a) => !(a.empId === empId && a.day === day)
        );
      },
      error: (err) => console.error(err),
    });
  }

  pageChanged(newPage: number) {
    this.userParams.pageNumber = newPage;
    this.getAllAttendences();
  }
}
