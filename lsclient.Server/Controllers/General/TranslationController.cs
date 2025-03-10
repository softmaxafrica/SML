using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models;
using lsclient.Server.Shared;
using lsclient.Server.Models.DataPayloads;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TranslationController(AppDbContext context)
        {
            _context = context;
        }

        #region GetGenEngTranslations
        [HttpGet]
        [Route("GetGenEngTranslations")]
        public async Task<IActionResult> GetGenEngTranslations()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetGenEngTranslations);

            try
            {
                var GenTranslations = await _context.GenTranslations.Where(g=>g.language=="english").ToListAsync();
                if (GenTranslations == null)
                {
                    return NotFound("Translation not found");
                }

                var jObject = new JObject();
                foreach (var item in GenTranslations)
                {
                    jObject.Add(item.Code, item.Description);
                }

                executionResult.SetData(JsonConvert.SerializeObject(jObject));
                executionResult.SetDataList(GenTranslations);

                 return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TranslationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

 
        #region AddTranslation
        [HttpPost]
        [Route("AddTranslation")]
        public async Task<IActionResult> AddTranslation([FromBody] GenTranslation newTranslation)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(AddTranslation);

 
            try
            {
                 newTranslation.Cdate= DateTime.Now.ToLocalTime();
                _context.GenTranslations.Add(newTranslation);
                await _context.SaveChangesAsync();

                executionResult.SetData(newTranslation);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TranslationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region UpdateTranslation
        [HttpPut]
        [Route("UpdateTranslationByCode/{Code}")]
        public async Task<IActionResult> UpdateTranslation(string Code, [FromBody] GenTranslation updatedTranslation)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateTranslation);

            try
            {
                var Translation = _context.GenTranslations.Where(t => t.Code == Code).FirstOrDefault();

                if (Translation == null)
                {
                    return NotFound("Translation not found");
                }

                 
                await _context.SaveChangesAsync();

                executionResult.SetData(Translation);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TranslationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region DeleteTranslation
        [HttpDelete]
        [Route("DeleteTranslationByCode/{ByCode}")]
        public async Task<IActionResult> DeleteTranslation(string ByCode)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteTranslation);

            try
            {

                var Translation = _context.GenTranslations.Where(t => t.Code == ByCode).FirstOrDefault();

                if (Translation == null)
                {
                    return NotFound("Translation not found");
                }

                _context.GenTranslations.Remove(Translation);
                await _context.SaveChangesAsync();

                executionResult.SetData(Translation);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TranslationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }
}
