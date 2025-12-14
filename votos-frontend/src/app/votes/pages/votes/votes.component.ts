import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VoteService } from '../../services/vote.service';
import { CandidateService } from '../../services/candidate.service';

@Component({
  selector: 'app-votes',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h2>Panel de Votación</h2>

    <div *ngIf="loading">Cargando...</div>

    <div *ngIf="!loading && hasVoted">
      <h3>Ya votaste</h3>
      <p>No puedes volver a votar.</p>
    </div>

    <div *ngIf="!loading && !hasVoted">
      <h3>Selecciona un candidato</h3>

      <div *ngFor="let c of candidates" style="border:1px solid #ccc; padding:10px; margin-bottom:10px;">
        <h4>{{ c.name }}</h4>
        <p><strong>Partido:</strong> {{ c.party }}</p>

        <ul>
          <li *ngFor="let p of c.proposals">{{ p }}</li>
        </ul>

        <button (click)="vote(c.id)">Votar</button>
      </div>
    </div>
  `
})
export class VotesComponent implements OnInit {

  loading = true;
  hasVoted = false;
  candidates: any[] = [];

  constructor(
    private voteService: VoteService,
    private candidateService: CandidateService
  ) {}

  ngOnInit(): void {
    this.voteService.getVoteStatus().subscribe({
      next: (res: any) => {
        this.hasVoted = res.hasVoted;

        if (!this.hasVoted) {
          this.loadCandidates();
        } else {
          this.loading = false;
        }
      },
      error: (err) => {
        console.error('Error estado voto', err);
        this.loading = false;
      }
    });
  }

  loadCandidates(): void {
    this.candidateService.getAll().subscribe({
      next: (data: any[]) => {
        this.candidates = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error candidatos', err);
        this.loading = false;
      }
    });
  }

  vote(candidateId: string): void {
    if (!confirm('¿Confirmas tu voto?')) return;

    this.voteService.vote(candidateId).subscribe({
      next: () => {
        alert('Voto registrado correctamente');
        this.hasVoted = true;
      },
      error: (err) => {
        alert(err.error?.message || 'Error al votar');
      }
    });
  }
}
