import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VoteService } from '../../services/vote.service';
import { CandidateService } from '../../services/candidate.service';

@Component({
  selector: 'app-votes',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h2>Panel de Votación</h2>

    <h3>Selecciona un candidato</h3>
    
    <div *ngIf="hasVoted">
      <h3>Ya votaste</h3>
      <p>No puedes volver a votar.</p>
    </div>

    <div *ngIf="!hasVoted">
      <p>Candidatos cargados: {{ candidates.length }}</p>

      <div *ngFor="let c of candidates"
           style="border:1px solid #000; padding:10px; margin:10px 0;">
        <h3>{{ c.name }}</h3>
        <p><strong>Partido:</strong> {{ c.party }}</p>

        <ul *ngIf="c.proposals && c.proposals.length > 0">
          <li *ngFor="let p of c.proposals">{{ p }}</li>
        </ul>

        <button (click)="vote(c.id)">Votar</button>
      </div>
    </div>
  `
})
export class VotesComponent implements OnInit {

  candidates: any[] = [];
  hasVoted = false; // Mantener la variable hasVoted

  constructor(
    private voteService: VoteService,
    private candidateService: CandidateService,
    private cdr: ChangeDetectorRef // 👈 INYECTADO: Para forzar la detección de cambios
  ) {}

  ngOnInit(): void {
    console.log('INIT VOTES');
    this.voteService.getVoteStatus().subscribe({
        next: (res: any) => {
            this.hasVoted = res.hasVoted;

            if (!this.hasVoted) {
                this.loadCandidates();
            }
            this.cdr.detectChanges(); // Forzar la actualización después de obtener el estado del voto
        },
        error: (err) => {
            console.error('Error estado voto', err);
            // Si hay error en el estado del voto, por seguridad se intenta cargar los candidatos.
            this.loadCandidates(); 
        }
    });
  }

  loadCandidates(): void {
    this.candidateService.getAll().subscribe({
        next: (data: any[]) => {
            console.log('CANDIDATES FROM API', data);
            this.candidates = data.map(c => ({
                ...c,
                proposals: c.proposals ?? [] // Asegura que proposals sea un array para el *ngIf
            }));
            this.cdr.detectChanges(); // 👈 SOLUCIÓN: Forzar la actualización
        },
        error: (err) => {
            console.error('ERROR CANDIDATES', err);
        }
    });
  }

  vote(candidateId: string): void {
    // Implementar la lógica de voto de nuevo
    if (!confirm('¿Confirmas tu voto?')) return;

    this.voteService.vote(candidateId).subscribe({
        next: () => {
            alert('Voto registrado correctamente');
            this.hasVoted = true;
            this.cdr.detectChanges(); // Forzar la actualización
        },
        error: (err) => {
            alert(err.error?.message || 'Error al votar');
        }
    });
  }
}