using Application.DTOs;
using Application.Service.Contracts;
using Microsoft.AspNetCore.Mvc;


namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController(IClaimService claimService) : ControllerBase
    {
        private readonly IClaimService _claimService = claimService;

        /// <summary>
        /// Gets all Claims
        /// </summary>
        /// <returns>Collection of claims</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimDTO>>> GetAsync()
        {
            var result = await _claimService.GetAll();

            return Ok(result);
        }

        /// <summary>
        /// Creates a new claim
        /// </summary>
        /// <param name="claim">Claim to be created</param>
        /// <returns>Created claim</returns>
        [HttpPost]
        public async Task<ActionResult<ClaimDTO>> CreateAsync(ClaimDTO claim)
        {
            var result = await _claimService.Create(claim);
            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            return Problem(detail: result.Errors,
                           statusCode: StatusCodes.Status400BadRequest,
                           title: "Bad Request");
        }

        /// <summary>
        /// Deletes Claim by Id
        /// </summary>
        /// <param name="id">GUID as string</param>
        /// <returns>Status code 200 if delete was successful</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            await _claimService.Delete(Guid.Parse(id));
            return Ok();
        }


        /// <summary>
        /// Gets Claim by specified Id
        /// </summary>
        /// <param name="id">GUID as string</param>
        /// <returns>Claim</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimDTO>> GetAsync(string id)
        {
            var result = await _claimService.Get(Guid.Parse(id));
            return Ok(result);
        }
    }
}
