import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FaturamentoService {
  constructor(private http: HttpClient) {}

  apiUrl:string =  'http://localhost:54152'

  getFaturas(competencia: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/api/faturas?competencia=${competencia}`);
  }

  gerarFaturas(competencia: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/faturas/gerar`, { competencia });
  }

  getPlacas(faturaId: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/api/faturas/${faturaId}/placas`);
  }
}
