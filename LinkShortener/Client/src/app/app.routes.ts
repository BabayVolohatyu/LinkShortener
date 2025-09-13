import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { IndexComponent } from './pages/index/index.component';
import { DetailsComponent } from './pages/details/details.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {path: 'index', component: IndexComponent},
  { path: 'details/:id', component: DetailsComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];

