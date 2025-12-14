import { Routes } from '@angular/router';

import { LoginComponent } from './votes/pages/login/login';
import { VotesComponent } from './votes/pages/votes/votes.component';
import { AdminDashboardComponent } from './votes/pages/admin-dashboard/admin-dashboard.component';
import { AdminVotersComponent } from './votes/pages/admin-voters/admin-voters.component';
import { AdminCandidatesComponent } from './votes/pages/admin-candidates/admin-candidates.component';
import { RegisterComponent } from './votes/pages/register/register.component';

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
    canActivate: [AuthGuard]
  },

  {
  path: 'admin',
  component: AdminDashboardComponent,
  canActivate: [AuthGuard, RoleGuard],
  data: { role: 'admin' }
  },

  {
    path: 'admin/users',
    component: AdminVotersComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'admin' }
  },
  
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  
  {
  path: 'admin/candidates',
  component: AdminCandidatesComponent,
  canActivate: [AuthGuard, RoleGuard],
  data: { role: 'admin' }
  },

  {
  path: 'register',
  component: RegisterComponent
  },

];
