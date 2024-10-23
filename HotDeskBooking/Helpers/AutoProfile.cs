using AutoMapper;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Requests;

namespace HotDeskBooking.Helpers;

public class AutoProfile : Profile
{
    public AutoProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<LocationDto, Location>();
        CreateMap<Location, LocationDto>();
        CreateMap<DeskDto, Desk>();
        CreateMap<Desk, DeskDto>();
        CreateMap<BookingDto, Booking>();
        CreateMap<Booking, BookingDto>();
        CreateMap<UserRoleDto, UserRole>();
        CreateMap<UserRole, UserRoleDto>();
        CreateMap<CreateUserRequest, User>();
    }
}
