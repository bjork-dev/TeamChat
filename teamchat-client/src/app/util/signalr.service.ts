import {inject, Injectable, OnDestroy, signal} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {HubConnection, HubConnectionState} from '@microsoft/signalr';
import {Notyf} from 'notyf';
import {filter, Subject} from 'rxjs';
import {AuthService} from '../components/auth/auth.service';
import {environment} from '../../../environment';

@Injectable(
  {providedIn: 'root'}
)
export class SignalrService implements OnDestroy {

  notyf = inject(Notyf)

  private _hubConnectionState = new Subject<HubConnectionState>();
  private _connection?: HubConnection;

  public hubConnectionState = this._hubConnectionState
    .pipe(
      filter(state => state === HubConnectionState.Connected),
    )

  constructor(authService: AuthService) {
    const token = authService.getAuthToken();
    if (!token) {
      this.notyf.error('You are not authenticated');
      return;
    }

    this._connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.server + '/hub', {accessTokenFactory: () => token})
      .build();

    this._connection.start()
      .then(() => {
        console.log('Connection started');
        this._hubConnectionState.next(this._connection!.state);
      })
      .catch(err => {
        console.error('Error while starting connection: ' + err)
        this.notyf.error('An error occurred while connecting to the server');
        this._hubConnectionState.next(this._connection!.state);
      });
  }

  async joinGroup(groupId: string) {
    await this._connection?.invoke('JoinGroup', groupId);
  }

  addListener<T>(eventName: string, callback: (data: T) => void) {
    this._connection?.on(eventName, callback);
  }

  ngOnDestroy(): void {
    console.log('Connection stopped');
    this._connection?.stop()
  }
}
