using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnVen.Web.Helper
{
    public interface IImageHelper
    {
        Task<string> UploadImage(IFormFile imageFile, string ruta, string guid);

        Task<string> UploadImage(byte[] imageFile, string ruta, string guid);

        bool DeleteImage(string ruta, string guid);
    }
}
