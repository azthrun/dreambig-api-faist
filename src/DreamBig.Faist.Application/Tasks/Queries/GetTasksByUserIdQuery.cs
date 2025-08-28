using System.Diagnostics.CodeAnalysis;
using DreamBig.Faist.Application.Common.Interfaces;
using DreamBig.Faist.Application.Common.Models;
using DreamBig.Faist.Application.Tasks.Dtos;
using FluentValidation;
using Mediator;

namespace DreamBig.Faist.Application.Tasks.Queries;

public sealed record GetTasksByUserIdQuery(Guid UserId) : IQuery<IEnumerable<TaskDto>>, IValidate
{
    public bool IsValid([NotNullWhen(false)] out ValidationError? errorMessage)
    {
        GetTasksByUserIdQueryValidator validator = new();
        FluentValidation.Results.ValidationResult results = validator.Validate(this);
        errorMessage = results.IsValid
            ? null
            : new ValidationError(results.Errors.Select(e => e.ErrorMessage));
        return results.IsValid;
    }
}

public sealed class GetTasksByUserIdQueryValidator : AbstractValidator<GetTasksByUserIdQuery>
{
    public GetTasksByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .Must(id => id != Guid.Empty);
    }
}
