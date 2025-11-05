import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,FormsModule],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css'
})
export class SignInComponent implements OnInit{
model:any = {};
isSubmitted =false;
signInForm! :FormGroup;
formError =false;
formFill =false;
constructor(private fb :FormBuilder,private _accountService :AccountService , private _router :Router , private _toastr :ToastrService){}
  ngOnInit(): void {
      this.initializeForm();
  }

  initializeForm() :void {
this.signInForm = this.fb.group ( {
    email : ['',[Validators.required , Validators.email]],
    password : ['',[Validators.required,Validators.minLength(4)]]
  }
)
  }
get email (){
  return this.signInForm.get("email")
}
get password (){
  return this.signInForm.get("password");
}


login(){
this.isSubmitted = true;
let email = this.signInForm.value['email']
let password = this.signInForm.value['password']
if(!this.email?.valid || !this.password?.valid)
return;

  this._accountService.login(email,password).subscribe({
    next :((res)=>{
      console.log(res)
this._toastr.success('logged in successfully');
this._router.navigate(['/home'])
    }),
    error :()=>{
this.formError = true;
    }

  })

}

}


