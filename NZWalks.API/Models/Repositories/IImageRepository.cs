using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.Models.Domain;

namespace NZWalks.Models.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}