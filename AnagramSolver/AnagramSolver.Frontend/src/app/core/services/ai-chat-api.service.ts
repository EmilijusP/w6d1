import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ChatRequest } from '../models/chat-request.model';
import { ChatResponse } from '../models/chat-response.model';

@Injectable({
  providedIn: 'root'
})
export class AiChatApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  sendMessage(request: ChatRequest): Observable<ChatResponse> {
    return this.http.post<ChatResponse>(`${this.baseUrl}/ai/chat`, request);
  }

  getHistory(sessionId: string): Observable<Array<{ role: string; content: unknown }>> {
    return this.http.get<Array<{ role: string; content: unknown }>>(
      `${this.baseUrl}/ai/chat/${encodeURIComponent(sessionId)}/history`
    );
  }
}
