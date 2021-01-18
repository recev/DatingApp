import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AuthorizationService } from '../services/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard implements CanActivate {
  constructor(
    private router: Router,
    private toastr: ToastrService,
    private authorization: AuthorizationService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    const roles = next.firstChild.data.roles as Array<string>;

    if (this.authorization.isLoggedIn()) {
      if (roles)
      {
        if (this.authorization.isUserAuthorized(roles)) {
         return true;
        }
      }
      else
      {
        return true;
      }
    }

    this.toastr.error('You are not authorized!');
    this.router.navigate(['/home']);
  }
}
