namespace TriangleClassifier;

public sealed class Triangle {
    public double A { get; }
    public double B { get; }
    public double C { get; }

    private Triangle(double a, double b, double c) {
        A = a;
        B = b;
        C = c;
    }

    public static Result<Triangle> Create(double a, double b, double c) {
        if (a <= 0 || b <= 0 || c <= 0)
            return Result<Triangle>.Fail("All side lengths must be positive numbers.");

        // Triangle inequality: each side must be shorter than the sum of the other two.
        if (a >= b + c || b >= a + c || c >= a + b)
            return Result<Triangle>.Fail(
                $"Sides {a}, {b}, {c} do not satisfy the triangle inequality " +
                $"and cannot form a valid triangle.");

        return Result<Triangle>.Ok(new Triangle(a, b, c));
    }

    public override string ToString() => $"Triangle(a={A}, b={B}, c={C})";
}
