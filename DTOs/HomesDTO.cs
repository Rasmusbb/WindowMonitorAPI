using BoilerMonitoringAPI.Models;

namespace BoilerMonitoringAPI.DTOs
{
    public class HomesDTO
    {
        public string HomeName { get; set; }
        public string Address { get; set; }
        public Guid UserID { get; set; }
    }
    public class HomesDTOID : HomesDTO
    {
        public Guid HomeID { get; set; }
    }   
}
