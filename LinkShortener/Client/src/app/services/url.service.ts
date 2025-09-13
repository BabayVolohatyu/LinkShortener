import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environment.development';

@Injectable({
  providedIn: 'root'
})
export class UrlService {
  private apiUrl = `${environment.apiUrl}/index`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl, { withCredentials: true });
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`, { withCredentials: true });
  }

  create(originalUrl: string): Observable<any> {
    return this.http.post<any>(
      this.apiUrl,
      { originalUrl },
      { withCredentials: true }
    );
  }

  update(id: number, dto: any): Observable<any> {
    return this.http.put<any>(
      `${this.apiUrl}/${id}`,
      dto,
      { withCredentials: true }
    );
  }

  delete(id: number): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/${id}`,
      {},
      { withCredentials: true }
    );
  }
  goToAbout(): void {
    window.location.href = `${environment.apiUrl}/about`;
  }
}
