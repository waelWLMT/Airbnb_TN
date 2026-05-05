using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Events.Proprietaires;
using Contracts.Events.Voyageurs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Messaging.Consumers.Proprietaires
{
    public class ProprietaireActivatedConsumer : IConsumer<ProprietaireActivatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProprietaireReadRepository _proprietaireReadRepository;
        private readonly IProprietaireWriteRepository _proprietaireWriteRepository;

        public ProprietaireActivatedConsumer
            (
            IUnitOfWork unitOfWork,
            IProprietaireReadRepository proprietaireReadRepository,
            IProprietaireWriteRepository proprietaireWriteRepository
            )
        {
            _unitOfWork = unitOfWork;
            _proprietaireReadRepository = proprietaireReadRepository;
            _proprietaireWriteRepository = proprietaireWriteRepository;
        }

        public async Task Consume(ConsumeContext<ProprietaireActivatedEvent> context)
        {
            Console.WriteLine("Proprietaire ActivatedConsumer received message");

            var message = context.Message;

            await HandleProprietaireActivation(message, context.CancellationToken);
        }

        private async Task HandleProprietaireActivation(ProprietaireActivatedEvent proprietaireActivatedEvent, CancellationToken cancellationToken)
        {
            // Handle proprietaire activation logic here
            Console.WriteLine("Handling Proprietaire Activated Event");

            var proprietaire = await _proprietaireReadRepository.GetFirstOne(p => p.UserId == proprietaireActivatedEvent.UserId, cancellationToken);
            
            if(proprietaire != null)
            {
                proprietaire.IsActive = proprietaireActivatedEvent.IsActive;
                await _proprietaireWriteRepository.UpdateAsync(proprietaire, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
            }
           
        }

    }
}
