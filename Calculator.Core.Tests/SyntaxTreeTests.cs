using Calculator.Core.Syntax;
using Calculator.Core.Tests.Utility;

namespace Calculator.Core.Tests;

public class SyntaxTreeTests
{
    private readonly SyntaxTree _syntaxTree = new();

    [Theory]
    [ClassData<SyntaxTreeTestsData>]
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