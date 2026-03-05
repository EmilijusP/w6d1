import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';

import { AiChatApiService } from '../../core/services/ai-chat-api.service';
import { mapApiError } from '../../core/utils/map-api-error';

type ChatRole = 'user' | 'assistant';

interface ChatMessage {
  role: ChatRole;
  text: string;
}

@Component({
  selector: 'app-chat-page',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './chat-page.component.html',
  styleUrl: './chat-page.component.css'
})
export class ChatPageComponent implements OnInit {
  private readonly sessionStorageKey = 'anagramsolver-chat-session-id';
  private readonly fb = inject(FormBuilder);
  private readonly aiChatApi = inject(AiChatApiService);
  private readonly cdr = inject(ChangeDetectorRef);

  readonly form = this.fb.nonNullable.group({
    message: ['', [Validators.required]]
  });

  sessionId = this.getOrCreateSessionId();
  isLoadingHistory = false;
  isSending = false;
  errorMessage: string | null = null;
  messages: ChatMessage[] = [];

  ngOnInit(): void {
    void this.loadHistory();
  }

  async send(): Promise<void> {
    if (this.form.invalid || this.isSending) {
      this.form.markAllAsTouched();
      return;
    }

    const message = this.form.controls.message.getRawValue().trim();

    if (!message) {
      this.form.controls.message.setValue('');
      this.form.markAllAsTouched();
      return;
    }

    this.isSending = true;
    this.errorMessage = null;
    this.messages = [...this.messages, { role: 'user', text: message }];
    this.form.controls.message.setValue('');

    try {
      const response = await firstValueFrom(
        this.aiChatApi.sendMessage({
          message,
          sessionId: this.sessionId
        })
      );

      this.sessionId = response.sessionId;
      localStorage.setItem(this.sessionStorageKey, this.sessionId);
      this.messages = [...this.messages, { role: 'assistant', text: response.response }];
    } catch (error) {
      this.errorMessage = mapApiError(error, 'Failed to send chat message.');
    } finally {
      this.isSending = false;
      this.cdr.detectChanges();
    }
  }

  private async loadHistory(): Promise<void> {
    this.isLoadingHistory = true;
    this.errorMessage = null;

    try {
      const history = await firstValueFrom(this.aiChatApi.getHistory(this.sessionId));
      this.messages = history.map((item) => ({
        role: this.mapRole(item.role),
        text: this.normalizeContent(item.content)
      }));
    } catch (error) {
      if (error instanceof HttpErrorResponse && error.status === 404) {
        this.messages = [];
      } else {
        this.errorMessage = mapApiError(error, 'Failed to load chat history.');
      }
    } finally {
      this.isLoadingHistory = false;
      this.cdr.detectChanges();
    }
  }

  private getOrCreateSessionId(): string {
    const storedSessionId = localStorage.getItem(this.sessionStorageKey);

    if (storedSessionId) {
      return storedSessionId;
    }

    const newSessionId = this.createSessionId();
    localStorage.setItem(this.sessionStorageKey, newSessionId);

    return newSessionId;
  }

  private createSessionId(): string {
    if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
      return crypto.randomUUID();
    }

    return `session-${Date.now()}-${Math.random().toString(36).slice(2, 10)}`;
  }

  private mapRole(role: string): ChatRole {
    return role.toLowerCase().includes('user') ? 'user' : 'assistant';
  }

  private normalizeContent(content: unknown): string {
    if (typeof content === 'string') {
      return content;
    }

    if (Array.isArray(content)) {
      const parts = content
        .map((item) => {
          if (typeof item === 'string') {
            return item;
          }

          if (item && typeof item === 'object' && 'text' in item) {
            const text = (item as { text?: unknown }).text;
            return typeof text === 'string' ? text : '';
          }

          return '';
        })
        .filter((part) => part.length > 0);

      return parts.join(' ');
    }

    if (content && typeof content === 'object') {
      if ('text' in content) {
        const text = (content as { text?: unknown }).text;
        if (typeof text === 'string') {
          return text;
        }
      }

      return JSON.stringify(content);
    }

    return '';
  }
}
