using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagerApi.Models.DTOs.Property
{
    public class PropertyCreate
    {
        [Required, MaxLength(100)]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        [DataType(DataType.PostalCode)]
        public string Postcode { get; set; }
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
        [DataType(DataType.Currency)]
        public double PurchasePrice { get; set; }
        [DataType(DataType.Currency)]
        public double PropertyValue { get; set; }
        public string Description { get; set; }
        [Range(1, 1000)]
        public int NoOfBeds { get; set; }
        [DataType(DataType.Currency)]
        public double RentalPrice { get; set; }
    }
}
