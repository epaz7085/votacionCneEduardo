import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CandidateService, Candidate } from '../../services/candidate.service';

@Component({
  selector: 'app-admin-candidates',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-candidates.component.html',
  styleUrls: ['./admin-candidates.component.css']
})
export class AdminCandidatesComponent implements OnInit {

  candidates: Candidate[] = [];
  loading = false;
  error = '';

  // FORM
  name = '';
  party = '';
  photoUrl = '';
  logoUrl = '';
  proposalsText = '';

  constructor(
    private candidateService: CandidateService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCandidates();
  }

  loadCandidates() {
    this.candidateService.getAll().subscribe({
      next: data => {
        this.candidates = data;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Error cargando candidatos';
      }
    });
  }

  createCandidate() {
    if (!this.name || !this.party) {
      alert('Nombre y partido son obligatorios');
      return;
    }

    const candidate = {
      name: this.name,
      party: this.party,
      photoUrl: this.photoUrl,
      logoUrl: this.logoUrl,
      proposals: this.proposalsText
        ? this.proposalsText.split('\n')
        : []
    };

    this.candidateService.create(candidate).subscribe({
      next: () => {
        alert('Candidato creado correctamente');
        this.resetForm();
        this.loadCandidates();
      },
      error: err => {
        alert(err.error || 'Error al crear candidato');
      }
    });
  }

  deleteCandidate(id: string) {
    if (!confirm('¿Seguro que deseas eliminar este candidato?')) return;

    this.candidateService.delete(id).subscribe({
      next: () => {
        alert('Candidato eliminado');
        this.loadCandidates();
      },
      error: err => {
        alert(err.error || 'No se puede eliminar (tiene votos)');
      }
    });
  }

  resetForm() {
    this.name = '';
    this.party = '';
    this.photoUrl = '';
    this.logoUrl = '';
    this.proposalsText = '';
  }
}
