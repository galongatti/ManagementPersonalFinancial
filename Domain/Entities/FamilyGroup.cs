namespace Domain.Entities;

public class FamilyGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public ICollection<User> Members { get; set; }
    
    public User Owner { get; set; }
}