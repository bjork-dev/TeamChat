import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Team} from '../../domain/teams/team';
import {AuthService} from '../auth/auth.service';
import {Observable, of} from 'rxjs';
import {Group} from '../../domain/teams/group';

@Injectable()
export class TeamService {

  http = inject(HttpClient);
  userId = inject(AuthService).user()?.id;

  getTeams(): Observable<Team[]> {
    if (!this.userId) return of(([]));
    return this.http.get<Team[]>(`/api/teams/user`);
  }

  getGroupDetails(groupId: number) {
    return this.http.get<Group>(`/api/teams/group/${groupId}`);
  }

  sendMessage(groupId: number, content: string) {
    return this.http.post(`/api/teams/group/${groupId}/message`, {content});
  }
}
