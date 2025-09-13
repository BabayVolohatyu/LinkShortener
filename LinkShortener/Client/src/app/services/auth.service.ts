import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenKey = 'auth_token';
  private jwtHelper = new JwtHelperService();

  constructor(private router: Router) {}

  // Save token after login
  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  clearToken(): void {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    return token != null && !this.jwtHelper.isTokenExpired(token);
  }

  getDecodedToken(): any {
    const token = this.getToken();
    if (!token) return null;
    return this.jwtHelper.decodeToken(token);
  }

  getUserId(): number | null {
    const decoded = this.getDecodedToken();
    return decoded?.id ? Number(decoded.id) : null;
  }

  getUserName(): string {
    return this.getClaim('unique_name') || this.getClaim('name') || 'Unknown';
  }

  hasRole(role: string): boolean {
    const decoded = this.getDecodedToken();
    if (!decoded || !decoded.role) return false;

    if (Array.isArray(decoded.role)) {
      return decoded.role.includes(role);
    }
    return decoded.role === role;
  }

  getClaim(key: string): string | null {
    const decoded = this.getDecodedToken();
    return decoded?.[key] || null;
  }

  isAdmin(): boolean {
    return this.hasRole('Admin');
  }

  isUser(): boolean {
    return this.hasRole('User');
  }
}
