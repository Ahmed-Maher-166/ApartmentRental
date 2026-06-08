using SmartRental.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Models
{
    public class Tenant : BaseEntity
    {

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
        public int UniversityId { get; set; }
        public University University { get; set; }
    }
}