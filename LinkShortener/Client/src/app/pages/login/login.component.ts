import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { trigger, style, animate, transition } from '@angular/animations';

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
    private router: Router
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.isRegisterMode) {
      // Registration
      this.http.post<any>('http://localhost:5091/register', this.form.value)
        .subscribe({
          next: res => {
            localStorage.setItem('jwt', res.token);
            this.router.navigate(['/index']);
          },
          error: err => {
            this.errorMessage = err.error?.message || err.message || 'Registration failed';
          }
        });
    } else {
      // Login
      this.http.post<any>('http://localhost:5091/login', {
        email: this.form.value.email,
        password: this.form.value.password
      }).subscribe({
        next: res => {
          localStorage.setItem('jwt', res.token);
          this.router.navigate(['/index']);
        },
        error: err => {
          if (err.status === 404) {
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

    //add field on registration
    if (!this.form.contains('name')) {
      this.form.addControl('name', this.fb.control('', Validators.required));
    }
  }

  continueAnonymously() {
    localStorage.setItem('jwt', 'ANONYMOUS');
    this.router.navigate(['/index']);
  }
}
