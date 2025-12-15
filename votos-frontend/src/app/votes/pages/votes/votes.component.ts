import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VoteService } from '../../services/vote.service';
import { CandidateService } from '../../services/candidate.service';
import { AlreadyVotedComponent } from '../already-voted/already-voted.component';

@Component({
  selector: 'app-votes',
  standalone: true,
  imports: [CommonModule, AlreadyVotedComponent],
  templateUrl: './votes.component.html',
  styleUrls: ['./votes.component.css']
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
