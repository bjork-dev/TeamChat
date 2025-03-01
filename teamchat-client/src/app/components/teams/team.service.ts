import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Team} from '../../domain/teams/team';
import {AuthService} from '../auth/auth.service';
import {Observable, of} from 'rxjs';

@Injectable()
export class TeamService {

  http = inject(HttpClient);
  userId = inject(AuthService).user()?.id;

  getTeams(): Observable<Team[]> {
    if (!this.userId) return of(([]));
    return this.http.get<Team[]>(`/api/teams/user/${this.userId}`);
  }
}
