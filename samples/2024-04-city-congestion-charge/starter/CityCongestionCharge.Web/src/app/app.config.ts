import { ApplicationConfig, InjectionToken } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

export const BASE_URL = new InjectionToken<string>('Base URL for API');

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    { provide: BASE_URL, useValue: 'https://localhost:7182' }
  ]
};
