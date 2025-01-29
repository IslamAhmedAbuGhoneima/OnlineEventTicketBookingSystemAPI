using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS;

namespace OnlineEventTicketBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BookController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("users/{userId:guid}/booking")]
        public IActionResult GetUserBookings(string userId)
        {
            var userBooking = _unitOfWork.Booking.FindByCondition(x => x.UserId == userId,"Event", false);

            List<BookingListDto> userBookingList = userBooking.Select(X => new BookingListDto
            {
                EventDate = X.Event!.DateTime,
                EventId = X.EventId,
                EventName = X.Event.Title,
                ReservedSeats = X.SeatsNumber,
                TicketPrice = X.TotalPrice,
            }).ToList();

            return Ok(userBookingList);
        }

        [HttpGet("bookings/{id:guid}")]
        public async Task<IActionResult> GetBooking(Guid id)
        {
            var booking = await _unitOfWork.Booking.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket([FromBody] BookingForCreationDto model)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var eventObj = await _unitOfWork.Events.GetByIdAsync(model.EventId);

            if (eventObj == null)
            {
                return NotFound(new { Message = "Event not found" });
            }


            bool isBookingSuccessful = false;
            lock (eventObj)
            {
                if (eventObj.AvailableSeats >= model.SeatsNumber)
                {
                    eventObj.AvailableSeats -= model.SeatsNumber;
                    isBookingSuccessful = true;
                }
            }

            if (!isBookingSuccessful)
            {
                return BadRequest(new { Message = "Not enough seats available" });
            }

            var userBooking = _mapper.Map<Book>(model);
            userBooking.TotalPrice = model.SeatsNumber * eventObj.TicketPrice;

            await _unitOfWork.Booking.AddAsync(userBooking);
            await _unitOfWork.SaveAsync();

            var bookingResponse = new
            {
                userBooking.Id,
                userBooking.SeatsNumber,
                EventDate = eventObj.DateTime,
                eventObj.TicketPrice,
                TotlalPrice = userBooking.SeatsNumber * eventObj.TicketPrice,
                userBooking.UserId,
                userBooking.EventId
            };

            return CreatedAtAction("GetBooking", new { id = userBooking.Id }, bookingResponse);
        }

        [HttpDelete("{userId:guid}/{eventId:guid}")]
        public async Task<IActionResult> DeleteBooking(Guid userId,Guid eventId)
        {
            var userBooking = _unitOfWork.Booking.FindByCondition(x => x.UserId == userId.ToString() && x.EventId == eventId, includes: null, trackChanges: false)
                .FirstOrDefault();

            if (userBooking == null)
                return NotFound(new { Message = "there is no such booking" });

            _unitOfWork.Booking.Delete(userBooking);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
