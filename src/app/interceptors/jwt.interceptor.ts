import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../services/account.service';
import { switchMap, take } from 'rxjs';


export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
const _accountService = inject(AccountService);
return _accountService.currentUser$.pipe(
  take(1),
  switchMap(user => {
    if(user &&user.token){
      const cloneRequest = req.clone({
setHeaders : {Authorization : `Bearer ${user.token}`}
      })
  return next(cloneRequest)
    };
return next(req)
  })
)
};
