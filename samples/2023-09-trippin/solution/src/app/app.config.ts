import { ApplicationConfig, InjectionToken, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { routes } from './app.routes';
import { HttpClientModule } from '@angular/common/http';

export const TRIPPIN_BASE_URL = new InjectionToken<string>('TrippinBaseUrl');

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    { provide: TRIPPIN_BASE_URL, useValue: 'https://services.odata.org/TripPinRESTierService/(S(m5bfpztyapemay4raovtk1wi))' },
    importProvidersFrom(HttpClientModule),
  ],
};
