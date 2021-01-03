using BDProiect.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MicroSocialPlatform.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(100, ErrorMessage = "Descrierea profilului nu poate avea mai mult de 100 de caractere")]
        public string ProfileDescription { get; set; }


        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //public virtual ICollection<Post> Posts { get; set; }
    }
}