using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Property
    /// </summary>
    public class Property : Base
    {
        public Guid AddressId { get; set; }
        /// <summary>
        /// Property Address
        /// </summary>

        public Address Address { get; set; }

        /// <summary>
        /// Property purchase date
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Property purchase price
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Property value
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PropertyValue { get; set; }

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
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RentalPrice { get; set; }

        /// <summary>
        /// List of tenants at the property
        /// </summary>
        public List<Tenant> Tenants { get; set; }

        public Guid PortfolioId { get; set; }

        /// <summary>
        /// Link back to the portfolio
        /// </summary>
        public virtual Portfolio Portfolio { get; set; }
    }
}