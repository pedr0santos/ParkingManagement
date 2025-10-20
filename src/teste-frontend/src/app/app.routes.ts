import { Routes } from '@angular/router';
import { ClientesComponent } from './pages/clientes/clientes';
import { VeiculosComponent } from './pages/veiculos/veiculos';
import { FaturamentoComponent } from './pages/faturamento/faturamento';
import { CsvUploadComponent } from './pages/csv-upload/csv-upload';


export const routes: Routes = [
  { path: '', redirectTo: 'clientes', pathMatch: 'full' },

  { path: 'clientes', component: ClientesComponent },
  { path: 'veiculos', component: VeiculosComponent },
  { path: 'faturamento', component: FaturamentoComponent },
  { path: 'csv-upload', component: CsvUploadComponent },

  { path: '**', redirectTo: 'clientes' } // rota coringa
];
