namespace Domain.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public BaseEntity()
    {
        DateTime now = DateTime.Now;
        Created = now;
        Updated = now;
    }
}