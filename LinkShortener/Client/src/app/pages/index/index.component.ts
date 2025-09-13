import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators, FormGroup } from '@angular/forms';
import { UrlService } from '../../services/url.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-index',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  urls: any[] = [];
  form: FormGroup;
  errorMessage = '';

  constructor(
    private urlService: UrlService,
    private fb: FormBuilder,
    public auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      originalUrl: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadUrls();
    this.auth.loadUser().subscribe();
  }

  loadUrls(): void {
    this.urlService.getAll().subscribe({
      next: data => {
        this.urls = data || [];
      },
      error: err => {
        this.errorMessage = err?.error?.message || 'Could not load URLs';
      }
    });
  }

  addUrl(): void {
    if (this.form.invalid) return;

    const originalUrl = this.form.value.originalUrl?.trim();
    if (!originalUrl) return;

    this.urlService.create(originalUrl).subscribe({
      next: res => {
        this.urls = [...this.urls, res]; 
        this.form.reset();
      },
      error: err => {
        this.errorMessage = err?.error?.message || 'Failed to create URL';
      }
    });
  }

  deleteUrl(id: number): void {
    if (!confirm('Are you sure you want to delete this URL?')) return;

    this.urlService.delete(id).subscribe({
      next: () => {
        this.urls = this.urls.filter(u => u.id !== id);
      },
      error: err => {
        this.errorMessage = err?.error?.message || 'Failed to delete URL';
      }
    });
  }

  viewDetails(id: number): void {
    this.router.navigate(['/details', id]);
  }

  goToAbout() {
    this.urlService.goToAbout();
  }

  onLogout(): void {
  this.auth.logout().subscribe(() => {
    this.router.navigate(['/login']); // optional redirect
  });
}

}
