import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { switchMap, tap } from 'rxjs/operators';
import { BehaviorSubject, of } from 'rxjs';
import { FaturamentoService } from '../../services/faturamento.service';

@Component({
  selector: 'app-faturamento',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './faturamento.html',
  styleUrl: './faturamento.scss',
})
export class FaturamentoComponent {
 comp = '2025-08';
  faturas$ = new BehaviorSubject<any[] | null>(null);
  isLoading = false;

  // controle individual por fatura
  expandedFaturaId: string | null = null;
  placasPorFatura: Record<string, string[]> = {};

  constructor(private faturamentoService: FaturamentoService) {
    this.loadFaturas();
  }

  loadFaturas() {
    this.isLoading = true;
    this.faturamentoService
      .getFaturas(this.comp)
      .pipe(tap(() => (this.isLoading = false)))
      .subscribe({
        next: (data) => this.faturas$.next(data),
        error: () => (this.isLoading = false),
      });
  }

  gerarFaturas() {
    this.faturamentoService
      .gerarFaturas(this.comp)
      .pipe(switchMap(() => this.faturamentoService.getFaturas(this.comp)))
      .subscribe({
        next: (data) => this.faturas$.next(data),
      });
  }

  togglePlacas(faturaId: string) {
    // Se já está aberta, fecha
    if (this.expandedFaturaId === faturaId) {
      this.expandedFaturaId = null;
      return;
    }

    // Abre nova
    this.expandedFaturaId = faturaId;

    // Busca placas só se ainda não tiver em cache
    if (!this.placasPorFatura[faturaId]) {
      this.faturamentoService.getPlacas(faturaId).subscribe((placas) => {
        this.placasPorFatura[faturaId] = placas;
      });
    }
  }
}
