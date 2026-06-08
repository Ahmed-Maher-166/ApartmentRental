using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Specification
{
    public class ApartmentSpecification 
    {
        public string? Sort { get; set; }
        public string? City { get; set; }
        public string? StreetName { get; set; }
        public int Index { get; set; } = 1;
        private byte pageSize;
        public byte PageSize
        {
            get { return pageSize; }
            set { pageSize = (byte)(value > 10 ? 10 : value); }
        }
 
    
    }
}
