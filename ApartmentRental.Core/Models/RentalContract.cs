using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Models
{
    public class RentalContract : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte AnnualIncreasePercentage { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
