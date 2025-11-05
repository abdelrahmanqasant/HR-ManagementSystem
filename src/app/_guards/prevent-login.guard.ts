import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../services/account.service';
import { map } from 'rxjs/operators';

export const preventloginGuard: CanActivateFn = (route, state) => {
 const accountService = inject(AccountService);
  const toastrService = inject(ToastrService);
  const router = inject(Router);
  return accountService.currentUser$.pipe(
    map(user => {
      if(!user) return true;
        else{
       toastrService.error('You are already logged in');
         router.navigate(['/home']);
      return false;
      }
    })
  );
};

