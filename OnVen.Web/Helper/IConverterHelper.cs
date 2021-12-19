using OnVen.Common.Entities;
using OnVen.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnVen.Web.Helper
{
    public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, string imageId, bool isNew);

        CategoryViewModel ToCategoryViewModel(Category category);

        Task<Product> ToProductAsync(ProductViewModel model, bool isNew);

        ProductViewModel ToProductViewModel(Product product);
    }
}
