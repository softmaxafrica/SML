import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { SecUser } from '../models/SecUser';
import { DataService } from './DataService';
import { LoginPayload } from '../models/LoginPayload';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private loggedInUserSubject: BehaviorSubject<SecUser | null>;
  public loggedInUser$: Observable<SecUser | null>;

  constructor(private dataService: DataService) {
    const storedUser = this.getLoggedInUser();
    this.loggedInUserSubject = new BehaviorSubject<SecUser | null>(storedUser);
    this.loggedInUser$ = this.loggedInUserSubject.asObservable();
  }

  

  logout(): void {
    sessionStorage.removeItem('loggedInUser');
    this.loggedInUserSubject.next(null);
  }

  getLoggedInUser(): SecUser | null {
    const user = sessionStorage.getItem('loggedInUser');
    return user ? JSON.parse(user) : null;
  }
}
