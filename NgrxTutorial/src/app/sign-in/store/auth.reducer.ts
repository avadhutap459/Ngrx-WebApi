import { createFeature, createReducer, on } from "@ngrx/store";
import { authActions } from "./auth.action";


export interface AuthState {
    token: string | null;
    refreshToken: string | null;
    error: string | null;
    loading: boolean;
}

export const initialState: AuthState = {
    token: null,
    refreshToken: null,
    error: null,
    loading: false
};

export const authFeature = createFeature({
    name: 'auth',
    reducer: createReducer(
        initialState,
        //login start
        on(authActions.login, (state) => ({ ...state, loading: true, error: null })),
        //login Success
        on(authActions.loginSuccess, (state, action) => ({ ...state, token: action.token, refreshToken: action.refreshToken, loading: false })),
        //login failure
        on(authActions.loginFailure, (state, action) => ({ ...state, error: action.error, loading: false })),
        // Logout
        on(authActions.logout, () => initialState)
    )
})

export const  { 
    name : authFeatureKey ,
    reducer : authReducer,
    selectToken,selectRefreshToken,selectError,selectLoading
} = authFeature