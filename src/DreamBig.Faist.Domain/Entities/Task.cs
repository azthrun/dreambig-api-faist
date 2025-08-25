namespace DreamBig.Faist.Domain.Entities;

public class Task : BaseEntity
{
    public Guid UserId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
}
