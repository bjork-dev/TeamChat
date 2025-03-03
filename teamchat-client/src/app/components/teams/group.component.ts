import {
  AfterViewChecked,
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  inject, OnDestroy,
  signal,
  viewChild
} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {TeamService} from './team.service';
import {AsyncPipe} from '@angular/common';
import {catchError, firstValueFrom, from, map, merge, of, Subject, switchMap, tap, withLatestFrom} from 'rxjs';
import {MessageComponent} from './message.component';
import {AuthService} from '../auth/auth.service';
import {ChatComponent} from './chat.component';
import {SignalrService} from '../../util/signalr.service';
import {MatDivider} from '@angular/material/divider';
import {Notyf} from 'notyf';
import {Message} from '../../domain/teams/message';
import {MatButton} from '@angular/material/button';
import {Title} from '@angular/platform-browser';

@Component({
  selector: 'app-group',
  imports: [
    AsyncPipe,
    MessageComponent,
    ChatComponent,
    MatDivider,
    MatButton
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [TeamService],
  styles: [
    `
      .messages {
        height: calc(100vh - 260px);
        overflow-y: scroll;
      }
    `
  ],
  template: `
    @if (group | async; as group) {
      <h1>{{ group.name }}</h1>

      <div class="messages" #scrollContainer>
        @for (message of group.messages; track message.id) {
          <div class="py-2 px-2">
            <app-message
              [message]="message"
              [isUserMessage]="message.userId == userId"
            />
          </div>
        }
      </div>

      <mat-divider/>

      <div class="mt-2">
        <app-chat
          (messageSent)="sendMessage($event)"
        />
      </div>
      <div>
        <button mat-button (click)="scrollToBottom(true)">Scroll to bottom</button>
      </div>
    }
  `
})
export class GroupComponent implements OnDestroy {
  route = inject(ActivatedRoute);
  teamService = inject(TeamService);
  userId = inject(AuthService).user()?.id;
  signalRService = inject(SignalrService);
  notyf = inject(Notyf);
  titleService = inject(Title);

  messageReceived = new Subject<void>();

  groupId = signal<number>(0);
  firstRender = signal(false);

  scrollContainer = viewChild<ElementRef>('scrollContainer');

  constructor() {
    console.log('GroupComponent created');
    this.signalRService.addListener('messageReceived', () => {
        this.messageReceived.next();
      }
    );
  }

  group =
    this.route.params.pipe(
      map(params => params['id']),
      tap(id => {
        this.groupId.set(id)
        this.firstRender.set(true)
      }),
      switchMap(id => from(this.signalRService.joinGroup(id)).pipe(map(() => id))),
      switchMap(id =>
        merge(of(id), this.messageReceived.pipe(
          tap(() => this.scrollToBottom()),
          map(() => id)))
      ),
      switchMap(id => this.teamService.getGroupDetails(id)),
      tap((group) => {
        this.titleService.setTitle(group.name);
        this.scrollToBottom()
        this.firstRender.set(false)
      })
    );

  scrollToBottom(viaButton = false) {
    // Ensure the container exists
    if (!this.scrollContainer()) return;
    const container = this.scrollContainer()!.nativeElement;
    const isAtBottom = container.scrollHeight - container.clientHeight <= container.scrollTop + 1;
    if (isAtBottom || this.firstRender() || viaButton) {
      setTimeout(() => {
        container.scrollTop = container.scrollHeight
      }, 100); // Wait for the view to update
    }
  }

  async sendMessage(message: string) {
    await firstValueFrom(this.teamService.sendMessage(this.groupId(), message).pipe(
      catchError(err => {
        this.notyf.error('An error occurred while sending the message')
        throw err;
      }),
    ));
  }

  ngOnDestroy() {
    console.log('GroupComponent destroyed');

    this.signalRService.leaveGroup(this.groupId());
  }
}
