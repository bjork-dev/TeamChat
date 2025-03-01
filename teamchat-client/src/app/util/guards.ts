import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {AuthService} from '../components/auth/auth.service';

export const authGuard: CanActivateFn = async () => {
  const user = inject(AuthService).user();
  const router = inject(Router);

  if (!user) {
    await router.navigate(['/login']);
    return false;
  }

  return true;
}
