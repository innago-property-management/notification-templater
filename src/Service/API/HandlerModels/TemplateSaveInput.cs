namespace Innago.Public.NotificationTemplater.API.HandlerModels;

using JetBrains.Annotations;

[PublicAPI]
public record TemplateSaveInput(string Key, string Template);