using ErrorOr;
using FluentValidation;
using MediatR;

namespace GymManagement.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IValidator<TRequest>? validator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null) return await next();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid) return await next();

        var errors = validationResult.Errors.ConvertAll(x => Error.Validation(x.ErrorCode, x.ErrorMessage));

        return (dynamic)errors;
    }
}