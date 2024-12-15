using System;
using System.Text;
using Calculator.Core.Tests.Utility;
using NSubstitute;

namespace Calculator.Core.Tests;

public class CalculatorServiceTests
{
    [Fact]
    public void WhenEnteringDigits_DigitsShouldBeOutput()
    {
        // arrange
        var output = Substitute.For<ICalculatorOutput>();
        var input = Substitute.For<ICalculatorInput>();
        var syntaxTree = new SyntaxTree();
        
        var outputBuilder = new StringBuilder();
        output
            .When(x => x.OnNext(Arg.Any<char>()))
            .Do(x => outputBuilder.Append(x[0]));

        IObserver<char>? inputObserver = null;
        input.When(x => x.Subscribe(Arg.Any<IObserver<char>>()))
            .Do(x => inputObserver = x[0] as IObserver<char>);

        using var calculator = new CalculatorService(
            input,
            output,
            syntaxTree
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
    [InlineData("5*8-1=","5 x 8 - 1 = 39\n")]
    public void WhenSimpleOperation_DigitsShouldBeExpectedOutput(string tokens, string expectedOutput)
    {
        // arrange
        var output = Substitute.For<ICalculatorOutput>();
        var input = Substitute.For<ICalculatorInput>();
        var syntaxTree = new SyntaxTree();
        var outputBuilder = new StringBuilder();
        output
            .When(x => x.OnNext(Arg.Any<char>()))
            .Do(x => outputBuilder.Append(x[0]));

        IObserver<char>? inputObserver = null;
        input.When(x => x.Subscribe(Arg.Any<IObserver<char>>()))
            .Do(x => inputObserver = x[0] as IObserver<char>);

        using var calculator = new CalculatorService(
            input,
            output,
            syntaxTree
        );
        
        // act
        inputObserver!.OnNext(tokens);
        
        // assert
        Assert.Equal(expectedOutput, outputBuilder.ToString());
    }
}