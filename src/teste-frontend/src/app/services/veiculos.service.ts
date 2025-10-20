import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class VeiculosService {
  constructor(private http: HttpClient) {}

  apiUrl:string =  'http://localhost:54152'

  getClientes(): Observable<any> {
    return this.http.get(`${this.apiUrl}/api/clientes?pagina=1&tamanho=100`);
  }

  getVeiculos(clienteId?: string): Observable<any> {
    const url = clienteId
      ? `${this.apiUrl}/api/veiculos?clienteId=${clienteId}`
      : `${this.apiUrl}/api/veiculos`;
    return this.http.get(url);
  }

  createVeiculo(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/veiculos`, data);
  }

  updateVeiculo(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/veiculos/${id}`, data);
  }

  deleteVeiculo(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/veiculos/${id}`);
  }
}
