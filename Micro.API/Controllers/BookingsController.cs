using Micro.API.Models;
using Micro.API.Services;
using Microsoft.AspNetCore.Mvc;
namespace Micro.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingsController : ControllerBase
{
    private readonly ILogger<BookingsController> _logger;
    private readonly IMessageProducer _messageProducer;

    public BookingsController(ILogger<BookingsController> logger, IMessageProducer messageProducer)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }

    // in-memory db
    public static readonly List<Booking> _bookings = new();

    [HttpPost]
    public IActionResult CreateBooking(Booking booking)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _bookings.Add(booking);

        _messageProducer.SendingMessage(booking);

        return Ok();
    }
}