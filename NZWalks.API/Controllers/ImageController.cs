using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Models.Repositories;

namespace NZWalks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        // POST: /api/Images/Upload
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequest request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {

                //convert DTO to Domain model
                var ImageDomainModel = new Image
                {
                    File = request.File,
                    ImageName = request.FileName,
                    ImageDescription = request.FileDescription,
                    ImageExtension = Path.GetExtension(request.File.FileName),
                    ImageSizeInBytes = request.File.Length
                };

                // Use repository to upload image
                await imageRepository.Upload(ImageDomainModel);

                return Ok(ImageDomainModel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequest request)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is more than 10MB, please enter a smaller size file.");
            }
        }
    }
}