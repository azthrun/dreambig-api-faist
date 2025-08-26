namespace DreamBig.Faist.Application.Common.Models;

public sealed record ValidationError(IEnumerable<string> Errors);
