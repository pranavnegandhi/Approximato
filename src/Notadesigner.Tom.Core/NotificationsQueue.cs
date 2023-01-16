using System.Collections.Concurrent;

namespace Notadesigner.Tom.Core
{
    public class NotificationsQueue
    {
        private readonly ConcurrentQueue<Notification> _messages = new();

        private readonly SemaphoreSlim _messageEnqueuedSignal = new(0);

        public void Enqueue(Notification message)
        {
            ArgumentNullException.ThrowIfNull(message);

            _messages.Enqueue(message);
            _messageEnqueuedSignal.Release();
        }

        public async Task<Notification?> DequeueAsync(CancellationToken cancellationToken)
        {
            await _messageEnqueuedSignal.WaitAsync(cancellationToken);

            if (_messages.TryDequeue(out var message))
            {
                return message;
            }

            throw new InvalidOperationException("Attempted to dequeue an empty queue.");
        }
    }
}