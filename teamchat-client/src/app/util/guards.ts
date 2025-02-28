import {CanActivateFn} from '@angular/router';

export const authGuard: CanActivateFn = (next, state) => {
  console.info('authGuard');
  return true;
}
