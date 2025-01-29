using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS;

namespace OnlineEventTicketBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("events")]
        public async Task<IActionResult> Events()
        {
            var events = await _unitOfWork.Events.ListAllAsync();
            return Ok(events);
        }

        [HttpGet("event/{id:guid}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var eventObj  = await _unitOfWork.Events.GetByIdAsync(id);
            if (eventObj is null)
                return NotFound();

            return Ok(eventObj);
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin,organizer")]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto model)
        {
            if (ModelState.IsValid)
            {
                var eventObj = _mapper.Map<Event>(model);

                await _unitOfWork.Events.AddAsync(eventObj);
                await _unitOfWork.SaveAsync();
                return CreatedAtAction("GetEvent", new { id = eventObj.Id }, eventObj);
            }
            return UnprocessableEntity(ModelState);
        }


        [HttpPut("update/{id:guid}")]
        [Authorize(Roles = "admin,organizer")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventDto model)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var obj = await _unitOfWork.Events.GetByIdAsync(id);

            if (obj is null)
                return NotFound();

            _mapper.Map(model, obj);
            _unitOfWork.Events.Update(obj);
            await _unitOfWork.SaveAsync();

            return Ok(obj);

        }


        [HttpDelete("delete/{id:guid}")]
        [Authorize(Roles = "admin,organizer")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var obj = await _unitOfWork.Events.GetByIdAsync(id);

            if (obj is null)
                return NotFound();

            _unitOfWork.Events.Delete(obj);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("organizer/{orgnizerId:guid}/events")]
        [Authorize(Roles= "admin,organizer")]
        public IActionResult GetEventOrgnizedByUser(Guid orgnizerId)
        {
            var user = _unitOfWork.Users.
                FindByCondition(U => U.Id == orgnizerId.ToString(), "OrganizedEvents", false)
                .FirstOrDefault();
            
            if (user is null)
                return NotFound(new { Message = "Somthing went wrong" });

            var userOrganizedEventsList = user.OrganizedEvents?
                .Select(X => new 
                {
                    X.Title,
                    X.Description,
                    X.Location,
                    X.DateTime,
                    X.TicketPrice,
                    X.TotalSeats,
                }).ToList();

            var organizedEvents = new
            {
                user.Id,
                user.UserName,
                user.Email,
                OrgnizedEvents = userOrganizedEventsList
            };

            return Ok(organizedEvents);
        }
    }
}
