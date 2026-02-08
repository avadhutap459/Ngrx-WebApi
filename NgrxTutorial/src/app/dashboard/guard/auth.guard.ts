import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { UserService } from "../../sign-in/services/user.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: UserService,
              private router: Router) {}

  canActivate(): boolean {
    debugger
    
    if (!this.authService.isLoggedIn()) {

      this.router.navigate(['/signIn']);
      return false;
    }

    return true;
  }
}