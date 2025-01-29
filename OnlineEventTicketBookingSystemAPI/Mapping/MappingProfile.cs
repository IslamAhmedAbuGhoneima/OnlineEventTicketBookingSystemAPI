using AutoMapper;
using Entities.Models;
using Shared.DTOS;

namespace OnlineEventTicketBookingSystemAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventDto, Event>()
                .ForMember(dest => dest.AvailableSeats,
                opt => opt.MapFrom(src => src.TotalSeats))
                .ForMember(dest => dest.DateTime,
                opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<BookingForCreationDto, Book>()
                .ForMember(dest => dest.BookingDate,
                opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
