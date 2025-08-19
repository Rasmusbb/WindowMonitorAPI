
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindowMonitorAPI.Models
{
    public class Home
    {
        [Key]
        public Guid HomeID { get; set; }
        public string HomeName { get; set; }
        public string Address { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
