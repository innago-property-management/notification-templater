using Grpc.Core;
using NotificationTemplater.Grpc;
using System.Collections.Concurrent;
using Status = Grpc.Core.Status;
using StatusCode = Grpc.Core.StatusCode;

namespace Innago.Public.NotificationTemplater.Service
{

    public class TemplateGrpcService : TemplateProcessor.TemplateProcessorBase
    {
        // Simulated in-memory store for templates
        private static readonly ConcurrentDictionary<string, (string Content, string Subject)> _templateStore = new()
        {
            ["welcome"] = ("<h1>Welcome, {UserName}</h1>", "Welcome Email"),
            ["reset-password"] = ("<p>Click here to reset your password: {Link}</p>", "Password Reset"),
        };

        private static readonly ConcurrentBag<string> _rollbackLog = new();

        public override Task<TemplateResponse> GetTemplate(TemplateRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.TemplateId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Template ID cannot be null or empty."));
            }

            if (_templateStore.TryGetValue(request.TemplateId, out var templateData))
            {
                var response = new TemplateResponse
                {
                    TemplateId = request.TemplateId,
                    Content = templateData.Content,
                    Subject = templateData.Subject
                };

                return Task.FromResult(response);
            }

            throw new RpcException(new Status(StatusCode.NotFound, $"Template with ID '{request.TemplateId}' not found."));
        }

        public override Task<RollbackTemplateResponse> RollbackTemplate(RollbackTemplateRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.TemplateId))
            {
                return Task.FromResult(new RollbackTemplateResponse { Success = false });
            }

            // Simulate rollback: log the rollback attempt
            _rollbackLog.Add(request.TemplateId);

            Console.WriteLine($"[Rollback] Rollback triggered for template: {request.TemplateId}");

            return Task.FromResult(new RollbackTemplateResponse { Success = true });
        }
    }
}
