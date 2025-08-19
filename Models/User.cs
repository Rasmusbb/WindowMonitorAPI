using System.ComponentModel.DataAnnotations;

namespace WindowMonitorAPI.Models
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<Home> Homes { get; set; }  
    }
}
