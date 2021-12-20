using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnVen.Common.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        [MaxLength(50, ErrorMessage = "The Maxlength is {0}")]
        [Required(ErrorMessage = "The {0} field is Required")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == string.Empty
            ? $"http://onven.linkonext.com/Images/noimage.png"
            : $"http://onven.linkonext.com/Categories/{ImageId}";

        public ICollection<Product> Products { get; set; }
    }
}
