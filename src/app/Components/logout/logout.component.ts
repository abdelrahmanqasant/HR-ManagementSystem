import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  standalone: true,
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {
   constructor(

      private _toastr: ToastrService,
      private _accountService: AccountService,
    private _router :Router
    ) {}
logout() {
  this._accountService.logout().subscribe({
    next: (res) => {
      this._toastr.success('logged out successfully  ');
      this._router.navigate(['/login']);
    },
    error: () => {
      this._accountService.forceLogout();
      this._toastr.success('logged out successfully' );
      this._router.navigate(['/login']);
    }
  });
}
}
