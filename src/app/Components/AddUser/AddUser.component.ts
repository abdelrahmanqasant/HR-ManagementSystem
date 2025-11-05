import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { IRole } from '../../interfaces/irole';
import { IRegister } from '../../interfaces/iregister';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../services/user.service';
import { RoleService } from '../../services/role.service';
import { CommonModule } from '@angular/common';
import { passwordMatchValidator } from '../../customValidators/confirmPassword';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './AddUser.component.html',
  styleUrls: ['./AddUser-component.css'],
})
export class AddUserComponent implements OnInit {
  validationUser!: FormGroup;
  roles: IRole[] = [];

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private userService: UserService,
    private roleService: RoleService,
    private router: Router
  ) {}

  get fullName() {
    return this.validationUser.get('fullName');
  }
  get email() {
    return this.validationUser.get('email');
  }
  get password() {
    return this.validationUser.get('password');
  }
  get confirmPassword() {
    return this.validationUser.get('confirmPassword');
  }
  get role() {
    return this.validationUser.get('role');
  }

  ngOnInit(): void {
    this.getAllRoles();

    this.validationUser = this.fb.group(
      {
        fullName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', Validators.required],
        role: ['', Validators.required],
      },
      { validators: passwordMatchValidator() }
    );
  }

  success() {
    if (this.validationUser.invalid) return;

    const newUser: IRegister = {
      fullName: this.fullName?.value,
      email: this.email?.value,
      password: this.password?.value,
      confirmPassword: this.confirmPassword?.value,
      roleName: this.role?.value,
    };

    this.userService.addUser(newUser).subscribe(() => {
      this.toastr.success('User added successfully');
      this.router.navigate(['/users']);
    });
  }

  getAllRoles() {
    this.roleService.getAllRoles().subscribe((data) => {
      this.roles = data;
    });
  }

  onCancel() {
    this.router.navigate(['/users']);
  }
}
