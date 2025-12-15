import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {

  email = '';
  password = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login() {
    if (!this.email || !this.password) return;

    this.loading = true;

    this.authService.login(this.email, this.password).subscribe({
      next: (res) => {
        localStorage.setItem('token', res.token);

        localStorage.setItem('user', JSON.stringify({
          userId: res.userId,
          email: res.email,
          fullName: res.fullName,
          role: res.role
        }));

        this.loading = false;

        // REDIRECCIÓN POR ROL
        if (res.role === 'admin') {
          this.router.navigate(['/admin/view']);
        } else {
          this.router.navigate(['/user']);
        }
      },
      error: () => {
        alert('Credenciales incorrectas');
        this.loading = false;
      }
    });
  }

}
