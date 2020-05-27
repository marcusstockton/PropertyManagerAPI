using System.Collections.Generic;

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
        public string Name { get; set; }

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
