import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClientesService } from '../../services/clientes.service';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.html',
  styleUrls: ['./clientes.scss'],
  imports: [CommonModule, FormsModule],
})
export class ClientesComponent implements OnInit {
  filtro = '';
  mensalista = 'all';
  form = {
    id: 0,
    nome: '',
    telefone: '',
    endereco: '',
    mensalista: false,
    valorMensalidade: '',
  };

  clientes: any[] = [];
  isLoading = false;

  constructor(private clientesService: ClientesService) {}

  ngOnInit() {
    this.loadClientes();
  }

  loadClientes() {
    this.isLoading = true;
    this.clientesService.getClientes(this.filtro, this.mensalista).subscribe({
      next: (res) => {
        this.clientes = res.itens || [];
        this.isLoading = false;
      },
      error: () => (this.isLoading = false),
    });
  }

  editar(cliente: any) {
    this.form = {
      id: cliente.id,
      nome: cliente.nome,
      telefone: cliente.telefone,
      endereco: cliente.endereco,
      mensalista: cliente.mensalista,
      valorMensalidade: cliente.valorMensalidade ?? '',
    };
  }

  salvarOuEditar() {
    if (this.form.id) {
      const data = {
        nome: this.form.nome,
        telefone: this.form.telefone,
        endereco: this.form.endereco,
        mensalista: this.form.mensalista,
        valorMensalidade: this.form.valorMensalidade
          ? Number(this.form.valorMensalidade)
          : null,
      };

      this.clientesService.updateCliente(this.form.id, data).subscribe({
        next: (response) => {
          this.resetForm();
          this.loadClientes();
        },
        error: (error) => {
          console.error('Erro ao alterar o status:', error);
        },
      });
    } else {
      this.salvar();
    }
  }
  salvar() {
    const data = {
      nome: this.form.nome,
      telefone: this.form.telefone,
      endereco: this.form.endereco,
      mensalista: this.form.mensalista,
      valorMensalidade: this.form.valorMensalidade
        ? Number(this.form.valorMensalidade)
        : null,
    };

    this.clientesService.createCliente(data).subscribe(() => {
      this.resetForm();
      this.loadClientes();
    });
  }

  excluir(id: number) {
    this.clientesService.deleteCliente(id).subscribe(() => this.loadClientes());
  }

  resetForm() {
    this.form = {
      id: 0,
      nome: '',
      telefone: '',
      endereco: '',
      mensalista: false,
      valorMensalidade: '',
    };
  }
}
