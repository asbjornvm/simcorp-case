# Triangle Classifier

Classifies a triangle by its side lengths. Requires [.NET 9 SDK](https://dotnet.microsoft.com/download).

## Run

**Interactive mode**
```
dotnet run --project src/TriangleClassifier
```

**Single input**
```
dotnet run --project src/TriangleClassifier -- 3 4 5
```

**Tests**

The test project uses [TUnit](https://github.com/thomhurst/TUnit) (v1.x). TUnit runs as a self-contained executable, so use `dotnet run` rather than `dotnet test`:

```
dotnet run --project tests/TriangleClassifier.Tests/TriangleClassifier.Tests.csproj
```

To filter by test name:
```
dotnet run --project tests/TriangleClassifier.Tests/TriangleClassifier.Tests.csproj -- --filter "Equilateral"
```

An HTML report is written to `tests/TriangleClassifier.Tests/bin/Debug/net9.0/TestResults/` after each run.

## Use as a library

The core domain is callable directly without going through the CLI:

```csharp
TriangleClassifier classifier = new TriangleClassifier(new List<ITriangleProperty>
{
    new SideEqualityProperty(),
});

Result<Triangle> result = Triangle.Create(3, 4, 5);
if (result.IsSuccess)
{
    IReadOnlyList<ClassificationResult> classifications = classifier.Classify(result.Value!);
    // -> [ClassificationResult("Shape", "Scalene")]

    bool isEquilateral = classifications.Has("Shape", "Equilateral");
    bool isIsosceles   = classifications.Has("Shape", "Isosceles");
    bool isScalene     = classifications.Has("Shape", "Scalene");
}
else
{
    Console.WriteLine(result.Error); // validation message
}
```

`Triangle.Create` returns a `Result<Triangle>` rather than throwing, so invalid input (negative sides, triangle inequality violations) is handled explicitly by the caller.

## Extend

Add a new check by implementing `ITriangleProperty` in its own file, then registering it in `Program.cs`:

```csharp
new AngleTypeProperty(),   // ← AngleTypeProperty.cs is a ready-made example
```
