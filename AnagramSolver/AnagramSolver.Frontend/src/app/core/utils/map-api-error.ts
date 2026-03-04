import { HttpErrorResponse } from '@angular/common/http';

export function mapApiError(error: unknown, fallbackMessage: string): string {
  if (!(error instanceof HttpErrorResponse)) {
    return fallbackMessage;
  }

  if (typeof error.error === 'string' && error.error.trim().length > 0) {
    return error.error;
  }

  if (error.error && typeof error.error.message === 'string') {
    return error.error.message;
  }

  if (error.status === 0) {
    return 'Cannot reach API. Check if backend is running and CORS is configured.';
  }

  if (error.message) {
    return error.message;
  }

  return fallbackMessage;
}
