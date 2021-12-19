using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnVen.Common.Entities
{
    public class City
    {
        public int CityId { get; set; }

        [MaxLength(50, ErrorMessage = "The Maxlength is {0}")]
        [Required(ErrorMessage = "The {0} field is Required")]
        [Display(Name = "City")]
        public string Name { get; set; }

        [JsonIgnore]
        public int DepartmentId { get; set; }

        [JsonIgnore]
        public Department Department { get; set; }
    }
}
