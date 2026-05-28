namespace TriangleClassifier;

public static class InputParser {
    public static Result<(double A, double B, double C)> Parse(IReadOnlyList<string> tokens) {
        if (tokens.Count != 3)
            return Result<(double, double, double)>.Fail(
                $"Expected exactly 3 side lengths, but received {tokens.Count}.");

        var values = new double[3];
        for (int i = 0; i < 3; i++) {
            if (!double.TryParse(tokens[i], System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture,
                                 out double v) || !double.IsFinite(v)) {
                return Result<(double, double, double)>.Fail(
                    $"'{tokens[i]}' is not a valid number. Please enter finite decimal values.");
            }
            values[i] = v;
        }

        return Result<(double, double, double)>.Ok((values[0], values[1], values[2]));
    }
}
