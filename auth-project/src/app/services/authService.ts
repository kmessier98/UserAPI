import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { RegisterDTO } from '../DTOs/RegisterDTO';
import { LoginDTO } from '../DTOs/LoginDTO';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  apiUrl = 'https://localhost:7295/api/user';

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  isAuthenticated(): Observable<boolean> {
    return this.http
      .get(`${this.apiUrl}/me`, {
        withCredentials: true,
      })
      .pipe(
        map(() => true),
        catchError(() => of(false)),
      );
  }

  register(userData: RegisterDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  login(loginData: LoginDTO): Observable<any> {
    loginData.rememberMe = false;
    return this.http.post(`${this.apiUrl}/login`, loginData, {
      withCredentials: true,
    });
  }

  logout(): void {
    this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true }).subscribe(() => {
      localStorage.removeItem('currentUser');
      this.router.navigate(['/auth/login']);
    });
  }
}
