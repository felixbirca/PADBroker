using System.Text.Json.Serialization;

namespace PADBroker.Sdk;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType
)]
[JsonDerivedType(typeof(GetMessageRequest), typeDiscriminator: "getMessage")]
[JsonDerivedType(typeof(SendMessageRequest), typeDiscriminator: "sendMessage")]
public class BaseRequestMessage { }
