namespace Innago.Public.NotificationTemplater.API.Service
{

    using Innago.Public.NotificationTemplater.API.Grpc;

    using Microsoft.Extensions.Caching.Distributed;

    public class NotificationTemplaterService : NotificationTemplater.NotificationTemplaterBase // Inherit from the generated base class
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<NotificationTemplaterService> _logger;

        public NotificationTemplaterService(IDistributedCache cache, ILogger<NotificationTemplaterService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public override async Task<GetTemplateResponse> GetTemplate(GetTemplateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC GetTemplate called for key: {Key}", request.Key);

            string template = await _cache.GetStringAsync(request.Key, context.CancellationToken).ConfigureAwait(false) ?? string.Empty;

            if (string.IsNullOrEmpty(template))
            {
                _logger.LogWarning("gRPC GetTemplate: Template not found for key: {Key}", request.Key);
            }

            return new GetTemplateResponse
            {
                TemplateContent = template
            };
        }
    }
}
