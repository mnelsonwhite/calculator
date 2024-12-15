namespace Calculator.Core.Tests;

public class SyntaxTreeTests
{
    private readonly SyntaxTree _syntaxTree;

    public SyntaxTreeTests()
    {
        _syntaxTree = new SyntaxTree();
    }

    [Fact]
    public void WhenAddingFirstLiteralToTree_ShouldOnLeftOfRoot()
    {
        // arrange
        var token1 = new LiteralToken(1);
        
        // act
        _syntaxTree.Add(token1);
        
        // assert
        Assert.IsType<RootToken>(_syntaxTree.RootNode.Token);
        Assert.Equal(token1, _syntaxTree.RootNode.Left?.Token);
    }
    
    [Fact]
    public void WhenAddingFirstOperatorToTree_ShouldReplaceRootUnknownOperator()
    {
        // arrange
        var token1 = new LiteralToken(1);
        _syntaxTree.Add(token1);
        var tokenAdd = new OperatorToken(OperatorType.Addition);
        
        // act
        _syntaxTree.Add(tokenAdd);
        
        // assert
        Assert.Equal(tokenAdd, _syntaxTree.RootNode.Token);
        Assert.Equal(token1, _syntaxTree.RootNode.Left?.Token);
    }
    
    [Fact]
    public void Test3()
    {
        // arrange
        var token1 = new LiteralToken(1);
        _syntaxTree.Add(token1);
        var tokenAdd = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd);
        var token2 = new LiteralToken(2);
        
        // act
        _syntaxTree.Add(token2);
        
