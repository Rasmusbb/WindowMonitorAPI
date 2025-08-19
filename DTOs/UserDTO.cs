using WindowMonitorAPI.Models;

namespace WindowMonitorAPI.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class UserDTOID : UserDTO
    {
        public Guid UserID { get; set; }
    }
}
