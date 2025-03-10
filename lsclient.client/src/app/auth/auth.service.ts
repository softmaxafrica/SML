import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { DataService } from '../services/DataService';
import { LoginPayload } from '../models/LoginPayload';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { SecUser } from '../models/SecUser';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements CanActivate {
  private loggedInUserSubject: BehaviorSubject<SecUser | null>;
  public loggedInUser$: Observable<SecUser | null>;

  constructor(
    private dataService: DataService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: any // Inject PLATFORM_ID
  ) {
    const storedUser = this.getLoggedInUserFromStorage();
    this.loggedInUserSubject = new BehaviorSubject<SecUser | null>(storedUser);
    this.loggedInUser$ = this.loggedInUserSubject.asObservable();
  }

  login(email: string, password: string): Observable<SecUser> {
    const payload: LoginPayload = { email, password };

    return this.dataService.login('Login', payload).pipe(
      map((response: ApiResponse<SecUser>) => {
        if (response.success && response.data) {
          return response.data;
        }
        throw new Error('Login failed'); // Handle failed login
      }),
      tap(user => {
        if (user && isPlatformBrowser(this.platformId)) {
          sessionStorage.setItem('loggedInUser', JSON.stringify(user));
          this.loggedInUserSubject.next(user); // Set the user into the BehaviorSubject
        }
      })
    );
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      sessionStorage.removeItem('loggedInUser');
    }
    this.loggedInUserSubject.next(null);
    this.router.navigate(['/account/login']);
  }

  isLoggedIn(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return !!sessionStorage.getItem('loggedInUser');
    }
    return false; // Return false for SSR
  }

  getUserRole(): string | null {
    const user = this.getLoggedInUserFromStorage();
    return user?.role ?? null;
  }

  getCompanyId(): string | null {
    const user = this.getLoggedInUserFromStorage();
    return user?.userID ?? null;
  }

  getUserId(): string | null {
    const user = this.getLoggedInUserFromStorage();
    return user?.userID ?? null;
  }

  public getLoggedInUserFromStorage(): SecUser | null {
    if (isPlatformBrowser(this.platformId)) {
      const userJson = sessionStorage.getItem('loggedInUser');
      return userJson ? JSON.parse(userJson) : null;
    }
    return null; // Return null for SSR
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const isLoggedIn = this.isLoggedIn();
    if (!isLoggedIn) {
      this.router.navigate(['/account/login']);
    }
    return isLoggedIn;
  }
}
