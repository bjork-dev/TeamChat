import {HttpClient} from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import {User} from '../../domain/users/user';

@Injectable()
export class UserService {
  http = inject(HttpClient);

  getUsers() {
    return this.http.get<User[]>('/api/users');
  }

  addUser(user: User) {
    return this.http.post<User>('/api/users', user);
  }

  deleteUser(id: number) {
    return this.http.delete(`/api/users/${id}`);
  }

  updateUser(user: User) {
    return this.http.put<User>(`/api/users/${user.id}`, user);
  }
}
