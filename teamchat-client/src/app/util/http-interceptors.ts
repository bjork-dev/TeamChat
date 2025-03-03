import {AuthService} from '../components/auth/auth.service';
import {inject} from '@angular/core';
import {HttpHandlerFn, HttpRequest} from '@angular/common/http';
import {catchError} from 'rxjs';
import {Notyf} from 'notyf';

export function errorInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  const notyf = inject(Notyf);
  const authService = inject(AuthService);
  return next(req).pipe(
    catchError(error => {
      if (error.status === 401) {
        notyf.error('Session expired, please login again');
        authService.logout();
      }
      throw error;
    })
  )
}

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  const authToken = inject(AuthService).getAuthToken();
  if (!authToken) {
    return next(req);
  }
  const newReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${authToken}`)
  });
  return next(newReq);
}
