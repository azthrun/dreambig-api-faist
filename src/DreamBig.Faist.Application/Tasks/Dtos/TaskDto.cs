namespace DreamBig.Faist.Application.Tasks.Dtos;

public class TaskDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
}
