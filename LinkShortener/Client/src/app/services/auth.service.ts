import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser: any = null;

  constructor(private http: HttpClient, private router: Router) {}

  loadUser(): Observable<any> {
    return this.http.get(`${environment.apiUrl}/me`, { withCredentials: true }).pipe(
      tap(user => this.currentUser = user),
      catchError(() => {
        this.currentUser = null;
        return of(null);
      })
    );
  }

  logout(): Observable<any> {
    return this.http.post(`${environment.apiUrl}/logout`, {}, { withCredentials: true })
      .pipe(
        tap(() => {
          this.currentUser = null; // clear frontend state
        })
      );
  }


  isLoggedIn(): boolean {
    return this.currentUser != null;
  }

  getUser(): any {
    return this.currentUser;
  }

  getUserName(): string {
    return this.currentUser?.name || 'Unknown';
  }

  isAdmin(): boolean {
    return this.currentUser?.role === 'Admin';
  }

  isUser(): boolean {
    return this.currentUser?.role === 'User';
  }
}
