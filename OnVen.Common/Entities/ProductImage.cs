using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnVen.Common.Entities
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Image")]
        public string ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == string.Empty
            ? $"http://onsale.linkonext.com/Images/noimage.png"
            : $"http://onsale.linkonext.com/Products/{ImageId}";

    }
}
