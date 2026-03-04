import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { CreateWordRequest } from '../models/create-word-request.model';
import { PaginationResponse } from '../models/pagination-response.model';

@Injectable({
  providedIn: 'root'
})
export class WordsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  getPagedWords(page: number, pageSize: number): Observable<PaginationResponse> {
    return this.http.get<PaginationResponse>(`${this.baseUrl}/words/${page}/${pageSize}`);
  }

  createWord(request: CreateWordRequest): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/words`, request);
  }

  deleteWord(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/words/${id}`);
  }

  downloadDictionaryFile(fileName: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/words/download/${encodeURIComponent(fileName)}`, {
      responseType: 'blob'
    });
  }
}
