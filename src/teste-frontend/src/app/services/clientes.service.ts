import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ClientesService {
  constructor(private http: HttpClient) {}

  apiUrl:string =  'http://localhost:54152'

  getClientes(filtro: string, mensalista: string): Observable<any> {
    const url = `${this.apiUrl}/api/clientes?pagina=1&tamanho=20&filtro=${encodeURIComponent(filtro)}&mensalista=${mensalista}`;
    return this.http.get(url);
  }

  createCliente(data: any): Observable<any> {''
    return this.http.post(`${this.apiUrl}/api/clientes`, data);
  }

  deleteCliente(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/clientes/${id}`);
  }

  updateCliente(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/clientes/${id}`, data);
  }
}
