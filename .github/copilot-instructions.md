# GitHub Copilot Instructions - Project Standards

To ensure high code quality and architectural consistency, please follow these guidelines when generating code for this project:

### 1. Architectural Principles (SOLID & OOP)
* **SOLID Principles:** Always adhere to SOLID principles. Prioritize Single Responsibility (one class, one responsibility) and Dependency Inversion (depend on abstractions, not concretions).
* **Object-Oriented Design:** Follow strict OOP patterns. Classes should be open for extension but closed for modification.
* **Naming Conventions:** Use descriptive and meaningful names for files, classes, and methods that reflect their intent (e.g., `ProcessOrderCommandHandler.cs` instead of `OrderManager.cs`). Follow language-specific casing (e.g., PascalCase for C#/.NET).

### 2. Testing Standards
When writing unit tests, you must strictly follow these requirements:
* **Arrange, Act, Assert (AAA):** Structure every test into these three distinct sections. Use comments to separate them.
* **Fluent Assertions:** Use "Fluent Assertions" syntax for the Assert phase (e.g., `result.Should().BeEquivalentTo(expectedValue);`) to ensure readability.
* **Descriptive Test Naming:** Use the pattern `MethodName_StateUnderTest_ExpectedBehavior` for all test methods.

### 3. Code Quality & Clean Code
* **DRY (Don't Repeat Yourself):** Avoid logic duplication. Abstract repetitive code into reusable services or helper methods.
* **Self-Documenting Code:** Write clear, readable code that minimizes the need for comments. Add comments only for complex business logic or "why" decisions.
* **Explicit Dependency Injection:** Prefer constructor injection over service location or manual instantiation.

### 4. Error Handling & Performance
* **Specific Exceptions:** Avoid generic `catch (Exception)`. Always catch and throw specific, meaningful exceptions.
* **Asynchronous Programming:** Prefer asynchronous patterns (`async/await`) for all I/O bound operations to prevent thread blocking.
* **Null Safety:** Use modern language features for null-checking (e.g., Nullable Reference Types, null-conditional operators).

---
*Note: If the suggested solution contradicts these principles, prioritize the principles over brevity.*