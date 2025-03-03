import {Component, inject} from '@angular/core';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef, MatRow, MatRowDef,
  MatTable
} from '@angular/material/table';
import {User} from '../../domain/users/user';
import {UserService} from '../users/user.service';
import {AsyncPipe} from '@angular/common';

@Component(
  {
    selector: 'app-users',
    providers: [UserService],
    styles: [],
    imports: [
      MatTable,
      MatHeaderCell,
      MatCell,
      MatCellDef,
      MatColumnDef,
      MatHeaderCellDef,
      MatHeaderRow,
      MatRow,
      MatRowDef,
      MatHeaderRowDef,
      AsyncPipe
    ],
    template: `
      @if (dataSource | async; as dataSource) {
        <table mat-table [dataSource]="dataSource" class="mat-elevation-z8 demo-table">
          @for (column of columns; track column) {
            <ng-container [matColumnDef]="column.columnDef">
              <th mat-header-cell *matHeaderCellDef>
                {{ column.header }}
              </th>
              <td mat-cell *matCellDef="let row">
                {{ column.cell(row) }}
              </td>
            </ng-container>
          }

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      }
    `
  }
)
export class UsersComponent {

  userService = inject(UserService);

  columns = [
    {
      columnDef: 'username',
      header: 'Username',
      cell: (user: User) => `${user.username}`,
    },
    {
      columnDef: 'firstName',
      header: 'First Name',
      cell: (user: User) => `${user.firstName}`,
    },
    {
      columnDef: 'lastName',
      header: 'Last Name',
      cell: (user: User) => `${user.lastName}`,
    },
    {
      columnDef: 'email',
      header: 'Email',
      cell: (user: User) => `${user.email}`,
    },
    {
      columnDef: 'role',
      header: 'Role',
      cell: (user: User) => `${user.role}`,
    },
  ];

  dataSource = this.userService.getUsers();
  displayedColumns = this.columns.map(c => c.columnDef);
}
