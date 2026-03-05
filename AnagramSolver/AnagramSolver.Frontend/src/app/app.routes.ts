import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'anagrams'
  },
  {
    path: 'anagrams',
    loadComponent: () => import('./features/anagrams/anagrams-page.component').then((m) => m.AnagramsPageComponent)
  },
  {
    path: 'dictionary',
    loadComponent: () => import('./features/dictionary/dictionary-page.component').then((m) => m.DictionaryPageComponent)
  },
  {
    path: 'dictionary/new',
    loadComponent: () => import('./features/dictionary-create/dictionary-create-page.component').then((m) => m.DictionaryCreatePageComponent)
  },
  {
    path: 'chat',
    loadComponent: () => import('./features/chat/chat-page.component').then((m) => m.ChatPageComponent)
  },
  {
    path: '**',
    loadComponent: () => import('./features/not-found/not-found-page.component').then((m) => m.NotFoundPageComponent)
  }
];
