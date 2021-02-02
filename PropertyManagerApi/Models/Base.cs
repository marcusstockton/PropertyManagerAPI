using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagerApi.Models
{
    public abstract class Base
    {
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDateTime { get; set; }

        public bool IsActive { get; set; }
    }
}