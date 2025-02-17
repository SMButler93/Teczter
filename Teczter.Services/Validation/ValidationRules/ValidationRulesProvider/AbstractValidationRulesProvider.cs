using System;
using System.Reflection;
using Teczter.Domain;
using Teczter.Domain.Entities;

namespace Teczter.Services.Validation.ValidationRules.ValidationRulesProvider;

public abstract class AbstractValidationRulesProvider<T>
{
    public abstract List<Func<T, CzValidationResult>> GetRules();

    protected List<Func<T, CzValidationResult>> GetRules(Type type)
    {
        var rules = new List<Func<T, CzValidationResult>>();

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.ReturnType == typeof(CzValidationResult) &&
                        m.GetParameters().Length == 1 &&
                        m.GetParameters().First().ParameterType == typeof(T));

        foreach (var method in methods)
        {
            var rule = (Func<T, CzValidationResult>)Delegate.CreateDelegate(
                typeof(Func<T, CzValidationResult>), method);

            rules.Add(rule);
        }

        return rules;
    }
}
