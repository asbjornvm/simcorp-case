namespace TriangleClassifier;

// Epsilon guards against floating-point noise treating near-equal sides as distinct.
public sealed class SideEqualityProperty : ITriangleProperty {
    private readonly double _epsilon;

    public string Category => "Shape";

    public SideEqualityProperty(double epsilon = 1e-9)
        => _epsilon = epsilon;

    public string? Evaluate(Triangle t) {
        bool ab = AreEqual(t.A, t.B);
        bool bc = AreEqual(t.B, t.C);

        return (ab, bc) switch {
            (true, true) => "Equilateral",
            (true, false) => "Isosceles",
            (false, true) => "Isosceles",
            (false, false) when AreEqual(t.A, t.C) => "Isosceles",
            _ => "Scalene"
        };
    }

    private bool AreEqual(double x, double y) => Math.Abs(x - y) < _epsilon;
}
