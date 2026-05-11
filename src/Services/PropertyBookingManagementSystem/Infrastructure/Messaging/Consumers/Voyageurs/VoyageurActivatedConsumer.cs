using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Events;
using Contracts.Events.Voyageurs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Repositories;
using MassTransit;

namespace Infrastructure.Messaging.Consumers.Voyageurs
{
    public class VoyageurActivatedConsumer : IConsumer<VoyageurActivatedEvent>
    {
      
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVoyageurReadRepository _voyageurReadRepository;
        private readonly IVoyageurWriteRepository _voyageurWriteRepsoitory;

        public VoyageurActivatedConsumer(IUnitOfWork unitOfWork, IVoyageurReadRepository voyageurReadRepository, IVoyageurWriteRepository voyageurWriteRepsoitory)
        {
            _unitOfWork = unitOfWork;
            _voyageurReadRepository = voyageurReadRepository;
            _voyageurWriteRepsoitory = voyageurWriteRepsoitory;
        }
        public async Task Consume(ConsumeContext<VoyageurActivatedEvent> context)
        {
            Console.WriteLine("Voyageur Activated Consumer received message");

            var message = context.Message;

            if (message is not null)
                await handleVoyageurActivation(message, context.CancellationToken);

        }

        private async Task handleVoyageurActivation(VoyageurActivatedEvent voyageurActivatedEvent, CancellationToken cancellationToken)
        {
            // Handle voyageur activation logic here
            Console.WriteLine("Handling voyageur activation");

            var voyageur = await _voyageurReadRepository.GetFirstOne(x=> x.UserId == voyageurActivatedEvent.UserId, cancellationToken);

            if (voyageur is not null)
            {
                voyageur.IsActive = voyageurActivatedEvent.IsActivated;
                await _voyageurWriteRepsoitory.UpdateAsync(voyageur, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);
            }

        }

    }
}
