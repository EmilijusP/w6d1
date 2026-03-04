import { WordModel } from './word.model';

export interface PaginationResponse {
  items: WordModel[];
  currentPage: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
