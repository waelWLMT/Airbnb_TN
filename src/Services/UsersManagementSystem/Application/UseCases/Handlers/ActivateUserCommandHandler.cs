using Application.Services;
using Application.UseCases.Commands;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Handlers
{
    public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepositroy;
        private readonly IOutboxMessageWriteRepository _outboxMessageRepository;
        private readonly ICorrelationContext _correlationContext;

        public ActivateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepositroy,
            IOutboxMessageWriteRepository outboxMessageRepository,
            ICorrelationContext correlationContext)
        {
            _unitOfWork = unitOfWork;
            _userReadRepository = userReadRepository;
            _userWriteRepositroy = userWriteRepositroy;
            _outboxMessageRepository = outboxMessageRepository;
            _correlationContext = correlationContext;
            
        }
        public async Task<bool> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetByIdAsync(request.Id);

            if (user == null)
                return false;

            user.IsActive = request.ActivateUser;
            await _userWriteRepositroy.UpdateAsync(user);

            if (user.RoleId == (int)UserRole.Admin)
            {
                await _unitOfWork.CommitAsync();
                return true;
            }


            var outboxMessage = user.RoleId == (int)UserRole.Voyageur
                                                                    ? OutBoxMessageBuilder.BuildVoyageurActivatedMessage(user, _correlationContext.CorrelationId)
                                                                    : OutBoxMessageBuilder.BuildProprietaireActivatedMessage(user, _correlationContext.CorrelationId);

            await _outboxMessageRepository.AddAsync(outboxMessage);

            await _unitOfWork.CommitAsync();

            return true;

        }
    }
}
