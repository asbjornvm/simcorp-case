# Triangle Classifier

Classifies a triangle given three side lengths. Requires [.NET 9 SDK](https://dotnet.microsoft.com/download).

## Run

```
dotnet run --project src/TriangleClassifier -- 3 4 5
```

Omit the arguments for interactive mode.

## Tests

TUnit runs as an executable, so use `dotnet run` instead of `dotnet test`:

```
dotnet run --project tests/TriangleClassifier.Tests
```

## Use as a library

```csharp
var classifier = new TriangleClassifier(new List<ITriangleProperty>
{
    new SideEqualityProperty(),
});

var result = Triangle.Create(3, 4, 5);
if (result.IsSuccess)
{
    var classifications = classifier.Classify(result.Value!);
    bool isScalene = classifications.Has("Shape", "Scalene");
}
else
{
    Console.WriteLine(result.Error);
}
```

`Triangle.Create` returns a `Result<T>` rather than throwing on invalid input.

## Extending

Implement `ITriangleProperty` and register it in `Program.cs`. `AngleTypeProperty.cs` is a ready-made example that's commented out by default.
