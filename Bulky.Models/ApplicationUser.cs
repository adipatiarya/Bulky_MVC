using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bulky.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public String Name { get; set; }
        public String? StreetAddress { get; set; }
        public String? City { get; set; }
        public String? State { get; set; }
        public String? PostalCode { get; set; }

    }
}
