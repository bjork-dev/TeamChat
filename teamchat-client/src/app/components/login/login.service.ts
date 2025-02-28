import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class LoginService {
  http = inject(HttpClient);

  login(username: string, password: string) {
    return this.http.post('/api/login', { username, password });
  }

}
