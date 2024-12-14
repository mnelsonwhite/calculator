using System.Text;
using Calculator.Core.Tests.Utility;
using NSubstitute;

namespace Calculator.Core.Tests;

public class CalculatorServiceTests
{
    private readonly ICalculatorOutput _output;
    private readonly ICalculatorInput _input;
    private readonly SyntaxTree _syntaxTree;

    public CalculatorServiceTests()
    {
        _output = Substitute.For<ICalculatorOutput>();
        _input = Substitute.For<ICalculatorInput>();
        _syntaxTree = new SyntaxTree();
    }

    [Fact]
    public void WhenEnteringDigits_DigitsShouldBeOutput()
    {
        // arrange
        var outputBuilder = new StringBuilder();
        _output
            .When(x => x.OnNext(Arg.Any<char>()))
            .Do(x => outputBuilder.Append(x[0]));

        IObserver<char>? inputObserver = null;
        _input.When(x => x.Subscribe(Arg.Any<IObserver<char>>()))
            .Do(x => inputObserver = x[0] as IObserver<char>);

        using var calculator = new CalculatorService(
            _input,
            _output,
            _syntaxTree
        );
        
        // act
        inputObserver!.OnNext("123.4");
        
        // assert
        Assert.Equal("123.4", outputBuilder.ToString());
    }
    
    [Theory]
    [InlineData("123+111=","123 + 111 = 234\n")]
    [InlineData("123-111=","123 - 111 = 12\n")]
    [InlineData("9/3=","9 / 3 = 3\n")]
    [InlineData("5*5=","5 x 5 = 25\n")]
    [InlineData("1+2*3+4=","1 + 2 x 3 + 4 = 11\n")]
    public void WhenSimpleOperation_DigitsShouldBeExpectedOutput(string input, string expectedOutput)
    {
        // arrange
        var outputBuilder = new StringBuilder();
        _output
            .When(x => x.OnNext(Arg.Any<char>()))
            .Do(x => outputBuilder.Append(x[0]));

        IObserver<char>? inputObserver = null;
        _input.When(x => x.Subscribe(Arg.Any<IObserver<char>>()))
            .Do(x => inputObserver = x[0] as IObserver<char>);

        using var calculator = new CalculatorService(
            _input,
            _output,
            _syntaxTree
        );
        
        // act
        inputObserver!.OnNext(input);
        
        // assert
        Assert.Equal(expectedOutput, outputBuilder.ToString());
    }
}