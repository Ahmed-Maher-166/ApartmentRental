using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Models
{
    public class University : BaseEntity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public ICollection<Tenant> Tenants { get; set; } = new HashSet<Tenant>();
    }
}
