namespace DreamBig.Faist.Domain.Entities;

public sealed class Task : BaseEntity
{
    public Guid UserId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid? ParentTaskId { get; set; }
    public Task? ParentTask { get; set; }
    public ICollection<Task> SubTasks { get; set; } = [];
}
