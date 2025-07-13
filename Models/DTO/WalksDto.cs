using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.Models.Domain;

namespace NZWalks.Models.DTO
{
    public class WalksDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public RegionDTO Region { get; set; }
    }
}