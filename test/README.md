# hmlib.PersianDateTests

This is a test project for validating the functionality of the `hmlib.PersianDate` library — a .NET library for working with Jalali (Persian) dates. It includes comprehensive tests to ensure correctness, consistency, and compatibility with the expected behavior of `System.DateTime`.

## ✅ Goal

Many of the tests aim to **mimic the behavior of `System.DateTime`** as closely as possible — especially in parsing, formatting (`ToString`), and arithmetic. This ensures that users transitioning from or integrating with standard .NET date types will experience familiar and predictable behavior.

## 📁 Project Structure

- `TokenizerTests.cs`: Tests for the internal format token parser.
- `JalaliDateTimeTests/`: Core test suite for the `JalaliDateTime` struct:
  - `AddTests.cs`: Validates logic for adding years, months, and days.
  - `ConstructorTests.cs`: Tests constructors with various argument combinations.
  - `CastingAndOperators.cs`: Verifies explicit/implicit casts and operator overloads.
  - `ParseTests.cs`: Ensures accurate parsing of date strings, mirroring `DateTime.Parse`.
  - `ToStringTests.cs`: Ensures `ToString()` outputs match standard .NET formatting patterns.
  - `CultureTests/`: Validates localized behavior under:
    - `FarsiCultureTest.cs`
    - `InvariantCultureTest.cs`
    - `CultureTest.cs`
- `Examples.cs`: Sample usage examples.


## 🧪 Running Tests

Use the following command to run all test cases:

```bash
dotnet test
```
