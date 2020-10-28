﻿namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Address 
    /// </summary>
    public class Address : Base
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Town { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }
    }
}