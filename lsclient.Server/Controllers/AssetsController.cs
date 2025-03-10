using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models; 
using lsclient.Server.Shared;
using lsclient.Server.Controllers.SecurityUser;
using lsclient.Server.Models.DataPayloads;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment; // Add this field

        public AssetsController(AppDbContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            _context = context;
            _config = config;
            _environment = environment;
        }
      
        #region RegisterAssets
        [HttpPost]
        [Route("RegisterImage")]
        public async Task<IActionResult> RegisterImage([FromForm] Image newImage, [FromForm] IFormFile? file)
        {
            var functionName = nameof(RegisterImage);
            var executionResult = new ExecutionResult();
            Image data = new Image();

            try
            {
                // Check if email already exists
                var existingImage = await _context.Images
                                                   .FirstOrDefaultAsync(d => d.ImageUrl == newImage.ImageUrl);

                if (existingImage != null)
                {
                    executionResult.SetFailure("Image is already registered. Please use a different email.");
                    return BadRequest(executionResult.GetServerResponse().Message);
                }

                 
                // Save the image file if provided
                if (file != null)
                {
                    var folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", newImage.Directory);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Get the file extension
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = $"{data.ImageName}{fileExtension}";  // Save the file with correct extension
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                     data.ImageUrl = Path.Combine("assets", "images", newImage.Directory, fileName).Replace("\\", "/");
                }

                _context.Images.Add(data);
                await _context.SaveChangesAsync();

                // Send the default password to the user via email
                executionResult.SetData(data);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion


        #region GetAllImages
        [HttpGet]
        [Route("GetAllImages")]
        public IActionResult GetAllImages()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllImages);

            try
            {
                var Images = _context.Images
                     .ToList();
                executionResult.SetData(Images);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(AssetsController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        

        #region DeleteImage
        [HttpDelete]
        [Route("DeleteImage/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteImage);

            try
            {
                var Image = await _context.Images.FindAsync(id);

                if (Image == null)
                {
                    return NotFound("Image not found");
                }

                _context.Images.Remove(Image);
                await _context.SaveChangesAsync();

                executionResult.SetData(Image);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(AssetsController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

       

    }
}
