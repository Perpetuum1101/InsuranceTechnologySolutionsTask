using Application.DTOs;
using Application.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController(ICoverService coverService) : ControllerBase
{
    private readonly ICoverService _coverService = coverService;

    /// <summary>
    /// Computes premium price
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>Premium price as decimal</returns>
    [HttpPost("compute")]
    public ActionResult<decimal> ComputePremium(ComputePremiumDTO dto)
    {
        var result = _coverService.ComputePremium(dto);
        return Ok(result);
    }

    /// <summary>
    /// Gets all covers
    /// </summary>
    /// <returns>A collection of covers</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoverDTO>>> GetAsync()
    {
        var results = await _coverService.GetAll();
        return Ok(results);
    }

    /// <summary>
    /// Gets Cover by specified Id
    /// </summary>
    /// <param name="id">GUID as string</param>
    /// <returns>Cover</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CoverDTO>> GetAsync(string id)
    {
        var results = await _coverService.Get(Guid.Parse(id));
        return Ok(results);
    }

    /// <summary>
    /// Creates new Cover
    /// </summary>
    /// <param name="cover">Cover to be created</param>
    /// <returns>Created Cover</returns>
    [HttpPost]
    public async Task<ActionResult<CoverDTO>> CreateAsync(CoverDTO cover)
    {
        var result = await _coverService.Create(cover);
        if (result.IsSuccess)
        {
            return Ok(result.Response);
        }

        return Problem(detail: result.Errors,
                       statusCode: StatusCodes.Status400BadRequest,
                       title: "Bad Request");
    }

    /// <summary>
    /// Deletes Cover by Id
    /// </summary>
    /// <param name="id">GUID as string</param>
    /// <returns>Status code 200 if delete was successful</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _coverService.Delete(Guid.Parse(id));
        return Ok();
    }

    
}
