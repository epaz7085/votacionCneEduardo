import {
  Component,
  OnInit,
  ChangeDetectorRef,
  ViewChild,
  ElementRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Candidate, CandidateService } from '../../services/candidate.service';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

import {
  Chart,
  BarController,
  BarElement,
  CategoryScale,
  LinearScale,
  PieController,
  ArcElement,
  Tooltip,
  Legend
} from 'chart.js';

Chart.register(
  BarController,
  BarElement,
  CategoryScale,
  LinearScale,
  PieController,
  ArcElement,
  Tooltip,
  Legend
);

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css'],
  
})
export class AdminDashboardComponent implements OnInit {

  candidates: Candidate[] = [];
  loading = true;
  error = '';

  barChart: Chart | null = null;
  pieChart: Chart | null = null;

  @ViewChild('barCanvas') barCanvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild('pieCanvas') pieCanvas!: ElementRef<HTMLCanvasElement>;

  constructor(
    private candidateService: CandidateService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadCandidates();
  }

  get totalVotes(): number {
    return this.candidates.reduce((sum, c) => sum + c.votesCount, 0);
  }

  get winner() {
    if (!this.candidates.length) return null;
    return [...this.candidates].sort((a, b) => b.votesCount - a.votesCount)[0];
  }

  loadCandidates() {
    this.loading = true;
    this.error = '';

    this.candidateService.getAll().subscribe({
      next: data => {
        this.candidates = data;
        this.loading = false;
        this.cdr.detectChanges();

        setTimeout(() => this.renderCharts());
      },
      error: () => {
        this.error = 'Error cargando candidatos';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  renderCharts() {
    if (!this.barCanvas || !this.pieCanvas) return;

    const labels = this.candidates.map(c => c.name);
    const votes = this.candidates.map(c => c.votesCount);

    const colors = ['#4CAF50', '#2196F3', '#FF9800', '#E91E63'];

    this.barChart?.destroy();
    this.pieChart?.destroy();

    // 📊 Gráfico de barras
    this.barChart = new Chart(this.barCanvas.nativeElement, {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          data: votes,
          backgroundColor: colors.slice(0, votes.length)
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false }
        }
      }
    });

    // 🥧 Gráfico circular
    this.pieChart = new Chart(this.pieCanvas.nativeElement, {
      type: 'pie',
      data: {
        labels,
        datasets: [{
          data: votes,
          backgroundColor: colors.slice(0, votes.length)
        }]
      },
      options: {
        responsive: true
      }
    });
    
  }

  exportToPDF() {
    const doc = new jsPDF();

    // ===== TÍTULO =====
    doc.setFontSize(16);
    doc.text('Reporte de Resultados - VotoSeguro', 14, 15);

    doc.setFontSize(10);
    doc.text(`Fecha: ${new Date().toLocaleString()}`, 14, 22);

    doc.setFontSize(12);
    doc.text(`Total de votos: ${this.totalVotes}`, 14, 32);

    if (this.winner) {
      doc.text(
        `Candidato ganador: ${this.winner.name} (${this.winner.party}) con ${this.winner.votesCount} votos`,
        14,
        40
      );
    }

    // ===== TABLA =====
    const tableData = this.candidates.map(c => [
      c.name,
      c.party,
      c.votesCount
    ]);

    autoTable(doc, {
      startY: 50,
      head: [['Nombre', 'Partido', 'Votos']],
      body: tableData
    });

    // Posición después de la tabla
    const finalY = (doc as any).lastAutoTable.finalY + 10;

    // ===== GRÁFICA DE BARRAS =====
    const barImage = this.barCanvas.nativeElement.toDataURL('image/png');
    doc.text('Gráfica de votos por candidato', 14, finalY + 10);
    doc.addImage(barImage, 'PNG', 14, finalY + 15, 80, 60);

    // ===== GRÁFICA CIRCULAR =====
    const pieImage = this.pieCanvas.nativeElement.toDataURL('image/png');
    doc.text('Distribución de votos', 110, finalY + 10);
    doc.addImage(pieImage, 'PNG', 110, finalY + 15, 80, 60);

    // ===== GUARDAR =====
    doc.save('reporte_votacion.pdf');
  }
}
