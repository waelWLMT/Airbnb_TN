using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Events;
using Contracts.Events.Proprietaires;
using Domain.Interfaces;
using Infrastructure.Repositories;
using MassTransit;

namespace Infrastructure.Messaging.Consumers.Proprietaires
{
    public class ProprietaireDeletedConsumer : IConsumer<ProprietaireDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProprietaireWriteRepository _proprietaireWriteRepository;
        private readonly IProprietaireReadRepository _proprietaireReadRepository;

        public ProprietaireDeletedConsumer
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
        public async Task Consume(ConsumeContext<ProprietaireDeletedEvent> context)
        {
            var message = context.Message;
            Console.WriteLine("Proprietaire Deleted Consumer received message");            
            await HandleProprietaireDeletion(message.UserId, context);
        }

        private async Task HandleProprietaireDeletion(Guid userId, ConsumeContext context)
        {
            // Handle proprietaire deletion logic here
            Console.WriteLine("Handling proprietaire deletion");
            
            var proprietaire = await _proprietaireReadRepository.GetFirstOne(x=> x.UserId == userId, context.CancellationToken);
            proprietaire.IsActive = false;

            await _proprietaireWriteRepository.UpdateAsync(proprietaire, context.CancellationToken);
            await _unitOfWork.CommitAsync(context.CancellationToken);
        }

        


    }
}
