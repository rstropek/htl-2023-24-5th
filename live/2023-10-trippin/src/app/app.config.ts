import { ApplicationConfig, InjectionToken, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { HttpClientModule, provideHttpClient } from '@angular/common/http';

import { routes } from './app.routes';

export const TRIPPIN_BASE_URL = new InjectionToken<string>('TRIPPIN_BASE_URL');

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    {
      provide: TRIPPIN_BASE_URL,
      useValue: 'https://services.odata.org/TripPinRESTierService/(S(m5bfpztyapemay4raovtk1wi))',
    },
    //importProvidersFrom(HttpClientModule)
    provideHttpClient(),
  ],
};
