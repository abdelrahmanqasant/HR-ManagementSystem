import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { importProvidersFrom } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { jwtInterceptor } from './app/interceptors/jwt.interceptor';
import { loadingInterceptor } from './app/interceptors/loading.interceptor';
import { errorInterceptor } from './app/interceptors/error.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([jwtInterceptor, loadingInterceptor, errorInterceptor])
    ),
    importProvidersFrom(
      BrowserAnimationsModule,
      ToastrModule.forRoot(),
      ModalModule.forRoot()
    ),
  ],
}).catch((err) => console.error(err));
