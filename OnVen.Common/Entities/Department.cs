
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnVen.Common.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [MaxLength(50, ErrorMessage = "The Maxlength is {0}")]
        [Required(ErrorMessage = "The {0} field is Required")]
        [Display(Name = "Department")]
        public string Name { get; set; }

        public int CountryId { get; set; }


        [Display(Name = "Cities Number")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;


        //[JsonIgnore]
        //public Country Country { get; set; }


        public ICollection<City> Cities { get; set; }
    }
}
