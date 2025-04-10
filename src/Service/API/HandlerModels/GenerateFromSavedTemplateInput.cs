namespace Innago.Public.NotificationTemplater.API.HandlerModels;

using JetBrains.Annotations;

[PublicAPI]
public record GenerateFromSavedTemplateInput(string Key, string Model);