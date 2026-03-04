export interface WordModel {
  id: number;
  lemma: string | null;
  form: string | null;
  word: string;
  frequency: number;
}
