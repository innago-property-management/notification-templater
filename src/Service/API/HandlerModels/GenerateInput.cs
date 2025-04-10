namespace Innago.Public.NotificationTemplater.API.HandlerModels;

using JetBrains.Annotations;

[PublicAPI]
public record GenerateInput(string Template, string Model);