using Kwetter.Services.Common.Application.Eventing;
using System;
using System.Text.Json;

namespace Kwetter.Services.Common.Infrastructure.EventSerializers
{
    /// <summary>
    /// Represents the <see cref="JsonEventSerializer"/> class.
    /// </summary>
    public sealed class JsonEventSerializer : IEventSerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonEventSerializer"/> class.
        /// </summary>
        public JsonEventSerializer()
        {
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <inheritdoc cref="IEventSerializer.Serialize{TMessage}"/>
        public ReadOnlyMemory<byte> Serialize<TMessage>(TMessage message) where TMessage : class
        {
            return JsonSerializer.SerializeToUtf8Bytes(message, _jsonSerializerOptions);
        }

        /// <inheritdoc cref="IEventSerializer.SerializeToString{TMessage}"/>
        public string SerializeToString<TMessage>(TMessage message) where TMessage : class
        {
            return JsonSerializer.Serialize(message, _jsonSerializerOptions);
        }

        /// <inheritdoc cref="IEventSerializer.Deserialize{TMessage}(ReadOnlyMemory{byte})"/>
        public TMessage Deserialize<TMessage>(ReadOnlyMemory<byte> message) where TMessage : class
        {
            return JsonSerializer.Deserialize<TMessage>(message.Span, _jsonSerializerOptions);
        }

        /// <inheritdoc cref="IEventSerializer.Deserialize(ReadOnlyMemory{byte}, Type)"/>
        public object Deserialize(ReadOnlyMemory<byte> message, Type type)
        {
            return JsonSerializer.Deserialize(message.Span, type, _jsonSerializerOptions);
        }
    }
}
