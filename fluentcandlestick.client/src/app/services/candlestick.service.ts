import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CandleStick } from '../models/candlestick.model';

@Injectable({
  providedIn: 'root'
})
export class CandleStickService {
  private apiUrl = '/api/candlestick';

  constructor(private http: HttpClient) { }

  getCandleStickData(): Observable<CandleStick[]> {
    return this.http.get<CandleStick[]>(this.apiUrl);
  }

  importCsvData(file: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/import`, formData, {
      responseType: 'text'
    });
  }
}
