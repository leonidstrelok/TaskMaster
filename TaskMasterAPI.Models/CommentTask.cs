namespace TaskMasterAPI.Models;

public class CommentTask : Bases.Base
{
    public string ClientId { get; set; }
    public ICollection<Task> Tasks { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}