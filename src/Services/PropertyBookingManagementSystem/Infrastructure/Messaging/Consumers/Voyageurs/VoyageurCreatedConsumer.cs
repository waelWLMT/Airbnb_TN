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
using MassTransit;


namespace Infrastructure.Messaging.Consumers.Voyageurs
{
    public class VoyageurCreatedConsumer : IConsumer<VoyageurCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVoyageurReadRepository _voyageurReadRepository;
        private readonly IVoyageurWriteRepository _voyageurWriteRepsoitory;

        public VoyageurCreatedConsumer(IUnitOfWork unitOfWork, IVoyageurReadRepository voyageurReadRepository, IVoyageurWriteRepository voyageurWriteRepsoitory)    
        {
            _unitOfWork = unitOfWork;
            _voyageurReadRepository = voyageurReadRepository;
            _voyageurWriteRepsoitory = voyageurWriteRepsoitory;
        }
        public async Task Consume(ConsumeContext<VoyageurCreatedEvent> context)
        {

            Console.WriteLine("Voyageur Created Consumer received message");

            var message = context.Message;

            if (message is not null)
            {
                var userId = message.UserId;
                var userExist = false;

                userExist = await _voyageurReadRepository.AnyAsync(l => l.UserId == userId, context.CancellationToken);

                if (!userExist)
                    await HandleLocataireCreation(userId, context);

            }
        }
        
        private async Task HandleLocataireCreation(Guid userId, ConsumeContext context)
        {
            var voyageur = new Voyageur() { UserId = userId };            
            await _voyageurWriteRepsoitory.AddAsync(voyageur, context.CancellationToken);
            await _unitOfWork.CommitAsync(context.CancellationToken);
        }

    }

}
