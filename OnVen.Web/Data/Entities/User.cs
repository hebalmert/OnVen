using Microsoft.AspNetCore.Identity;
using OnVen.Common.Enum;
using OnVen.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnVen.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        [Required]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The Maxlength is {0}")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The Maxlength is {0}")]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The Maxlength is {0}")]
        public string Address { get; set; }

        [Display(Name = "Image")]
        public String ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == string.Empty
            ? $"http://onsale.linkonext.com/Images/noimage.png"
            : $"http://onsale.linkonext.com/User/{ImageId}";

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        [Display(Name = "User")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "User")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
    }
}
