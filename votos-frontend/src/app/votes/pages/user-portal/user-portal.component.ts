import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-portal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-portal.component.html',
  styleUrls: ['./user-portal.component.css']
})
export class UserPortalComponent {

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  go(path: string) {
    this.router.navigate([path]);
  }

  logout() {
    this.authService.logout();
  }
}
