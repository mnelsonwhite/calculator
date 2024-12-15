using System.Reflection;
using Xunit.Sdk;

namespace Calculator.Core.Tests.Utility;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ClassDataAttribute<TClass>(params object[] args) : DataAttribute where TClass : IEnumerable<object[]>
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var @class = typeof(TClass);
        if (Activator.CreateInstance(@class, args: args) is not IEnumerable<object[]> data)
        {
            throw new ArgumentException(
                $"{@class.FullName} must implement IEnumerable<object[]> to be used as ClassData for the test method named '{testMethod.Name}' on {testMethod.DeclaringType?.FullName}"
            );
        }
        
        return data;
    }
}