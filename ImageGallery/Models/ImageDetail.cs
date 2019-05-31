using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Models
{
    public class ImageDetail
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string ImageName { get; set; }

        [Display(Name = "Description")]
        public string ImageDescription { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Image")]
        public string PathImage { get; set; } = null;

        [Display(Name = "Size")]
        public long Size { get; set; }


        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
