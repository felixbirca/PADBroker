using System.Text.Json.Serialization;

namespace PADBroker.Sdk;

[JsonPolymorphic(
    TypeDiscriminatorPropertyName = "$type",
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType
)]
[JsonDerivedType(typeof(GetMessageResponse), "getMessageResponse")]
[JsonDerivedType(typeof(SendMessageResponse), "sendMessageResponse")]
public class BaseResponseMessage { }
