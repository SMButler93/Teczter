using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Teczter.WebApi.RequestValidations.ValidationAttributes;

public class RequestValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var param in context.ActionDescriptor.Parameters)
        {
            if (param.BindingInfo?.BindingSource != BindingSource.Body) continue;
            
            var arg = context.ActionArguments[param.Name];
            
            if (arg is null) continue;
            
            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());

            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator) 
                continue;

            var validationContext = new ValidationContext<object>(arg);
            var result = await validator.ValidateAsync(validationContext);

            if (result.IsValid) continue;
            
            context.Result = new BadRequestObjectResult(new
            {
                Errors = result.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                    e.ErrorCode
                })
            });
            return;
        }
        await next();
    }
}