import {Component, inject, output} from '@angular/core';
import {MatFormField, MatInput, MatLabel} from '@angular/material/input';
import {MatIconButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';

@Component({
  selector: 'app-chat',
  imports: [
    MatInput,
    MatFormField,
    MatLabel,
    MatIconButton,
    MatIcon,
    ReactiveFormsModule
  ],
  template: `
    <div class="d-flex gap-2" [formGroup]="messageForm">
      <mat-form-field class="w-100">
        <mat-label>Send a message</mat-label>
        <input matInput formControlName="message">
      </mat-form-field>
      <div>
        <button mat-icon-button (click)="sendMessage()" [disabled]="messageForm.invalid">
          <mat-icon>send</mat-icon>
        </button>
      </div>
    </div>

  `
})
export class ChatComponent {

  messageSent = output<string>();

  messageForm = new FormGroup({
      message: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(255)])
    }
  );

  sendMessage() {
    if (this.messageForm.valid) {
      this.messageSent.emit(this.messageForm.value.message!);
      this.messageForm.reset();
    }
  }
}
