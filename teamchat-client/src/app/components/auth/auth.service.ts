import {inject, Injectable, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {JwtHelperService} from '@auth0/angular-jwt';
import {User} from '../../domain/users/user';
import {Observable, of, switchMap, tap} from 'rxjs';
import {LoginToken} from '../../domain/auth/login-token';

@Injectable()
export class AuthService {

  http = inject(HttpClient);
  jwtHelper = inject(JwtHelperService);

  _user = signal<User | undefined>(undefined);
  user = this._user.asReadonly();

  constructor() {
    const tokenJson = localStorage.getItem('token');
    if (!tokenJson) return;
    const loginToken = JSON.parse(tokenJson) as LoginToken;
    if (!loginToken) return
    const user = this.decodeToken(loginToken.token)
    console.log(user);
    if (!user) return;
    this._user.set(user);
  }

  decodeToken(token: string): User | null {
    const decodedToken = this.jwtHelper.decodeToken(token);

    const id = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] as number
    const username = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] as string
    const email = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] as string
    const firstName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'] as string
    const lastName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'] as string
    const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] as string

    return {
      id: id,
      username: username,
      email: email,
      firstName: firstName,
      lastName: lastName,
      role: role,
      token: token
    }

  }

  login(username: string, password: string): Observable<boolean> {
    return this.http.post<LoginToken>('/api/login', {username, password})
      .pipe(
        switchMap((loginToken: LoginToken) => {
          localStorage.setItem('token', JSON.stringify(loginToken));
          const user = this.decodeToken(loginToken.token);
          console.log(user);
          if (!user) {
            throw new Error('Invalid token');
          }
          this._user.set(user);
          return of(true)
        })
      );
  }

}
