import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // 👈 Importamos ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-already-voted',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './already-voted.component.html'
})
export class AlreadyVotedComponent implements OnInit {

  loading = true;
  error = '';

  votedForName = '';
  voteTimestamp: string | null = null;

  // Inyectamos ChangeDetectorRef en el constructor
  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit(): void {
    this.authService.getMe().subscribe({
      next: (res) => {
        this.votedForName = res.votedForName;
        this.votedForName = res.votedForName || '';

        this.voteTimestamp = res.voteTimestamp
        ? new Date(res.voteTimestamp).toISOString()
        : null;

        this.loading = false;
        this.cdr.detectChanges(); // 👈 Forzamos la detección de cambios
      },
      error: () => {
        this.error = 'Error cargando información del voto';
        this.loading = false;
        this.cdr.detectChanges(); // 👈 Forzamos la detección de cambios en caso de error
      }
    });
  }
}