using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Portfolio class
    /// </summary>
    public class Portfolio : Base
    {
        /// <summary>
        /// Portfolio Name
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        /// <summary>
        /// Portfolio Owner
        /// </summary>
        public ApplicationUser Owner { get; set; }

        /// <summary>
        /// List of properties against the Portfolio
        /// </summary>
        public List<Property> Properties { get; set; }
    }
}