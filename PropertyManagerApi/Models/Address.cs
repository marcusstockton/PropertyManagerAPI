namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Address 
    /// </summary>
    public class Address : Base
    {
        /// <summary>
        /// Address Line 1
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// Address Line 2
        /// </summary>
        public string Line2 { get; set; }

        /// <summary>
        /// Address Line 3
        /// </summary>
        public string Line3 { get; set; }

        /// <summary>
        /// Address Town
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// Address City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Address Postcode
        /// </summary>
        public string PostCode { get; set; }
    }
}