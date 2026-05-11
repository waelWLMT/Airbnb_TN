using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Events;
using Contracts.Events.Proprietaires;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Messaging.Consumers.Proprietaires
{
    public class ProprietaireCreatedConsumer : IConsumer<ProprietaireCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProprietaireReadRepository _proprietaireReadRepository;
        private readonly IProprietaireWriteRepository _proprietaireWriteRepository;
        private readonly ILogger<ProprietaireCreatedConsumer> _logger;

        public ProprietaireCreatedConsumer(IUnitOfWork unitOfWork,
            IProprietaireReadRepository proprietaireReadRepository,
            IProprietaireWriteRepository proprietaireWriteRepository,
            ILogger<ProprietaireCreatedConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _proprietaireReadRepository = proprietaireReadRepository;
            _proprietaireWriteRepository = proprietaireWriteRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<ProprietaireCreatedEvent> context)
        {
            _logger.LogInformation("Proprietaire Created Consumer received message");

            var message = context.Message;

            if (message is not null)
            {
                var userId = message.UserId;
                var userExist = await _proprietaireReadRepository.AnyAsync(l => l.UserId == userId, context.CancellationToken);

                if (!userExist)
                    await HandleProprietaireCreation(userId, context);
            }

            _logger.LogInformation("Proprietaire Created Event has been treated");

        }
        private async Task HandleProprietaireCreation(Guid userId, ConsumeContext context)
        {
            var proprietaire = new Proprietaire() { UserId = userId };
            await _proprietaireWriteRepository.AddAsync(proprietaire, context.CancellationToken);
            await _unitOfWork.CommitAsync(context.CancellationToken);
        }

    }
}
