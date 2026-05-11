using Domain.Interfaces;

namespace Presentation.Helpers
{
    public class CorrelationContext : ICorrelationContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CorrelationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid CorrelationId
        {
           get
            {                               
                var value = _httpContextAccessor.HttpContext?.Items["CorrelationId"];

                if (value is Guid guid)
                    return guid;

                if (value is string str && Guid.TryParse(str, out var parsed))
                    return parsed;

                return Guid.Empty;
            }
        }
    }
}
