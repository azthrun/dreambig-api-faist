using System.Reflection;
using System.Text.Json;
using DreamBig.Faist.Application.Common.Behaviors;
using DreamBig.Faist.Application.Common.Exceptions;
using DreamBig.Faist.Application.Common.Interfaces;
using DreamBig.Faist.Application.Common.Models;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DreamBig.Faist.UnitTests.Application.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly ValidationBehavior<IValidate, object> _behavior;

    public ValidationBehaviorTests()
    {
        _behavior = new ValidationBehavior<IValidate, object>();
    }

    [Fact(DisplayName = "Handle should not throw an exception when the message is valid")]
    public async Task Handle_Should_Not_Throw_Exception_When_Message_Is_Valid()
    {
        // Arrange
        IValidate message = Substitute.For<IValidate>();
        message.IsValid(out ValidationError? error).Returns(true);

        MethodInfo? method = _behavior.GetType().GetMethod("Handle", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        Exception exception = await Record.ExceptionAsync(() => ((ValueTask)method.Invoke(_behavior, [message, CancellationToken.None])).AsTask());

        // Assert
        exception.ShouldBeNull();
    }

    [Fact(DisplayName = "Handle should throw a FatalException when the message is invalid")]
    public async Task Handle_Should_Throw_FatalException_When_Message_Is_Invalid()
    {
        // Arrange
        IValidate message = Substitute.For<IValidate>();
        ValidationError validationError = new(["Test error message"]);
        message.IsValid(out Arg.Any<ValidationError?>()).Returns(x =>
        {
            x[0] = validationError;
            return false;
        });

        MethodInfo? method = _behavior.GetType().GetMethod("Handle", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act & Assert
        TargetInvocationException ex = await Should.ThrowAsync<TargetInvocationException>(() => ((ValueTask)method.Invoke(_behavior, [message, CancellationToken.None])).AsTask());
        FatalException fatalException = ex.InnerException.ShouldBeOfType<FatalException>();
        fatalException.Message.ShouldBe(JsonSerializer.Serialize(validationError));
    }
}