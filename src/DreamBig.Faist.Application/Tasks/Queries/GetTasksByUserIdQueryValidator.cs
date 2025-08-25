using FluentValidation;

namespace DreamBig.Faist.Application.Tasks.Queries;

public sealed class GetTasksByUserIdQueryValidator : AbstractValidator<GetTasksByUserIdQuery>
{
    public GetTasksByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
