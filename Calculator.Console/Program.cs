// See https://aka.ms/new-console-template for more information

using Calculator.Console;
using Calculator.Core;
using Calculator.Core.Syntax;

var consoleOutput = new ConsoleOutput();
using var consoleInput = new ConsoleInput();
using var calculatorService = new CalculatorService(
    consoleInput,
    consoleOutput,
    new SyntaxTree()
);

var (task, cancel) = consoleInput.Start();
await task;