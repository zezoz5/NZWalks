using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Models.DTO
{
    public class CreateRegionRequest
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be max of 100 characters.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(3, ErrorMessage = "Code must be a maximum of 3 characters.")]
        [MinLength(3, ErrorMessage = "Code must be at least 3 characters.")]
        public string Code { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}