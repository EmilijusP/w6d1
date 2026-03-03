---
description: Generates high-quality unit tests following the project's architectural standards.
---

# Generate Unit Tests

Generate unit tests for the selected code. Follow these strict requirements:

## 1. Structure (AAA Pattern)
Every test method must be clearly divided into three sections using comments:
- `// Arrange`: Initialize variables, objects, and mock dependencies.
- `// Act`: Execute the method being tested.
- `// Assert`: Verify the results.

## 2. Assertions
- Use **Fluent Assertions** library syntax for all assertions.
- Example: `result.Should().Be(expected);` or `action.Should().Throw<ArgumentException>();`.

## 3. Naming Convention
- Use the following naming pattern for test methods: `MethodName_StateUnderTest_ExpectedBehavior`.
- Example: `CalculateTotal_WithValidItems_ReturnsCorrectSum`.

## 4. Technical Details
- If the class has dependencies, use a mocking framework (like Moq) to mock them.
- Follow **SOLID** principles and ensure the tests are isolated.
- Aim for 100% coverage of the logic in the provided code snippet, including edge cases and error handling.

## 5. Output Format
Provide only the code for the test class. Ensure all necessary using statements and namespace definitions are included.

## 6. Project Structure & File Management
- **Target Project:** Locate the corresponding test project (e.g., if testing `ProjectName`, look for `TestProjects/ProjectName.Tests`). If it doesn't exist, suggest creating a new test project with that naming convention.
- **File Placement:** - If a test file for the selected class does not exist, create a new file named `{ClassName}Tests.cs` in the mirrored folder structure of the test project.
    - If `{ClassName}Tests.cs` already exists, analyze its content and either append new test cases or update existing ones to fix bugs or improve coverage.
- **Namespace Alignment:** Ensure the namespace follows the pattern `[ProjectName].Tests.[FolderStructure]`.
- **Reference Check:** Ensure the test file includes necessary `using` statements for the project being tested, `Xunit`, `FluentAssertions`, `Moq`.
---

**Code to test:**
{{selection}}