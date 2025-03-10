import {Component, inject, signal} from '@angular/core';
import {MatCard, MatCardActions, MatCardContent, MatCardHeader, MatCardTitle} from '@angular/material/card';
import {MatFormField} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {AuthService} from './auth.service';
import {catchError, firstValueFrom, tap} from 'rxjs';
import {Notyf} from 'notyf';
import {HttpErrorResponse} from '@angular/common/http';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import {Router} from '@angular/router';
import {HeaderComponent} from '../header.component';

@Component({
  imports: [
    MatCard,
    MatFormField,
    MatCardContent,
    MatCardTitle,
    MatInput,
    MatButton,
    MatCardActions,
    MatCardHeader,
    ReactiveFormsModule,
    MatProgressSpinner,
    HeaderComponent
  ],
  styles: [`
    .login {
      height: calc(100vh - 64px);
    }
  `],
  template: `
    <app-header/>
    <div class="container-fluid d-flex align-items-center justify-content-center login">
      <mat-card class="w-100" style="max-width: 400px;">
        <mat-card-header>
          <mat-card-title>TeamChat Login</mat-card-title>
        </mat-card-header>
        <form [formGroup]="loginForm" (ngSubmit)="login()">
          <mat-card-content>
            <div>
              <mat-form-field class="w-100">
                <input matInput placeholder="Username" formControlName="username">
              </mat-form-field>
            </div>
            <div>
              <mat-form-field class="w-100">
                <input matInput placeholder="Password" formControlName="password">
              </mat-form-field>
            </div>
          </mat-card-content>
          <mat-card-actions>
            <button type="submit" class="w-100" mat-flat-button color="primary"
                    [disabled]="loginForm.invalid || loading()">
              @if (loading()) {
                <mat-spinner diameter="20"></mat-spinner>
              } @else {
                Login
              }
            </button>
          </mat-card-actions>
        </form>
      </mat-card>
    </div>
  `,
})
export class LoginComponent {

  loginService = inject(AuthService);
  notyf = inject(Notyf);
  formBuilder = inject(FormBuilder);
  router = inject(Router);
  loading = signal(false);

  loginForm = this.formBuilder.group({
    username: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]],
    password: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]]
  });

  async login() {
    this.loading.set(true);
    const {username, password} = this.loginForm.value;
    await firstValueFrom(this.loginService.login(username!, password!).pipe(
        tap(async () =>
          await this.router.navigateByUrl('/')
        ),
        catchError((error: HttpErrorResponse) => {
            this.notyf.error(error.error);
            this.loading.set(false);
            throw error;
          }
        )
      )
    );
  }
}
