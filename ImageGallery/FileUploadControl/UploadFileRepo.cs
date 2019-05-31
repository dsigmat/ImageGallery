using ImageGallery.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.FileUploadControl
{
    public class UploadFileRepo : IUploadInterface
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public UploadFileRepo(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async void UploadFileMultiple(IList<IFormFile> files)
        {
            long totalBytes = files.Sum(f => f.Length);
            foreach (IFormFile item in files)
            {
                string filename = item.FileName.Trim('"');
                byte[] buffer = new byte[16 * 1024];
                using (FileStream output = System.IO.File.Create(this.GetpathAndFileName(filename)))
                {
                    using (Stream input = item.OpenReadStream())
                    {
                        //long totalReadBytes = 0;
                        int readBytes;
                        while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            await output.WriteAsync(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        }

                    }
                }
            }
        }

        private string GetpathAndFileName(string filename)
        {
            string path = _hostingEnvironment.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + filename;
        }
    }
}
