using Kwetter.Services.Common.EventBus.Abstractions;
using System;
using System.Text.Json;

namespace Kwetter.Services.Common.Infrastructure.MessageSerializers
{
    /// <summary>
    /// Represents the <see cref="JsonMessageSerializer"/> class.
    /// </summary>
    public sealed class JsonMessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonMessageSerializer()
        {
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <inheritdoc cref="IMessageSerializer.Serialize{TMessage}"/>
        public ReadOnlyMemory<byte> Serialize<TMessage>(TMessage message) where TMessage : class
        {
            return JsonSerializer.SerializeToUtf8Bytes(message, _jsonSerializerOptions);
        }

        /// <inheritdoc cref="IMessageSerializer.SerializeToString{TMessage}"/>
        public string SerializeToString<TMessage>(TMessage message) where TMessage : class
        {
            return JsonSerializer.Serialize(message, _jsonSerializerOptions);
        }

        /// <inheritdoc cref="IMessageSerializer.Deserialize{TMessage}(ReadOnlyMemory{byte})"/>
        public TMessage Deserialize<TMessage>(ReadOnlyMemory<byte> message) where TMessage : class
        {
            return JsonSerializer.Deserialize<TMessage>(message.Span, _jsonSerializerOptions);
        }

        /// <inheritdoc cref="IMessageSerializer.Deserialize(ReadOnlyMemory{byte}, Type)"/>
        public object Deserialize(ReadOnlyMemory<byte> message, Type type)
        {
            return JsonSerializer.Deserialize(message.Span, type, _jsonSerializerOptions);
        }
    }
}
