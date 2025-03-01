import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {Notyf} from 'notyf';
import {JwtHelperService, JwtModule} from '@auth0/angular-jwt';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    importProvidersFrom(
      JwtModule.forRoot({
        config: {
          tokenGetter: () => {
            return localStorage.getItem('token');
          }
        }
      })
    ),
    provideHttpClient(
      withInterceptorsFromDi()
    ),
    {
      provide: Notyf, useValue: new Notyf({
        duration: 4000,
      })
    },

    {provide: JwtHelperService, useClass: JwtHelperService},
  ]
};
