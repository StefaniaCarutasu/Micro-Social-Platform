using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDProiect.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        public string GroupName { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}