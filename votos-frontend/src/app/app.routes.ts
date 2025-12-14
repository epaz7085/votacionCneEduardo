import { Routes } from '@angular/router';
import { LoginComponent } from './votes/pages/login/login';
import { VotesComponent } from './votes/pages/votes/votes.component';
import { AuthGuard } from './votes/guards/auth.guard';
import { RoleGuard } from './votes/guards/role.guard';

export const routes: Routes = [

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'votes',
    component: VotesComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'votante' }
  },

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];
