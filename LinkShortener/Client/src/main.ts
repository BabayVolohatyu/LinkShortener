import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { provideAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

bootstrapApplication(App, {
  providers: [
    provideAnimations(),                 
    importProvidersFrom(HttpClientModule, RouterModule)
  ]
});
