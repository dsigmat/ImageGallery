using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Interface
{
    public interface IUploadInterface
    {
        void UploadFileMultiple(IList<IFormFile> files);
    }
}
