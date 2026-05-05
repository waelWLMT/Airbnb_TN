using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Events;
using Contracts.Events.Voyageurs;
using Domain.Interfaces;
using Infrastructure.Repositories;
using MassTransit;

namespace Infrastructure.Messaging.Consumers.Voyageurs
{
    public class VoyageurDeletedConsumer : IConsumer<VoyageurDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVoyageurReadRepository _voyageurReadRepository;
        private readonly IVoyageurWriteRepository _voyageurWriteRepository;

        public VoyageurDeletedConsumer(IUnitOfWork unitOfWork,
            IVoyageurReadRepository voyageurReadRepository,
            IVoyageurWriteRepository voyageurWriteRepository)
        {
            _unitOfWork = unitOfWork;
            _voyageurReadRepository = voyageurReadRepository;
            _voyageurWriteRepository = voyageurWriteRepository;
        }
        public async Task Consume(ConsumeContext<VoyageurDeletedEvent> context)
        {
            var message = context.Message;
            
            Console.WriteLine("Voyageur Deleted Consumer received message");

            await HandleLocataireDeletion(message.UserId, context);

        }

        private async Task HandleLocataireDeletion(Guid userId, ConsumeContext context)
        {
            // Handle locataire deletion logic here
            Console.WriteLine("Handling voaygeur deletion");

            var voyageur = await _voyageurReadRepository.GetFirstOne(x=> x.UserId == userId);
           
            if (voyageur != null)
            {
                voyageur.IsActive = false;
                _voyageurWriteRepository.UpdateAsync(voyageur);
                await _unitOfWork.CommitAsync(context.CancellationToken);                
            }
        }


    }
}
