using System.Text.Json;
using DreamBig.Faist.Application.Common.Exceptions;
using DreamBig.Faist.Application.Common.Interfaces;
using Mediator;

namespace DreamBig.Faist.Application.Common.Behaviors;

internal sealed class ValidationBehavior<TMessage, TResponse> : MessagePreProcessor<TMessage, TResponse> where TMessage : IValidate
{
    protected override ValueTask Handle(TMessage message, CancellationToken cancellationToken)
    {
        if (!message.IsValid(out Models.ValidationError? error))
        {
            throw new FatalException(JsonSerializer.Serialize(error));
        }
        return default;
    }
}