        // assert
        Assert.Equal(tokenAdd, _syntaxTree.RootNode.Token);
        Assert.Equal(token1, _syntaxTree.RootNode.Left?.Token);
        Assert.Equal(token2, _syntaxTree.RootNode.Right?.Token);
    }
    
    [Fact]
    public void Test4()
    {
        // arrange
        var token1 = new LiteralToken(1);
        _syntaxTree.Add(token1);
        var tokenAdd = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd);
        var token2 = new LiteralToken(2);
        _syntaxTree.Add(token2);
        var tokenMultiply = new OperatorToken(OperatorType.Multiplication);
        
        // act
        _syntaxTree.Add(tokenMultiply);
        
        // assert
        Assert.Equal(tokenAdd, _syntaxTree.RootNode.Token);
        Assert.Equal(tokenMultiply, _syntaxTree.RootNode.Right?.Token);
        Assert.Equal(token1, _syntaxTree.RootNode.Left?.Token);
        Assert.Equal(token2, _syntaxTree.RootNode.Right?.Left?.Token);
    }
    
    [Fact]
    public void Test5()
    {
        // arrange
        var token1 = new LiteralToken(1);
        _syntaxTree.Add(token1);
        var tokenAdd = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd);
        var token2 = new LiteralToken(2);
        _syntaxTree.Add(token2);
        var tokenMultiply = new OperatorToken(OperatorType.Multiplication);
        _syntaxTree.Add(tokenMultiply);
        var token3 = new LiteralToken(3);
        
        // act
        _syntaxTree.Add(token3);
        
        // assert
        Assert.Equal(tokenAdd, _syntaxTree.RootNode.Token);
        Assert.Equal(tokenMultiply, _syntaxTree.RootNode.Right?.Token);
        Assert.Equal(token1, _syntaxTree.RootNode.Left?.Token);
        Assert.Equal(token2, _syntaxTree.RootNode.Right?.Left?.Token);
        Assert.Equal(token3, _syntaxTree.RootNode.Right?.Right?.Token);
    }
    
    [Fact]
    public void Test6()
    {
        // arrange
        var token1 = new LiteralToken(1);
        _syntaxTree.Add(token1);
        var tokenAdd1 = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd1);
        var token2 = new LiteralToken(2);
        _syntaxTree.Add(token2);
        var tokenMultiply = new OperatorToken(OperatorType.Multiplication);
        _syntaxTree.Add(tokenMultiply);
        var token3 = new LiteralToken(3);
        _syntaxTree.Add(token3);
        var tokenAdd2 = new OperatorToken(OperatorType.Addition);
        
        // act
        _syntaxTree.Add(tokenAdd2);
        
        // assert
        Assert.StrictEqual(tokenAdd2, _syntaxTree.RootNode.Token);
        Assert.StrictEqual(tokenAdd1, _syntaxTree.RootNode.Left?.Token);
        Assert.StrictEqual(token1, _syntaxTree.RootNode.Left?.Left?.Token);
        Assert.StrictEqual(tokenMultiply, _syntaxTree.RootNode.Left?.Right?.Token);
        Assert.StrictEqual(token2, _syntaxTree.RootNode.Left?.Right?.Left?.Token);
        Assert.StrictEqual(token3, _syntaxTree.RootNode.Left?.Right?.Right?.Token);
    }
    
    [Fact]
    public void Test7()
    {
        // arrange
        var token1 = new LiteralToken(1);
        _syntaxTree.Add(token1);
        var tokenAdd1 = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd1);
        var token2 = new LiteralToken(2);
        _syntaxTree.Add(token2);
        var tokenMultiply = new OperatorToken(OperatorType.Multiplication);
        _syntaxTree.Add(tokenMultiply);
        var token3 = new LiteralToken(3);
        _syntaxTree.Add(token3);
        var tokenAdd2 = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd2);
        var token4 = new LiteralToken(4);
        
        // act
        _syntaxTree.Add(token4);
        
        // assert
        Assert.StrictEqual(tokenAdd2, _syntaxTree.RootNode.Token);
        Assert.StrictEqual(tokenAdd1, _syntaxTree.RootNode.Left?.Token);
        Assert.StrictEqual(token4, _syntaxTree.RootNode.Right?.Token);
        Assert.StrictEqual(token1, _syntaxTree.RootNode.Left?.Left?.Token);
        Assert.StrictEqual(tokenMultiply, _syntaxTree.RootNode.Left?.Right?.Token);
        Assert.StrictEqual(token2, _syntaxTree.RootNode.Left?.Right?.Left?.Token);
        Assert.StrictEqual(token3, _syntaxTree.RootNode.Left?.Right?.Right?.Token);
    }
    
    [Fact]
    public void Test8()
    {
        // arrange
        var token1 = new LiteralToken(1);
        _syntaxTree.Add(token1);
        var tokenAdd1 = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd1);
        var token2 = new LiteralToken(2);
        _syntaxTree.Add(token2);
        var tokenMultiply = new OperatorToken(OperatorType.Multiplication);
        _syntaxTree.Add(tokenMultiply);
        var token3 = new LiteralToken(3);
        _syntaxTree.Add(token3);
        var tokenAdd2 = new OperatorToken(OperatorType.Addition);
        _syntaxTree.Add(tokenAdd2);
        var token4 = new LiteralToken(4);
        _syntaxTree.Add(token4);
        var tokenDivision = new OperatorToken(OperatorType.Division);
        _syntaxTree.Add(tokenDivision);
        var token5 = new LiteralToken(5);
        
        // act
        _syntaxTree.Add(token5);
        
        // assert
        var tree = _syntaxTree.RootNode.PrintTree();
        Assert.StrictEqual(tokenAdd2, _syntaxTree.RootNode.Token);
        Assert.StrictEqual(tokenAdd1, _syntaxTree.RootNode.Left?.Token);
        Assert.StrictEqual(tokenDivision, _syntaxTree.RootNode.Right?.Token);
        Assert.StrictEqual(token4, _syntaxTree.RootNode.Right?.Left?.Token);
        Assert.StrictEqual(token5, _syntaxTree.RootNode.Right?.Right?.Token);
        Assert.StrictEqual(token1, _syntaxTree.RootNode.Left?.Left?.Token);
        Assert.StrictEqual(tokenMultiply, _syntaxTree.RootNode.Left?.Right?.Token);
        Assert.StrictEqual(token2, _syntaxTree.RootNode.Left?.Right?.Left?.Token);
        Assert.StrictEqual(token3, _syntaxTree.RootNode.Left?.Right?.Right?.Token);
    }
}