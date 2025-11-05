import { FormsModule } from '@angular/forms';


import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { provideToastr } from 'ngx-toastr';

import { AccountService } from './services/account.service';
import { IUser } from './interfaces/iuser';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SidebarComponent } from './Components/sidebar/sidebar.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,ToastrModule,NgxSpinnerModule ,SidebarComponent,CommonModule],
  templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'AngularUI';
  constructor(public accountService:AccountService){}
  ngOnInit(): void {
this.setCurrentUser();
  }

  setCurrentUser(){
const userString = localStorage.getItem('user')
if(!userString) return;
const user :IUser = JSON.parse(userString);
this.accountService.setCurrentUser(user);
  }
}
