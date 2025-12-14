import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VoteService } from '../../services/vote.service';
import { CandidateService } from '../../services/candidate.service';
import { AlreadyVotedComponent } from '../already-voted/already-voted.component';

@Component({
  selector: 'app-votes',
  standalone: true,
  imports: [CommonModule, AlreadyVotedComponent],
  template: `
    <h2>Panel de Votación</h2>

    <!-- YA VOTÓ -->
    <app-already-voted *ngIf="hasVoted"></app-already-voted>

    <!-- NO HA VOTADO -->
    <div *ngIf="!hasVoted">

      <h3>Selecciona un candidato</h3>

      <div *ngFor="let c of candidates"
           style="border:1px solid #000; padding:10px; margin:10px 0;">
        <h3>{{ c.name }}</h3>
        <p><strong>Partido:</strong> {{ c.party }}</p>

        <ul *ngIf="c.proposals.length">
          <li *ngFor="let p of c.proposals">{{ p }}</li>
        </ul>

        <button (click)="vote(c.id)">Votar</button>
      </div>

    </div>
  `
})
export class VotesComponent implements OnInit {

  candidates: any[] = [];
  hasVoted = false;

  constructor(
    private voteService: VoteService,
    private candidateService: CandidateService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.voteService.getVoteStatus().subscribe({
      next: (res) => {
        this.hasVoted = res.hasVoted;

        if (!this.hasVoted) {
          this.loadCandidates();
        }

        this.cdr.detectChanges();
      },
      error: () => {
        this.loadCandidates();
      }
    });
  }

  loadCandidates(): void {
    this.candidateService.getAll().subscribe({
      next: (data) => {
        this.candidates = data.map(c => ({
          ...c,
          proposals: c.proposals ?? []
        }));
        this.cdr.detectChanges();
      }
    });
  }

  vote(candidateId: string): void {
    if (!confirm('¿Confirmas tu voto?')) return;

    this.voteService.vote(candidateId).subscribe({
      next: () => {
        alert('Voto registrado correctamente');
        this.hasVoted = true;
        this.cdr.detectChanges();
      },
      error: (err) => {
        alert(err.error?.message || 'Error al votar');
      }
    });
  }
}
