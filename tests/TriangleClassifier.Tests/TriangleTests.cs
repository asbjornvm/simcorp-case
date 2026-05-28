using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace TriangleClassifier.Tests;

public class TriangleCreateTests {
    [Test]
    public async Task RejectsZeroSide() {
        var result = Triangle.Create(0, 4, 5);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task RejectsNegativeSide() {
        var result = Triangle.Create(-1, 4, 5);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    [Arguments(1, 2, 10)]
    [Arguments(1, 2, 3)]   // degenerate: equality not strictly less
    public async Task RejectsTriangleInequalityViolation(double a, double b, double c) {
        var result = Triangle.Create(a, b, c);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task AcceptsValidTriangle() {
        var result = Triangle.Create(3, 4, 5);
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task ErrorMessageMentionsAllThreeSides() {
        var result = Triangle.Create(1, 2, 10);
        await Assert.That(result.Error).Contains("1");
        await Assert.That(result.Error).Contains("2");
        await Assert.That(result.Error).Contains("10");
    }
}

public class SideEqualityPropertyTests {
    private readonly SideEqualityProperty _property = new();

    [Test]
    public async Task Equilateral()
        => await AssertShape(5, 5, 5, "Equilateral");

    [Test]
    [Arguments(5, 5, 3, "Isosceles")]  // AB equal
    [Arguments(3, 5, 5, "Isosceles")]  // BC equal
    [Arguments(5, 3, 5, "Isosceles")]  // AC equal
    public async Task Isosceles(double a, double b, double c, string expected)
        => await AssertShape(a, b, c, expected);

    [Test]
    public async Task Scalene()
        => await AssertShape(3, 4, 5, "Scalene");

    private async Task AssertShape(double a, double b, double c, string expected) {
        var triangle = Triangle.Create(a, b, c).Value!;
        await Assert.That(_property.Evaluate(triangle)).IsEqualTo(expected);
    }
}

public class AngleTypePropertyTests {
    private readonly AngleTypeProperty _property = new();

    [Test]
    public async Task Right_3_4_5()
        => await AssertAngleType(3, 4, 5, "Right");

    [Test]
    public async Task Acute_Equilateral()
        => await AssertAngleType(5, 5, 5, "Acute");

    [Test]
    public async Task Obtuse_2_3_4()
        => await AssertAngleType(2, 3, 4, "Obtuse");

    private async Task AssertAngleType(double a, double b, double c, string expected) {
        var triangle = Triangle.Create(a, b, c).Value!;
        await Assert.That(_property.Evaluate(triangle)).IsEqualTo(expected);
    }
}

public class InputParserTests {
    [Test]
    public async Task RejectsNonNumericToken() {
        var result = InputParser.Parse(["3", "abc", "5"]);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task RejectsTooFewTokens() {
        var result = InputParser.Parse(["3", "4"]);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task RejectsTooManyTokens() {
        var result = InputParser.Parse(["3", "4", "5", "6"]);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task RejectsInfinityInput() {
        var result = InputParser.Parse(["3", "Infinity", "5"]);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task RejectsNaNInput() {
        var result = InputParser.Parse(["3", "NaN", "5"]);
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task AcceptsValidTokens() {
        var result = InputParser.Parse(["3", "4", "5"]);
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task AcceptsDecimalInput() {
        var result = InputParser.Parse(["3.5", "4.5", "5.5"]);
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task ErrorMessageIncludesOffendingToken() {
        var result = InputParser.Parse(["3", "abc", "5"]);
        await Assert.That(result.Error).Contains("abc");
    }
}
