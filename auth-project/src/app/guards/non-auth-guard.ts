import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/authService';
import { inject } from '@angular/core';
import { map } from 'rxjs';

export const nonAuthGuard: CanActivateFn = (route, state) => {
  const authService: AuthService = inject(AuthService);
  const router = inject(Router);

  return authService.isAuthenticated().pipe(
    map((isAuth) => {
      if (!isAuth) {
        return true;
      }
      return router.createUrlTree(['/']);
    }),
  );
};
