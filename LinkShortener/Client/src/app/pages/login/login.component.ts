import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { trigger, style, animate, transition } from '@angular/animations';
import { AuthService } from '../../services/auth.service'; 
import { environment } from '../../environment.development';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  animations: [
    trigger('slideIn', [
      transition(':enter', [
        style({ height: 0, opacity: 0 }),
        animate('300ms ease-out', style({ height: '*', opacity: 1 }))
      ])
    ])
  ]
})
export class LoginComponent {
  form: FormGroup;
  errorMessage = '';
  userNotFound = false;
  isRegisterMode = false;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService   
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.isRegisterMode) {
      // Registration
      this.http.post<any>(`${environment.apiUrl}/register`, this.form.value, { withCredentials: true })
        .subscribe({
          next: res => {
            // No need to store token manually
            this.router.navigate(['/index']);
          },
          error: err => {
            this.errorMessage = err.error?.message || err.message || 'Registration failed';
          }
        });
    } else {
      // Login
      this.http.post<any>(`${environment.apiUrl}/login`, {
        email: this.form.value.email,
        password: this.form.value.password
      }, { withCredentials: true }).subscribe({
        next: res => {
          // Token is stored in HTTP-only cookie
          this.router.navigate(['/index']);
        },
        error: err => {
          if (err.status === 401) {
            this.userNotFound = true;
          } else {
            this.errorMessage = err.error?.message || err.message || 'Login failed';
          }
        }
      });
    }
  }

  registerMode() {
    this.userNotFound = false;
    this.isRegisterMode = true;

    // add name field on registration
    if (!this.form.contains('name')) {
      this.form.addControl('name', this.fb.control('', Validators.required));
    }
  }

  continueAnonymously() {
    // Send a request to clear cookie if needed or just navigate
    this.router.navigate(['/index']);
  }
}
