using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Models.DTOs.Tenant
{
    public class Tenant_DetailDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime TenancyStartDate { get; set; }
        public DateTime? TenancyEndDate { get; set; }
        public string Profession { get; set; }
        public List<Note> Notes { get; set; }
        public string Image { get; set; }

    }
}
