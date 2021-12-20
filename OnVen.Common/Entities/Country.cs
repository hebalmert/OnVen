using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnVen.Common.Entities
{
    public class Country
    {
        public int CountryId { get; set; }

        [MaxLength(50, ErrorMessage = "The Maxlength is {0}")]
        [Required(ErrorMessage = "The {0} field is Required")]
        [Display(Name = "Country")]
        public string Name { get; set; }

        [Display(Name = "Departments Number")]
        public int DepartmentsNumber => Departments == null ? 0 : Departments.Count;

        public ICollection<Department> Departments { get; set; }
    }
}
