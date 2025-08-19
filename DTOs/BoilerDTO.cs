using BoilerMonitoringAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoilerMonitoringAPI.DTOs
{
    public class BoilerDTO
    {
        public string BoilerName { get; set; }
        public BoilerType BoilerType { get; set; }
        public BoilerStatus BoilerStatus { get; set; }
        public double TargetTemperature { get; set; }
        public Guid HomeID { get; set; }
    }

    public class BoilerDTOID : BoilerDTO
    {
        public Guid BoilerID { get; set; }
    }
}
