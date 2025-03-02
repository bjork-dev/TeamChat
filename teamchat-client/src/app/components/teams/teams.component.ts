import {Component, inject} from '@angular/core';
import {TeamService} from './team.service';
import {AsyncPipe} from '@angular/common';
import {
  MatAccordion,
  MatExpansionPanel, MatExpansionPanelDescription,
  MatExpansionPanelHeader,
  MatExpansionPanelTitle
} from '@angular/material/expansion';
import {MatDivider} from '@angular/material/divider';
import {MatAnchor} from '@angular/material/button';
import {RouterLink} from '@angular/router';

@Component(
  {
    selector: 'app-team',
    providers: [TeamService],
    imports: [
      AsyncPipe,
      MatExpansionPanelHeader,
      MatAccordion,
      MatExpansionPanel,
      MatExpansionPanelTitle,
      MatExpansionPanelDescription,
      MatDivider,
      MatAnchor,
      RouterLink
    ],
    template: `
      <div class="px-2">
        <h5>Teams</h5>
        @for (team of teams | async; track team!.id) {
          <mat-accordion>
            <mat-expansion-panel class="mat-elevation-z0">
              <mat-expansion-panel-header>
                <mat-panel-title> {{ team.name }}</mat-panel-title>
              </mat-expansion-panel-header>
              <small>{{ team.description }}</small>
              <mat-divider/>
              @for (group of team.groups; track group.id) {
                <div class="my-2">
                  <a class="w-100" mat-flat-button [routerLink]="['/group', group.id]">{{ group.name }}</a>
                </div>
              } @empty {
                <p>No groups</p>
              }
            </mat-expansion-panel>
          </mat-accordion>
        }
      </div>
    `
  }
)
export class TeamsComponent {
  teamService = inject(TeamService);
  teams = this.teamService.getTeams();
}
