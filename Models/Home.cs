using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoilerMonitoringAPI.Models
{
    public class Home
    {
        [Key]
        public Guid HomeID { get; set; }
        public string HomeName { get; set; }
        public string Address { get; set; }
        
        public ICollection<Boilers> Boilers { get; set; } = new List<Boilers>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
