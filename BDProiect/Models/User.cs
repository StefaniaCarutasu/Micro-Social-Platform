using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace BDProiect.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [MinLength(10)] [MaxLength(10)] [Required] 
        public string PhoneNumber { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}