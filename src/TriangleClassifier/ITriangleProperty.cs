namespace TriangleClassifier;

// Implement this to add a new triangle check without touching any existing code.
public interface ITriangleProperty {
    string Category { get; }
    string? Evaluate(Triangle triangle);
}
