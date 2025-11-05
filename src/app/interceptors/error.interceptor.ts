import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);

  return next(req).pipe(
    catchError((error) => {
      if (error) {
        switch (error.status) {
          case 400:
            if (error.error?.errors) {
              const modelStateErrors: string[] = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) {
                  modelStateErrors.push(error.error.errors[key]);
                }
              }
              return throwError(() => modelStateErrors.flat());
            } else {
              toastr.error(error.error, 'Bad Request');
            }
            break;

          case 401:
            toastr.error('Unauthorized', error.status);
            break;

          case 403:
            router.navigateByUrl('/home');
            toastr.error('Forbidden', error.status);
            break;

          case 404:
            router.navigateByUrl('/not-found');
            break;

          case 500:
            toastr.error('Internal Server Error');
            break;

          default:
            toastr.error('Something unexpected went wrong');
            console.error(error);
            break;
        }
      }
      return throwError(() => error);
    })
  );
};
