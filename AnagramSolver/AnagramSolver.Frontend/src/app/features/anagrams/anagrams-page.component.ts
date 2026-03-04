import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AnagramsApiService } from '../../core/services/anagrams-api.service';
import { mapApiError } from '../../core/utils/map-api-error';

@Component({
  selector: 'app-anagrams-page',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './anagrams-page.component.html',
  styleUrl: './anagrams-page.component.css'
})
export class AnagramsPageComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly anagramsApi = inject(AnagramsApiService);
  private readonly cdr = inject(ChangeDetectorRef);

  readonly minLength = environment.minInputWordsLength;
  readonly form = this.fb.nonNullable.group({
    word: ['', [Validators.required, Validators.minLength(environment.minInputWordsLength)]]
  });

  isLoading = false;
  errorMessage: string | null = null;
  searchedWord: string | null = null;
  anagrams: string[] = [];

  ngOnInit(): void {
    const prefillWord = this.route.snapshot.queryParamMap.get('word');

    if (prefillWord) {
      this.form.controls.word.setValue(prefillWord);
      void this.search(prefillWord);
    }
  }

  async onSubmit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    await this.search(this.form.controls.word.getRawValue().trim());
  }

  private async search(word: string): Promise<void> {
    this.isLoading = true;
    this.errorMessage = null;
    this.anagrams = [];
    this.searchedWord = word;

    try {
      this.anagrams = await firstValueFrom(this.anagramsApi.getAnagrams(word));
    } catch (error) {
      this.errorMessage = mapApiError(error, 'Failed to fetch anagrams.');
    } finally {
      this.isLoading = false;
      this.cdr.detectChanges();
    }
  }
}
