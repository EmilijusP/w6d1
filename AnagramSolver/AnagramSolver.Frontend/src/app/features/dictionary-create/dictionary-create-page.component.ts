import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { WordsApiService } from '../../core/services/words-api.service';
import { mapApiError } from '../../core/utils/map-api-error';

@Component({
  selector: 'app-dictionary-create-page',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './dictionary-create-page.component.html',
  styleUrl: './dictionary-create-page.component.css'
})
export class DictionaryCreatePageComponent {
  private readonly fb = inject(FormBuilder);
  private readonly wordsApi = inject(WordsApiService);
  private readonly router = inject(Router);
  private readonly cdr = inject(ChangeDetectorRef);

  readonly form = this.fb.nonNullable.group({
    word: ['', [Validators.required]],
    lemma: [''],
    form: ['']
  });

  isSubmitting = false;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  async onSubmit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.successMessage = null;
    this.errorMessage = null;

    const payload = {
      word: this.form.controls.word.getRawValue().trim(),
      lemma: this.form.controls.lemma.getRawValue().trim() || '-',
      form: this.form.controls.form.getRawValue().trim() || '-',
      frequency: 1
    };

    try {
      await firstValueFrom(this.wordsApi.createWord(payload));
      this.successMessage = `Word "${payload.word}" added successfully.`;

      setTimeout(() => {
        void this.router.navigate(['/dictionary']);
      }, 800);
    } catch (error) {
      this.errorMessage = mapApiError(error, 'Failed to create dictionary word.');
    } finally {
      this.isSubmitting = false;
      this.cdr.detectChanges();
    }
  }
}
