## Plan: Angular Frontend for Anagram Solver

Build a new Angular (latest) SPA that talks to `AnagramSolver.Api` for:
1. Anagram search by typed word.
2. Paged dictionary browsing.
3. Dictionary management (add/delete/download).
AI chat is explicitly excluded. CSS will remain minimal structural only (no styling work).

**Steps**
1. Create Angular app baseline (`AnagramSolver.Frontend`) with routing + simple CSS setup only.
2. Add environment config for API base URL and typed models matching backend contracts.
3. Create API services:
   1. `AnagramsApiService` for `GET /api/anagrams/{word}`.
   2. `WordsApiService` for paged list, create, delete, download.
4. Configure routes:
   1. `/` -> redirect `/anagrams`
   2. `/anagrams`
   3. `/dictionary`
   4. `/dictionary/new`
   5. `**` fallback not-found
5. Build `Anagrams` page:
   1. Word input + submit.
   2. Results list + loading/error state.
   3. Frontend min-length validation aligned to API.
6. Build `Dictionary` page:
   1. Load paged words from API.
   2. First/prev/next/last controls from pagination flags.
   3. Delete action + refresh behavior.
   4. “Find anagrams” action to route into `/anagrams`.
7. Build `Dictionary Create` page:
   1. Reactive form (`word` required, `lemma/form` optional).
   2. Submit to `POST /api/words`.
   3. Success/error feedback and navigation.
8. Add dictionary file download interaction using `GET /api/words/download/{fileName}` (blob).
9. Cross-cutting reliability pass:
   1. Consistent API error mapping.
   2. Empty/loading/error states on each page.
   3. Confirm no AI chat route/service/component exists.
10. Integration prerequisite check:
   1. Verify/enable CORS in API for Angular dev origin.
11. Verification pass:
   1. Route checks.
   2. Functional checks for search/paging/create/delete/download.
   3. Angular test/build checks.

**Relevant files**
- `AnagramSolver.Api/Controllers/AnagramsController.cs` - anagram endpoint behavior and validation.
- `AnagramSolver.Api/Controllers/WordsController.cs` - paging/create/delete/download contracts.
- `AnagramSolver.Api/Program.cs` - CORS policy for Angular dev server origin.
- `AnagramSolver.Contracts/Models/WordModel.cs` - dictionary DTO shape.
- `AnagramSolver.Contracts/Models/PaginationResponse.cs` - paging DTO shape.
- `AnagramSolver.WebApp/Controllers/HomeController.cs` - search flow reference.
- `AnagramSolver.WebApp/Controllers/WordsController.cs` - dictionary flow reference.
- `AnagramSolver.WebApp/Views/Home/Index.cshtml` - search UI reference.
- `AnagramSolver.WebApp/Views/Words/Index.cshtml` - paging/actions reference.
- `AnagramSolver.WebApp/Views/Words/New.cshtml` - create form reference.

**Scope decisions**
- Included: search, paged dictionary, add/delete/download, Angular routing.
- Excluded: `AiChatController` UI and all styling/theming implementation.
