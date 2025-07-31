using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string ImageName { get; set; }
        public string? ImageDescription { get; set; }
        public string ImageExtension { get; set; }
        public string ImagePath { get; set; }
        public long ImageSizeInBytes { get; set; }
    }
}