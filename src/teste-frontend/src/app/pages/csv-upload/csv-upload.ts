import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-csv-upload',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './csv-upload.html',
  styleUrls: ['./csv-upload.scss'],
})
export class CsvUploadComponent {
  log: any = null;
  isLoading = false;
  apiUrl:string =  'http://localhost:54152'

  constructor(private http: HttpClient) {}

  handleUpload(event: Event) {
    event.preventDefault();

    const form = event.target as HTMLFormElement;
    const input = form.querySelector<HTMLInputElement>('input[type=file]');
    const file = input?.files?.[0];
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    this.isLoading = true;
    this.http.post(`${this.apiUrl}/api/import/csv`, formData).subscribe({
      next: (response) => {
        this.log = response;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Erro ao enviar CSV:', err);
        this.log = { error: 'Falha no upload' };
        this.isLoading = false;
      },
    });
  }
}
