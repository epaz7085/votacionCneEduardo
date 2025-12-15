import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  fullName = '';
  email = '';
  password = '';
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

 register() {
  if (!this.fullName || !this.email || !this.password) {
    this.error = 'Todos los campos son obligatorios';
    return;
  }

  localStorage.clear();

  this.loading = true;
  this.error = '';

  this.authService.register({
    fullName: this.fullName,
    email: this.email,
    password: this.password
  }).subscribe({
    next: (res) => {

      localStorage.setItem('token', res.token);
      localStorage.setItem('user', JSON.stringify({
        userId: res.userId,
        email: res.email,
        fullName: res.fullName,
        role: res.role
      }));

      this.loading = false;

      this.router.navigate(['/votes']);
    },
    error: err => {
      this.error = err.error?.error || 'Error al registrar usuario';
      this.loading = false;
    }
  });
}
}