namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public int? FamilyGroupId { get; set; }
    public FamilyGroup? FamilyGroup { get; set; }
    public int? FamilyGroupOwnerId { get; set; }
    public FamilyGroup? FamilyGroupOwner { get; set; }
    public bool IsActive { get; set; }
}