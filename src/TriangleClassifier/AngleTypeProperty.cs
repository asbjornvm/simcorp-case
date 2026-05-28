namespace TriangleClassifier;

// Converse of the Pythagorean theorem: compare c^2 with a^2+b^2 where c is the longest side.
public sealed class AngleTypeProperty : ITriangleProperty {
    private readonly double _epsilon;

    public string Category => "Angle type";

    public AngleTypeProperty(double epsilon = 1e-9)
        => _epsilon = epsilon;

    public string? Evaluate(Triangle t) {
        double c = Math.Max(t.A, Math.Max(t.B, t.C));
        double c2 = c * c;
        double ab2 = t.A * t.A + t.B * t.B + t.C * t.C - c2;

        if (Math.Abs(c2 - ab2) < _epsilon) return "Right";
        return c2 < ab2 ? "Acute" : "Obtuse";
    }
}
