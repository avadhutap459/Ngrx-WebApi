import { inject } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { UserService } from "../services/user.service";
import { authActions } from "./auth.action";
import { catchError, map, of, switchMap, tap } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";


export const registerEffect = createEffect(
    (action$ = inject(Actions),userSvc = inject(UserService)) =>{
        return action$.pipe(
            ofType(authActions.login),
            switchMap(({emailId,password}) => {
                debugger
                let loginDa = { EmailId: emailId,Password: password};
                return userSvc.login(loginDa).pipe(
                    map((res: any) => {
                        debugger
                         userSvc.saveTokens(res.Token,res.RefreshToken);
                         userSvc.startRefreshTimer();
                         return authActions.loginSuccess({token : res.Token,refreshToken : res.RefreshToke})
                    }),
                    catchError((errorResponse : HttpErrorResponse) => {
                        return of(authActions.loginFailure({error : errorResponse.error.errors}))
                    })
                )
            })
        )
    },
    {functional : true}
)

export const redirectAfterLoginEffect = createEffect(
    (action$ = inject(Actions), router = inject(Router)) => {
        return action$.pipe(
            ofType(authActions.loginSuccess),
            tap(() => {
                debugger
                router.navigateByUrl('/dashboard/home')
            })
        )
    },
    {functional : true, dispatch : false}
)