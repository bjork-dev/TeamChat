import {Component, inject} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {TeamService} from './team.service';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-group',
  providers: [TeamService],
  styles: [],
  imports: [
    AsyncPipe
  ],
  template: `
    @if (group | async; as group) {
      <h1>{{ group.name }}</h1>
    }
  `
})
export class GroupComponent {
  route = inject(ActivatedRoute);
  teamService = inject(TeamService);

  constructor() {
    console.log('GroupComponent constructor');
  }

  group = this.teamService.getGroupDetails(this.route.snapshot.params['id']);
}
