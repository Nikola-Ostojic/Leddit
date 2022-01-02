namespace Backend.DAL.Entities
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }

    // This should be a separate table with a table that connects the user and the role tables
    // Example:
    // User
    // Role
    // UserRole
    // This approach would give us more flexibility when working with users and authorization, also 
    // for extending the functionality, for example an admin panel. 
    // However, for simplicity sake it's going to be a hard coded enum
    public enum Role
    {
        User,
        Admin
    }
}
