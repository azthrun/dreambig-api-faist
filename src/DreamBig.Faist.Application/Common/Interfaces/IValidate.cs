using System.Diagnostics.CodeAnalysis;
using DreamBig.Faist.Application.Common.Models;
using Mediator;

namespace DreamBig.Faist.Application.Common.Interfaces;

public interface IValidate : IMessage
{
    bool IsValid([NotNullWhen(false)] out ValidationError? errorMessage);
}