namespace TriangleClassifier;

internal static class Program {
    private static readonly TriangleClassifier Classifier = BuildClassifier();

    private static TriangleClassifier BuildClassifier() {
        // Order here determines display order, nothing else depends on it.
        var properties = new List<ITriangleProperty>
        {
            new SideEqualityProperty(),
            // To add a new check: implement ITriangleProperty, then register it here.
            // new AngleTypeProperty(), // <- AngleTypeProperty.cs
        };
        return new TriangleClassifier(properties);
    }

    internal static void Main(string[] args) {
        if (args.Length > 0) {
            ProcessAndPrint(args);
            return;
        }

        RunInteractiveMode();
    }

    private static void RunInteractiveMode() {
        PrintBanner();

        while (true) {
            Console.Write("\nEnter three side lengths (or 'q' to quit): ");
            string? line = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(line) || line.Equals("q", StringComparison.OrdinalIgnoreCase)) {
                Console.WriteLine("Goodbye.");
                break;
            }

            ProcessAndPrint(line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }

    private static void ProcessAndPrint(IReadOnlyList<string> tokens) {
        // tokens -> parsed numbers → validated triangle → classification
        var result = InputParser.Parse(tokens)
            .Map(sides => Triangle.Create(sides.A, sides.B, sides.C));

        if (!result.IsSuccess) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {result.Error}");
            Console.ResetColor();
            return;
        }

        Triangle triangle = result.Value!;
        IReadOnlyList<ClassificationResult> classifications = Classifier.Classify(triangle);

        PrintResults(triangle, classifications);
    }

    private static void PrintResults(Triangle triangle, IReadOnlyList<ClassificationResult> results) {
        Console.WriteLine();
        Console.WriteLine($"  Triangle  :  {triangle}");
        Console.WriteLine(new string('─', 36));
        foreach (var r in results) {
            string label = r.Label ?? "(n/a)";
            Console.WriteLine($"  {r.Category,-14}:  {label}");
        }
        Console.WriteLine();
    }

    private static void PrintBanner() {
        Console.WriteLine("┌─────────────────────────────────────┐");
        Console.WriteLine("│       Triangle Classifier  v1.0     │");
        Console.WriteLine("└─────────────────────────────────────┘");
    }
}
