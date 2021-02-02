using System.ComponentModel.DataAnnotations;

namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Address
    /// </summary>
    public class Address : Base
    {
        [MaxLength(100)]
        public string Line1 { get; set; }

        [MaxLength(100)]
        public string Line2 { get; set; }

        [MaxLength(100)]
        public string Line3 { get; set; }

        [MaxLength(100)]
        public string Town { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [DataType(DataType.PostalCode)]
        public string PostCode { get; set; }
    }
}