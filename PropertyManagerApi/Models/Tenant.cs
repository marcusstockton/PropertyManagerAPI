using System;
using System.Collections.Generic;

namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Tenant Class
    /// </summary>
    public class Tenant : Base
    {
        /// <summary>
        /// Tenant's title - Mr, Mrs etc
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tenants first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Tenants last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Tenant contact number
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Tenant email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Date tenancy started
        /// </summary>
        public DateTime TenancyStartDate { get; set; }
        
        /// <summary>
        /// Date tenancy ended
        /// </summary>
        public DateTime? TenancyEndDate { get; set; }

        /// <summary>
        /// Tenants profession
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// List of notes for the tenant
        /// </summary>
        public List<Note> Notes { get; set; }
    }
}