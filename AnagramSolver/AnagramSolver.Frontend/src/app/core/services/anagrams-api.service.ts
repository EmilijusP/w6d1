import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnagramsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  getAnagrams(word: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/anagrams/${encodeURIComponent(word)}`);
  }
}
