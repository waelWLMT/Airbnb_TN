using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Models
{
    public class OutboxMessage
    {       
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public required string EventName { get; set; }
        public required string Content { get; set; }       
        public DateTime CreatedAt { get; set; }
        public  DateTime? LockedUntil { get; set; }
        public DateTime? NextRetryAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public OutboxStatus Status { get; set; }
        public int RetryCount { get; set; }
        public string? LastError { get; set; }
    }
}
