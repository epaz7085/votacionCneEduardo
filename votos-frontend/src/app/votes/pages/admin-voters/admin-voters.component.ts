import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-admin-voters',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-voters.component.html'
})
export class AdminVotersComponent implements OnInit {

  users: User[] = [];
  loading = true;
  error = '';

  constructor(private userService: UserService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.userService.getAll().subscribe({
      next: data => {
        this.users = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Error cargando usuarios';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
