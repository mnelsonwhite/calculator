using System.Collections;
using Calculator.Core.Tests.Utility;

namespace Calculator.Core.Tests;

public class SyntaxTreeTests
{
    private readonly SyntaxTree _syntaxTree = new();

    [Theory]
    [ClassData<SyntaxTreeTestData>]
    public void WhenBuildingASynctaxTree_ShouldBeExpectedStructure(IEnumerable<ICalculatorToken> tokens, SyntaxTreeNode expectedTree)
    {
        // arrange
        var tokensList = tokens.ToList();
        if (tokensList.Count > 1)
        {
            foreach (var token in tokensList[..^1])
            {
                _syntaxTree.Add(token);
            }
        }
        
        // act
        _syntaxTree.Add(tokensList[^1]);
        
        // assert
        Assert.Equivalent(expectedTree, _syntaxTree.RootNode);
    }
}

class SyntaxTreeTestData : IEnumerable<object[]>
{
    private readonly int _totalCases;

    public SyntaxTreeTestData() : this(_tokens.Length)
    {
    }
    
    public SyntaxTreeTestData(int totalCases)
    {
        _totalCases = totalCases;
    }
    
    // 1+2*3+4/5
    private static readonly ICalculatorToken[] _tokens = [
        new LiteralToken(1),
        new OperatorToken(OperatorType.Addition),
        new LiteralToken(2),
        new OperatorToken(OperatorType.Multiplication),
        new LiteralToken(3),
        new OperatorToken(OperatorType.Addition),
        new LiteralToken(4),
        new OperatorToken(OperatorType.Division),
        new LiteralToken(5)
    ];

    private static readonly SyntaxTreeNode[] _expectedTrees =
    [
        new (
            new RootToken(),
            left: new (new LiteralToken(1))
        ),
        new (
            new OperatorToken(OperatorType.Addition),
            left: new (new LiteralToken(1))
        ),
        new (
            new OperatorToken(OperatorType.Addition),
            left: new (new LiteralToken(1)),
            right: new (new LiteralToken(2))
        ),
        new SyntaxTreeNode(
            new OperatorToken(OperatorType.Addition),
            left: new (new LiteralToken(1)),
            right: new (
                new OperatorToken(OperatorType.Multiplication),
                left: new (new LiteralToken(2))
            )
        ),
        new(
            new OperatorToken(OperatorType.Addition),
            left: new (new LiteralToken(1)),
            right: new (
                new OperatorToken(OperatorType.Multiplication),
                left: new (new LiteralToken(2)),
                right: new (new LiteralToken(3))
            )
        ),
        new(
            new OperatorToken(OperatorType.Addition),
            left: new (
                new OperatorToken(OperatorType.Addition),
                new(new LiteralToken(1)),
                new (
                    new OperatorToken(OperatorType.Multiplication),
                    left: new (new LiteralToken(2)),
                    right: new(new LiteralToken(3))
                )
            )
        ),
        new(
            new OperatorToken(OperatorType.Addition),
            left: new (
                new OperatorToken(OperatorType.Addition),
                new(new LiteralToken(1)),
                new (
                    new OperatorToken(OperatorType.Multiplication),
                    left: new (new LiteralToken(2)),
                    right: new(new LiteralToken(3))
                )
            ),
            right: new(new LiteralToken(4))
        ),
        new (
            new OperatorToken(OperatorType.Addition),
            left: new (
                new OperatorToken(OperatorType.Addition),
                new(new LiteralToken(1)),
                new (
                    new OperatorToken(OperatorType.Multiplication),
                    left: new (new LiteralToken(2)),
                    right: new(new LiteralToken(3))
                )
            ),
            new(
                new OperatorToken(OperatorType.Division),
                left: new(new LiteralToken(4))
            )
        ),
        new (
            new OperatorToken(OperatorType.Addition),
            left: new (
                new OperatorToken(OperatorType.Addition),
                new(new LiteralToken(1)),
                new (
                    new OperatorToken(OperatorType.Multiplication),
                    left: new (new LiteralToken(2)),
                    right: new(new LiteralToken(3))
                )
            ),
            new(
                new OperatorToken(OperatorType.Division),
                left: new(new LiteralToken(4)),
                right: new(new LiteralToken(5))
            )
        )
        
    ];
    
    public IEnumerator<object[]> GetEnumerator() => Enumerable
        .Range(1, _expectedTrees.Length)
        .Select(
            i => new[] { (object) _tokens.Take(i), _expectedTrees[i - 1] }
        )
        .Take(_totalCases)
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}