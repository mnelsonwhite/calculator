using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Core;

public class SyntaxTree : ISyntaxTree
{
    public SyntaxTreeNode? _lastOperator;
    private SyntaxTreeNode _rootNode = new(new RootToken());
    public SyntaxTreeNode RootNode => _rootNode;
    public Stack<(SyntaxTreeNode Root, SyntaxTreeNode? LastOperator)> _treeStack = new();
    

    public void Add(LiteralToken token) => ApplyNode(new SyntaxTreeNode(token));
    public void Add(OperatorToken token) => ApplyNode(new SyntaxTreeNode(token));
    public void Add(OpenParenthesisToken token)
    {
        _treeStack.Push((_rootNode, _lastOperator));
        _lastOperator = null;
        _rootNode = new(new RootToken());
    }

    public bool Add(CloseParenthesisToken token)
    {
        if (!_treeStack.Any())
        {
            return false;
        }
        
        var node = _rootNode;
        (_rootNode, _lastOperator) = _treeStack.Pop();
        ApplyNode(node);
        return true;
    }

    public SyntaxTreeNode? Complete()
    {
        if (_treeStack.Any())
        {
            return null;
        }

        return _rootNode;
    }

    public void Reset()
    {
        _treeStack.Clear();
        _rootNode = new SyntaxTreeNode(new RootToken());
        _lastOperator = null;
    }

    private void ApplyNode(SyntaxTreeNode node)
    {
        if (HandleRootToken(node))
        {
            HandleFinalOperations(node);
            return;
        }
        
        if (HandleLastOperator(node))
        {
            HandleFinalOperations(node);
            return;
        }

        if (HandleNewOperator(node))
        {
            HandleFinalOperations(node);
            return;
        }
    }

    private bool HandleNewOperator(SyntaxTreeNode node)
    {
        if (node.Token is not OperatorToken opNode)
        {
            return false;
        }

        if (_lastOperator?.Token is not OperatorToken lastOpNode)
        {
            _lastOperator = node;
            return true;
        }

        if (GetOperatorPrecedence(lastOpNode) < GetOperatorPrecedence(opNode))
        {
            HandleHigherOperatorInsert(
                _lastOperator?.Right
                ?? throw new Exception("Invalid state"),
                node
            );

            return true;
        }
        if (GetOperatorPrecedence(lastOpNode) >= GetOperatorPrecedence(opNode))
        {
            HandleLowerOperatorInsert(
                _lastOperator,
                node
            );

            return true;
        }
        
        return false;
    }
    
    private void HandleLowerOperatorInsert(SyntaxTreeNode targetNode, SyntaxTreeNode node)
    {
        var opToken = (OperatorToken)node.Token;

        if (targetNode == _rootNode)
        {
            ReplaceNode(node, targetNode);
            node.Left = targetNode;
        }
        else if (targetNode.Token is OperatorToken targetOpToken
            && GetOperatorPrecedence(targetOpToken) > GetOperatorPrecedence(opToken))
        {
            HandleLowerOperatorInsert(
                FindParentNode(targetNode) ?? _rootNode,
                node
            );
        }
        else
        {
            ReplaceNode(node, targetNode);
            node.Left = targetNode;
        }
    }

    private void HandleHigherOperatorInsert(SyntaxTreeNode targetNode, SyntaxTreeNode node)
    {
        var opToken = (OperatorToken)node.Token;
        if (targetNode.Token is OperatorToken targetOpToken
            && GetOperatorPrecedence(targetOpToken) < GetOperatorPrecedence(opToken))
        {
            HandleHigherOperatorInsert(
                targetNode.Left ?? throw new Exception("Invalid state"),
                node
            );
        }
        else if (targetNode.Token is LiteralToken)
        {
            ReplaceNode(node, targetNode);
            node.Left = targetNode;
        }
    }

    private void ReplaceNode(SyntaxTreeNode replacement, SyntaxTreeNode target)
    {
        var parentNode = FindParentNode(_rootNode, target);

        if (parentNode is null)
        {
            _rootNode = replacement;
        }
        else if (ReferenceEquals(parentNode.Left, target))
        {
            parentNode.Left = replacement;
        }
        else if (ReferenceEquals(parentNode.Right, target))
        {
            parentNode.Right = replacement;
        }
    }

    private SyntaxTreeNode? FindParentNode(SyntaxTreeNode node)
    {
        return FindParentNode(_rootNode, node);
    }
    
    private SyntaxTreeNode? FindParentNode(SyntaxTreeNode current, SyntaxTreeNode target)
    {
        if (ReferenceEquals(current.Left, target) || ReferenceEquals(current.Right, target))
            return current;
        
        if (current.Left is not null)
        {
            var leftResult = FindParentNode(current.Left, target);
            if (leftResult is not null)
                return leftResult;
        }
    
        if (current.Right is not null)
        {
            var rightResult = FindParentNode(current.Right, target);
            if (rightResult is not null)
                return rightResult;
        }
    
        return null;
    }

    private void HandleFinalOperations(SyntaxTreeNode node)
    {
        if (node.Token is OperatorToken)
        {
            _lastOperator = node;
        }
    }

    private bool HandleLastOperator(SyntaxTreeNode node)
    {
        if (_lastOperator is null)
        {
            return true;
        }

        if (_lastOperator.Left is null)
        {
            _lastOperator.Left = node;
            return true;
        }

        if (_lastOperator.Right is null)
        {
            _lastOperator.Right = node;
            return true;
        }

        return false;
    }

    private bool HandleRootToken(SyntaxTreeNode node)
    {
        if (node.Token is RootToken)
        {
            throw new Exception("RootToken cannot be applied to AST");
        }
        
        if (_rootNode.Token is RootToken)
        {
            // if the root node is a root token and the node to apply
            // is a literal token, this is probably the first token
            if (node.Token is LiteralToken)
            {
                if (_rootNode.Left is null)
                {
                    _rootNode.Left = node;
                    return true;
                }
                throw new Exception("Invalid state");
            }
            
            if (node.Token is OperatorToken)
            {
                if (_rootNode.Right is not null)
                {
                    throw new Exception("Invalid State");
                }
                
                if (_rootNode.Left is not null)
                {
                    node.Left = _rootNode.Left;
                    _rootNode.RemoveChildren();
                    _rootNode = node;
                    return true;
                }
            }
        }

        return false;
    }
    
    private int GetOperatorPrecedence(OperatorToken token) => token.OperationType switch
    {
        OperatorType.Undefined => 0,
        OperatorType.Addition => 1,
        OperatorType.Subtraction => 1,
        OperatorType.Multiplication => 2,
        OperatorType.Division => 2,
        OperatorType.Exponent => 3,
        OperatorType.Equals => throw new Exception("Operator does not have precedence"),
        _ => throw new ArgumentOutOfRangeException()
    };
}