using System.Text.Json.Serialization;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType
)]
[JsonDerivedType(typeof(GetMessageRequest), typeDiscriminator: "getMessage")]
[JsonDerivedType(typeof(PADMessage), typeDiscriminator: "sendMessage")]
public class BaseRequestMessage { }
