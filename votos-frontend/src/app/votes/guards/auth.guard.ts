import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {

    // VALIDAR DESDE LOCALSTORAGE (NO MEMORIA)
    const token = localStorage.getItem('token');

    if (!token) {
      this.router.navigate(['/login']);
      return false;
    }

    // VALIDACIÓN DE ROL (SI APLICA)
    const requiredRole = route.data['role'];
    if (requiredRole) {
      const user = localStorage.getItem('user');
      if (!user) {
        this.router.navigate(['/login']);
        return false;
      }

      const userRole = JSON.parse(user).role;
      if (userRole !== requiredRole) {
        this.router.navigate(['/votes']);
        return false;
      }
    }

    return true;
  }
}
