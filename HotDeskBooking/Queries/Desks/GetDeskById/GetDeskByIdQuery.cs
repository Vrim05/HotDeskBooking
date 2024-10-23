using HotDeskBooking.Models.Dto;
using MediatR;

namespace HotDeskBooking.Queries.Desks.GetDeskById;

public record GetDeskByIdQuery(int Id) : IRequest<DeskDto>;
