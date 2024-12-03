using CryptoTradingDesktopApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class FirestoreTestController : ControllerBase
{
    private readonly FirestoreService _firestoreService;

    public FirestoreTestController(FirestoreService firestoreService)
    {
        _firestoreService = firestoreService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddTestOrder()
    {
        var order = new { OrderId = Guid.NewGuid(), CryptoName = "BTC", Amount = 1, Price = 50000 };
        await _firestoreService.AddDocumentAsync("TestCollection", order.OrderId.ToString(), order);
        return Ok("Document added.");
    }
}
