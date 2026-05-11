using Application.UserCases.Commands;
using Application.UserCases.Queries;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LogementController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CreateLogement")]
        public async Task<int> CreateLogement(CreateLogementCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpGet]
        [Route("GetLogementById")]
        public async Task<LogementReadDto> GetLogementById(int id)
        {
            var logement = await _mediator.Send(new GetLogementByIdRequest { Id = id });            
            var result = _mapper.Map<LogementReadDto>(logement);

            return result;
        }

        [HttpGet]
        [Route("GetAllLogement")]
        public async Task<List<LogementReadDto>> GetAllLogement()
        {            
            var logements = await _mediator.Send(new GetAllLogementRequest());            
            
            return _mapper.Map<List<LogementReadDto>>(logements);
        }
    }

}
