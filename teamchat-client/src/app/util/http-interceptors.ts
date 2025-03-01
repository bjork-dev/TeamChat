import {AuthService} from '../components/auth/auth.service';
import {inject} from '@angular/core';
import {HttpHandlerFn, HttpRequest} from '@angular/common/http';

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  const authToken = inject(AuthService).getAuthToken();
  if(!authToken) {
    return next(req);
  }
  const newReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${authToken}`)
  });
  return next(newReq);
}
