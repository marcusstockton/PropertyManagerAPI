using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Property
    /// </summary>
    public class Property : Base
    {
        /// <summary>
        /// Property Address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Property purchase date
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Property purchase price
        /// </summary>
        public double PurchasePrice { get; set; }
        
        /// <summary>
        /// Property value
        /// </summary>
        public double PropertyValue { get; set; }

        /// <summary>
        /// Description of property
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Number Of Bedrooms in the property
        /// </summary>
        public int NoOfBeds { get; set; }

        /// <summary>
        /// Property rental price
        /// </summary>
        public double RentalPrice { get; set; }

        /// <summary>
        /// List of tenants at the property
        /// </summary>
        public List<Tenant> Tenants { get; set; }
    }
}
