using System;

namespace Kwetter.Services.Common.EventBus.Abstractions
{
    /// <summary>
    /// Represents the <see cref="IMessageSerializer"/> interface.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Serializes a message to a readonly memory byte array.
        /// </summary>
        /// <typeparam name="TMessage">The type of message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>Returns a read only memory byte array.</returns>
        public ReadOnlyMemory<byte> Serialize<TMessage>(TMessage message) where TMessage : class;
        
        /// <summary>
        /// Serializes a message to a string.
        /// </summary>
        /// <typeparam name="TMessage">The type of message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>Returns the message serialized as string.</returns>
        public string SerializeToString<TMessage>(TMessage message) where TMessage : class;

        /// <summary>
        /// Deserializes a readonly memory byte to a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message.</typeparam>
        /// <param name="message">The read only memory byte array containing the message.</param>
        /// <returns>Returns the deserialized message.</returns>
        public TMessage Deserialize<TMessage>(ReadOnlyMemory<byte> message) where TMessage : class;

        /// <summary>
        /// Deserializes a readonly memory byte to a message object.
        /// </summary>
        /// <param name="message">The readonly memory byte array containing the message.</param>
        /// <param name="type">The message type.</param>
        /// <returns>Returns the deserialized message.</returns>
        public object Deserialize(ReadOnlyMemory<byte> message, Type type);
    }
}
