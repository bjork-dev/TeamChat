import {Component, input} from '@angular/core';
import {Message} from '../../domain/teams/message';
import {MatCard, MatCardContent, MatCardHeader} from '@angular/material/card';
import {DatePipe, NgClass} from '@angular/common';

@Component(
  {
    selector: 'app-message',
    imports: [
      MatCard,
      MatCardContent,
      MatCardHeader,
      NgClass,
      DatePipe
    ],
    styles: [
      `
        .message {
          max-width: 50%;
          overflow-wrap: break-word;
        }
      `
    ],
    template: `
      <div class="d-flex flex-wrap" [ngClass]="{'justify-content-end': isUserMessage()}">
        <mat-card appearance="outlined" class="message">
          <mat-card-header>
            <div class="d-flex justify-content-between w-100 gap-4">
              <div>
                <small> {{ message().firstName }} {{ message().lastName }}</small>
              </div>
              <div>
                <small>{{ message().createdAt | date: 'short' }}</small>
              </div>
            </div>
          </mat-card-header>
          <mat-card-content class="text-wrap">
            {{ message().text }}
          </mat-card-content>
        </mat-card>
      </div>
    `
  }
)
export class MessageComponent {
  message = input.required<Message>();
  isUserMessage = input.required<boolean>();
}
