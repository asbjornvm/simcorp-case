namespace TriangleClassifier;

public sealed class TriangleClassifier {
    private readonly IReadOnlyList<ITriangleProperty> _properties;

    public TriangleClassifier(IEnumerable<ITriangleProperty> properties)
        => _properties = properties.ToList();

    public IReadOnlyList<ClassificationResult> Classify(Triangle triangle)
        => _properties
            .Select(p => new ClassificationResult(p.Category, p.Evaluate(triangle)))
            .ToList();
}

public sealed record ClassificationResult(string Category, string? Label);

public static class ClassificationResultExtensions {
    public static bool Has(this IReadOnlyList<ClassificationResult> results, string category, string label)
        => results.Any(r => r.Category == category && r.Label == label);
}
