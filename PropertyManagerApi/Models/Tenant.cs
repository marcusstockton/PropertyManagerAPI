using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(10)]
        public string Title { get; set; }

        /// <summary>
        /// Tenants first name
        /// </summary>
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Tenants last name
        /// </summary>
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Tenant contact number
        /// </summary>
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        /// <summary>
        /// Tenant email address
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Date tenancy started
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime TenancyStartDate { get; set; }

        /// <summary>
        /// Date tenancy ended
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? TenancyEndDate { get; set; }

        /// <summary>
        /// Tenants profession
        /// </summary>
        [MaxLength(100)]
        public string Profession { get; set; }

        /// <summary>
        /// The URL for the tenants profile image
        /// </summary>
        [DataType(DataType.ImageUrl)]
        public string Profile_Url { get; set; }

        /// <summary>
        /// List of notes for the tenant
        /// </summary>
        public List<Note> Notes { get; set; }

        public virtual Property Property { get; set; }
    }
}