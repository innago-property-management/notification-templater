using System.Text.Json.Serialization;

using Innago.Platform.Messaging.EntityEvents;

using Scriban;

namespace Innago.Public.NotificationTemplater;

[JsonSerializable(typeof(Template))]
[JsonSerializable(typeof(IEntityEventInfo<Template>))]
[JsonSerializable(typeof(EntityEventInfo<Template>))]
internal partial class SourceGenerationContext : JsonSerializerContext;