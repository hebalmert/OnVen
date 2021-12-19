using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnVen.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnVen.Web.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }


        //Para Manejar los precios y evitar problemas 
        //del usao de Punto o Coma como decimales.
        [Display(Name = "Price")]
        [MaxLength(12)]
        [RegularExpression(@"^\d+([\.\,]?\d+)?$", ErrorMessage = "Use only numbers and . or , to put decimals")]
        [Required]
        public string PriceString { get; set; }


        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
