import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';
import {routes} from './app.routes';
import {provideHttpClient, withInterceptors,} from '@angular/common/http';
import {Notyf} from 'notyf';
import {JwtHelperService, JwtModule} from '@auth0/angular-jwt';
import {authInterceptor, errorInterceptor} from './util/http-interceptors';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([
      errorInterceptor,
      authInterceptor
    ])),
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
    {
      provide: Notyf, useValue: new Notyf({
        duration: 4000,
      })
    },
    {provide: JwtHelperService, useClass: JwtHelperService},
  ]
};
