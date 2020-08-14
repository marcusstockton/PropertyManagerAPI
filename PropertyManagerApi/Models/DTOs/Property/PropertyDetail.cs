using PropertyManagerApi.Models.DTOs.Tenant;
using System;
using System.Collections.Generic;

namespace PropertyManagerApi.Models.DTOs.Property
{
    public class PropertyDetail
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public Address Address { get; set; }
        public DateTime PurchaseDate { get; set; }        
        public double PurchasePrice { get; set; }
        public double PropertyValue { get; set; }
        public string Description { get; set; }
        public int NoOfBeds { get; set; }
        public double RentalPrice { get; set; }
        public List<Tenant_Detail> Tenants { get; set; }
    }
}
