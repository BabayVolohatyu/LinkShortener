import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UrlService } from '../../services/url.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})
export class DetailsComponent implements OnInit {
url: any;
  form!: FormGroup;
  errorMessage = '';
  id!: number;
  canEdit = false;

  constructor(
    private route: ActivatedRoute,
    private urlService: UrlService,
    private fb: FormBuilder,
    public auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.loadUrl();
  }

  loadUrl(): void {
    this.urlService.getById(this.id).subscribe({
      next: data => {
        this.url = data;
        this.checkPermissions();
        this.form = this.fb.group({
          description: [this.url.description || '', [Validators.maxLength(500)]]
        });
      },
      error: err => {
        this.errorMessage = err?.error?.message || 'Could not load URL details';
      }
    });
  }

  checkPermissions(): void {
  if (!this.auth.isLoggedIn()) {
    this.canEdit = false;
    return;
  }

  const isAdmin = this.auth.isAdmin();
  const isOwner = this.url.createdBy === this.auth.getUserName();

  this.canEdit = isAdmin || isOwner;
    }

    save(): void {
    if (!this.canEdit || this.form.invalid) return;

    const updated = {
      ...this.url,
      description: this.form.value.description
    };

    this.urlService.update(this.id, updated).subscribe({
      next: res => {
        this.url = res;
        alert('Description updated successfully!');
      },
      error: err => {
        this.errorMessage = err?.error?.message || 'Failed to update description';
      }
    });
  }

  back(): void {
    this.router.navigate(['/index']);
  }
}
