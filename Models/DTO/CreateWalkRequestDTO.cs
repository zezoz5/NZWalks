using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Models.DTO
{
    public class CreateWalkRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [Range(0,50)]
        public double LengthInKm { get; set; }
        
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid RegionId { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
    }
}