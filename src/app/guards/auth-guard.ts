import { Injectable } from '@angular/core';
import { Router, UrlTree } from '@angular/router';
import { UserService } from './../services/user/user.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

  constructor(private  UserService: UserService, private router: Router) { }

  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this.UserService.isloggedIn()) {
      this.router.navigate(['/home']);
      return false;
    }
    return this.UserService.isloggedIn();
    // return true;
  }
}
