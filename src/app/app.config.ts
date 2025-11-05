import { bootstrapApplication } from '@angular/platform-browser';

import { importProvidersFrom } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppComponent } from './app.component';
import { appConfig } from './services/toastr.service';

bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...appConfig.providers,
    importProvidersFrom(
      BrowserAnimationsModule,
      ToastrModule.forRoot() 
    )
  ]
}).catch((err) => console.error(err));
