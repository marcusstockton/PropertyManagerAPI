using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Models
{
    public abstract class Base
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
