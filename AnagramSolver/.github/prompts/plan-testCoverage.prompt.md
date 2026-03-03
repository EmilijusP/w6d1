## Plan: Maximize Coverage for BusinessLogic & WebApp

To reach 80% test coverage for the Business Logic and Web App projects, we need to add missing unit tests for currently uncovered services and controllers. We will also run a coverage report to pinpoint any failing or ignored tests in the existing suites and fix them.

**Steps**
1. Create tests for `InputNormalizationService` in a new [AngaramSolver.BusinessLogic.Tests/InputNormalizationServiceTests.cs](AngaramSolver.BusinessLogic.Tests/InputNormalizationServiceTests.cs) file to cover text preprocessing logic.
2. Create tests for `FilterPipeline` in a new [AngaramSolver.BusinessLogic.Tests/FilterPipelineTests.cs](AngaramSolver.BusinessLogic.Tests/FilterPipelineTests.cs) file.
3. Create tests for `MemoryCache` in a new [AngaramSolver.BusinessLogic.Tests/MemoryCacheTests.cs](AngaramSolver.BusinessLogic.Tests/MemoryCacheTests.cs) file.
4. Create tests for `AnagramFinder` in a new [AngaramSolver.BusinessLogic.Tests/AnagramFinderTests.cs](AngaramSolver.BusinessLogic.Tests/AnagramFinderTests.cs) file.
5. Create tests for `WordsController` in a new [AnagramSolver.WebApp.Tests/WordsControllerTests.cs](AnagramSolver.WebApp.Tests/WordsControllerTests.cs) file to verify standard web MVC actions and view models.
6. Review existing tests ([AngaramSolver.BusinessLogic.Tests/AnagramAlgorithmTests.cs](AngaramSolver.BusinessLogic.Tests/AnagramAlgorithmTests.cs), [AnagramSolver.WebApp.Tests/HomeControllerTests.cs](AnagramSolver.WebApp.Tests/HomeControllerTests.cs), etc.) to fix any failing tests or poorly mocked dependencies blocking coverage paths.

**Verification**
Run `dotnet test --collect:"XPlat Code Coverage"` in your terminal. We can then use the `reportgenerator` global tool (`dotnet tool install -g dotnet-reportgenerator-globaltool`) to generate an HTML report, allowing us to visually verify that the `BusinessLogic` and `WebApp` projects meet the 80% line and branch coverage threshold.

**Decisions**
- **Scope**: Excluded `AnagramSolver.Api`, focusing purely on `AnagramSolver.BusinessLogic` and `AnagramSolver.WebApp` to meet your requirements.
- **Framework**: Standardizing on `xUnit` for any net-new testing files.
