import {Component, inject} from '@angular/core';
import {TeamService} from './team.service';
import {AsyncPipe} from '@angular/common';

@Component(
  {
    selector: 'app-team',
    providers: [TeamService],
    imports: [
      AsyncPipe
    ],
    template: `
      <div>
        <h5>Teams</h5>
        <p>Here are the teams</p>
        @for (team of teams | async; track team!.id) {
          {{ team.name }}
        }
      </div>
    `
  }
)
export class TeamsComponent {
  teamService = inject(TeamService);
  teams = this.teamService.getTeams();
}
