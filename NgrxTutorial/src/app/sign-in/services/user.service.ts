import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject, Observable, Subscription, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';

import { LoginResponse } from '../models/loginResponse.interface';

@Injectable({
  providedIn: 'root',
})
export class UserService {

  private apiUrl = "https://localhost:7009/api/Auth";

  // ✅ BehaviorSubject stores token in runtime
  private userTokenSubject = new BehaviorSubject<string | null>(null);

  // Observable for Dashboard, Components
  userToken$ = this.userTokenSubject.asObservable();

  // ✅ Refresh Timer Subscription (to prevent multiple timers)
  private refreshSub?: Subscription;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    // ✅ Only restore token (DO NOT start timer here)
    if (isPlatformBrowser(this.platformId)) {
      const token = localStorage.getItem("accessToken");

      if (token) {
        this.userTokenSubject.next(token);
      }
    }
  }

  // =====================================================
  // ✅ LOGIN API
  // =====================================================
  login(data: any): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, data);
  }

  // =====================================================
  // ✅ Save Tokens (Only After Login or Refresh)
  // =====================================================
  saveTokens(token: string, refreshToken: string) {

    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem("accessToken", token);
      localStorage.setItem("refreshToken", refreshToken);
    }

    // ✅ Update BehaviorSubject
    this.userTokenSubject.next(token);
  }

  // =====================================================
  // ✅ Safe Token Getters (SSR Safe)
  // =====================================================
  getAccessToken(): string | null {

    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem("accessToken");
    }

    return null;
  }

  getRefreshToken(): string | null {

    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem("refreshToken");
    }

    return null;
  }

  // =====================================================
  // ✅ Refresh Token API CALL
  // =====================================================
  refreshToken(): Observable<any> {

    const model = {
      accessToken: this.getAccessToken(),
      refreshToken: this.getRefreshToken()
    };

    return this.http.post(`${this.apiUrl}/refresh-token`, model);
  }

  // =====================================================
  // ✅ Start Refresh Timer (Call Only After Login)
  // =====================================================
  startRefreshTimer() {

    // ✅ Stop old timer if exists
    this.refreshSub?.unsubscribe();

    this.refreshSub = timer(15 * 60 * 1000, 15 * 60 * 1000)
      .pipe(
        switchMap(() => this.refreshToken())
      )
      .subscribe({
        next: (res: any) => {

          console.log("Token refreshed successfully");

          // ✅ Save new tokens (DO NOT restart timer)
          this.saveTokens(res.accessToken, res.refreshToken);
        },
        error: () => {
          console.log("Refresh token failed → Logout");
          this.logout();
        }
      });
  }

  // =====================================================
  // ✅ Logout
  // =====================================================
  logout() {

    this.refreshSub?.unsubscribe();

    if (isPlatformBrowser(this.platformId)) {
      localStorage.clear();
    }

    this.userTokenSubject.next(null);
  }

  // =====================================================
  // ✅ Check Login Status
  // =====================================================
  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }
}
