using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnVen.Web.Helper
{
    public class ImageHelper : IImageHelper
    {
        public async Task<string> UploadImage(IFormFile imageFile, string ruta, string guid)
        {
            var file = guid;
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                ruta,
                file);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"{file}";
        }


        public bool DeleteImage(string ruta, string guid)
        {
            string path;
            path = Path.Combine(
                Directory.GetCurrentDirectory(),
                ruta,
                guid);

            File.Delete(path);

            return true;
        }
    }
}
