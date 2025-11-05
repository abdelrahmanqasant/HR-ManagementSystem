import { Injectable, importProvidersFrom } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { ApplicationConfig } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CustomToastrService {

  constructor(private toastr: ToastrService) {}

  showSuccess(message: string, title?: string) {
    this.toastr.success(message, title);
  }

  showError(message: string, title?: string) {
    this.toastr.error(message, title);
  }

  showInfo(message: string, title?: string) {
    this.toastr.info(message, title);
  }

  showWarning(message: string, title?: string) {
    this.toastr.warning(message, title);
  }
}


export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    importProvidersFrom(
      ToastrModule.forRoot({
        positionClass: 'toast-bottom-right',
        timeOut: 3000,
        preventDuplicates: true,
      })
    )
  ]
};
