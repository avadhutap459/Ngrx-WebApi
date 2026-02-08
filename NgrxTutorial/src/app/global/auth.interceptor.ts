import { HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UserService } from "../sign-in/services/user.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: UserService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {

    debugger
    const token = this.authService.getAccessToken();

    if (token) {

      const clonedReq = req.clone({
        headers: req.headers.set("Authorization", "Bearer " + token)
      });

      return next.handle(clonedReq);
    }

    return next.handle(req);
  }
}