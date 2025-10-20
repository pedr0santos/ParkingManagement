import { Component, OnInit } from '@angular/core';
import { VeiculosService } from '../../services/veiculos.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-veiculos',
  templateUrl: './veiculos.html',
  styleUrls: ['./veiculos.scss'],
  imports: [CommonModule, FormsModule],
})
export class VeiculosComponent implements OnInit {
  clientes: any[] = [];
  veiculos: any[] = [];

  clienteId: string = '';
  form = {
    placa: '',
    modelo: '',
    ano: '',
    clienteId: '',
  };

  editingId: string | null = null;
  editForm = { modelo: '', clienteId: '' };

  isLoadingClientes = false;
  isLoadingVeiculos = false;

  constructor(private veiculosService: VeiculosService) {}

  ngOnInit() {
    this.loadClientes();
  }

  loadClientes() {
    this.isLoadingClientes = true;
    this.veiculosService.getClientes().subscribe({
      next: (res) => {
        this.clientes = res.itens || [];
        this.isLoadingClientes = false;

        // Se não tiver clienteId selecionado, define o primeiro
        if (this.clientes.length && !this.clienteId) {
          this.clienteId = this.clientes[0].id;
          this.form.clienteId = this.clienteId;
          this.loadVeiculos();
        }
      },
      error: () => (this.isLoadingClientes = false),
    });
  }

  loadVeiculos() {
    this.isLoadingVeiculos = true;
    this.veiculosService.getVeiculos(this.clienteId).subscribe({
      next: (res) => {
        this.veiculos = Array.isArray(res.itens) ? res.itens : res;
        this.isLoadingVeiculos = false;
      },
      error: () => (this.isLoadingVeiculos = false),
    });
  }

  salvar() {
    const data = {
      placa: this.form.placa,
      modelo: this.form.modelo,
      ano: this.form.ano ? Number(this.form.ano) : null,
      clienteId: this.form.clienteId || this.clienteId,
    };

    this.veiculosService.createVeiculo(data).subscribe(() => {
      this.resetForm();
      this.loadVeiculos();
    });
  }

  startEdit(veiculo: any) {
    this.editingId = veiculo.id;
    this.editForm = {
      modelo: veiculo.modelo,
      clienteId: veiculo.clienteId,
    };
  }

  confirmEdit(veiculo: any) {
    const data = {
      placa: veiculo.placa,
      modelo: this.editForm.modelo,
      ano: veiculo.ano,
      clienteId: this.editForm.clienteId,
    };

    this.veiculosService.updateVeiculo(veiculo.id, data).subscribe({
      next: () => {
        // Atualiza apenas o item na lista local
        const index = this.veiculos.findIndex((v) => v.id === veiculo.id);
        if (index !== -1) {
          this.veiculos[index] = { ...veiculo, ...data };
        }
        this.cancelEdit();
      },
      error: (err) =>
        alert('Erro ao atualizar veículo: ' + err.error?.message || err),
    });
  }

  cancelEdit() {
    this.editingId = null;
    this.editForm = { modelo: '', clienteId: '' };
  }

  excluir(id: number) {
    if (!confirm('Deseja realmente excluir este veículo?')) return;

    this.veiculosService.deleteVeiculo(id).subscribe(() => {
      this.loadVeiculos();
    });
  }

  resetForm() {
    this.form = { placa: '', modelo: '', ano: '', clienteId: this.clienteId };
  }
}
