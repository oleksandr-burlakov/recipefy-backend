namespace Recepify.DLL.Entities;

public class Entity
{
    public Guid Id { get; set; } 
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public DateTime? UpdatedTime { get; set; }
}