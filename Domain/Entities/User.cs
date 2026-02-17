namespace Domain.Entities;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string FullName { get; set; }
    
    public int? FamilyGroupId { get; set; }
    public FamilyGroup? FamilyGroup { get; set; }
    
    public int? FamilyGroupOwnerId { get; set; }
    public FamilyGroup? FamilyGroupOwner { get; set; }
}