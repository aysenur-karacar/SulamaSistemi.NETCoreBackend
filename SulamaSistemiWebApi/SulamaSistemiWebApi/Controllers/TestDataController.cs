using Microsoft.AspNetCore.Mvc;
using SulamaSistemiWebApi.Services;

namespace SulamaSistemiWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestDataController : ControllerBase
{
    private readonly TestDataService _testDataService;

    public TestDataController(TestDataService testDataService)
    {
        _testDataService = testDataService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTestData()
    {
        var result = await _testDataService.CreateTestDataAsync();
        return Ok(new { message = result });
    }

    [HttpPost("create-multiple")]
    public async Task<IActionResult> CreateMultipleTestData([FromQuery] int count = 10)
    {
        var result = await _testDataService.CreateMultipleTestDataAsync(count);
        return Ok(new { message = result });
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearTestData()
    {
        var result = await _testDataService.ClearTestDataAsync();
        return Ok(new { message = result });
    }
} 