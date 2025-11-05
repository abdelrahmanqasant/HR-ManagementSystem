import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IUser } from '../interfaces/iuser';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private currentUserSource = new BehaviorSubject<IUser | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  baseURL = environment.baseUrl;

  constructor(private http: HttpClient) {
    this.loadCurrentUser();
  }

  login(email: string, password: string) {
    return this.http
      .post<IUser>(`${this.baseURL}/ApplicationUser/login`, {
        email: email,
        password: password,
      })
      .pipe(
        map((response: IUser) => {
          const user = response;
          if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
        })
      );
  }

  setCurrentUser(user: IUser) {
    this.currentUserSource.next(user);
  }
  private loadCurrentUser() {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      const user: IUser = JSON.parse(userStr);
      this.currentUserSource.next(user);
    }
  }
  logout(): Observable<any> {
    return this.http.post(`${this.baseURL}/ApplicationUser/logout`, {}).pipe(
      map(() => {
        localStorage.removeItem('user');
        this.currentUserSource.next(null);
        return { message: 'Logged out successfully' };
      })
    );
  }

  forceLogout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
