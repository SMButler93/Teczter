using System;
using System.Reflection;
using Teczter.Domain;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation.ValidationRules.ValidationRulesProvider;

public abstract class AbstractValidationRulesProvider<T>
{
    public abstract List<Func<T, TeczterValidationResult>> GetRules();

    protected List<Func<T, TeczterValidationResult>> GetRules(Type type)
    {
        var rules = new List<Func<T, TeczterValidationResult>>();

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.ReturnType == typeof(TeczterValidationResult) &&
                        m.GetParameters().Length == 1 &&
                        m.GetParameters().First().ParameterType == typeof(T));

        foreach (var method in methods)
        {
            var rule = (Func<T, TeczterValidationResult>)Delegate.CreateDelegate(
                typeof(Func<T, TeczterValidationResult>), method);

            rules.Add(rule);
        }

        return rules;
    }
}
