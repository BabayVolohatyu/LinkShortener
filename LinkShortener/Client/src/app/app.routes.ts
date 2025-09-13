import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { IndexComponent } from './pages/index/index.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {path: 'index', component: IndexComponent},
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];

