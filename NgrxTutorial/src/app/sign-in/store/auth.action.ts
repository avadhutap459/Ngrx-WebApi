import { createAction, createActionGroup, emptyProps, props } from "@ngrx/store";


export const authActions = createActionGroup({
    source : 'auth',
    events:{
        login : props<{emailId: string; password: string}>(),
        'login success' : props<{token: string; refreshToken: string}>(),
        'login failure' : props<{error: string}>(),
        'logout' : emptyProps()
    }
})
