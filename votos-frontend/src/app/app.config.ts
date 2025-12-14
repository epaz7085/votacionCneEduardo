import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http'; // ¡Necesario!

import { routes } from './app.routes';
import { authInterceptor } from './votes/interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    // 👈 Esta línea es esencial para que HttpClient funcione
    provideHttpClient(
      withInterceptors([authInterceptor])
    )
  ]
};

