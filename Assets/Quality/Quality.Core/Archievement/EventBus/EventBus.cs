using System;
using System.Collections.Generic;
using Quality.Core.Logger;

namespace Quality.Core.EventBus
{
    public class EventBus<TBaseEvent> : IDisposable
    {
        private Queue<QueuedEvent> _queuedEvents = new(32);
        private int                _raiseRecursionDepth;

        private event Action<EventBus<TBaseEvent>> disposeEvent;

        public delegate void EventHandler<TEvent>(ref TEvent eventData) where TEvent : TBaseEvent;

        public void RaiseImmediately<TEvent>(ref TEvent @event) where TEvent : TBaseEvent
        {
            _raiseRecursionDepth++;

            try
            {
                var listeners = EventListeners<TEvent>.GetListeners(this);

                foreach (var listener in listeners)
                {
                    try
                    {
                        listener?.Invoke(ref @event);
                    }
                    catch (Exception e)
                    {
                        this.LogError(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                this.LogError(e.Message);
            }
            finally
            {
                _raiseRecursionDepth--;

                if (_raiseRecursionDepth == 0)
                {
                    while (_queuedEvents.Count > 0)
                    {
                        var queuedEvent = _queuedEvents.Dequeue();
                        queuedEvent.Raise(this);
                    }
                }
            }
        }

        public void Raise<TEvent>(in TEvent @event) where TEvent : TBaseEvent
        {
            if (_raiseRecursionDepth == 0)
            {
                var eventCopy = @event;
                RaiseImmediately(ref eventCopy);
            }

            var listeners = EventListeners<TEvent>.GetListeners(this);
            listeners.EnqueueEvent(in @event);
        }

        public virtual void SubscribeTo<TEvent>(EventHandler<TEvent> handler) where TEvent : TBaseEvent
        {
            var listeners = EventListeners<TEvent>.GetListeners(this);
            listeners.AddListener(handler);
        }

        public virtual void UnsubscribeFrom<TEvent>(EventHandler<TEvent> handler) where TEvent : TBaseEvent
        {
            var listeners = EventListeners<TEvent>.GetListeners(this);
            listeners.RemoveListener(handler);
        }

        public void ClearListeners<TEvent>() where TEvent : TBaseEvent
        {
            if (_raiseRecursionDepth > 0)
            {
                throw new InvalidOperationException("Không thể xóa listener trong khi đang xử lý sự kiện.");
            }

            var listeners = EventListeners<TEvent>.GetListeners(this);
            listeners.Clear();
        }

        public virtual void Dispose()
        {
            disposeEvent?.Invoke(this);
            disposeEvent = null;
        }

        protected abstract class QueuedEvent
        {
            public abstract void Raise(EventBus<TBaseEvent> eventBus);
        }

        private sealed class EventListeners<TEvent> : List<EventHandler<TEvent>> where TEvent : TBaseEvent
        {
            private static readonly Dictionary<EventBus<TBaseEvent>, EventListeners<TEvent>> s_listenersMap = new();

            private static readonly Stack<DerivedQueuedEvent> s_queuedEventPool = new();

            private readonly EventBus<TBaseEvent> _eventBus;

            private EventListeners(EventBus<TBaseEvent> eventBus)
            {
                _eventBus = eventBus;
            }

            public static EventListeners<TEvent> GetListeners(EventBus<TBaseEvent> eventBus)
            {
                if (!s_listenersMap.TryGetValue(eventBus, out var listeners))
                {
                    listeners = new EventListeners<TEvent>(eventBus);
                    s_listenersMap.Add(eventBus, listeners);

                    eventBus.disposeEvent += EventBusOnDisposeEvent;
                }

                return listeners;
            }

            private static void EventBusOnDisposeEvent(EventBus<TBaseEvent> eventBus)
            {
                s_listenersMap.Remove(eventBus);
            }

            public void AddListener(EventHandler<TEvent> handler)
            {
                if (!Contains(handler))
                {
                    Add(handler);
                }
            }

            public void RemoveListener(EventHandler<TEvent> handler)
            {
                Remove(handler);
            }

            public void EnqueueEvent(in TEvent @event)
            {
                var queuedEvent = s_queuedEventPool.Count > 0 ? s_queuedEventPool.Pop() : new DerivedQueuedEvent();
                queuedEvent.eventData = @event;
                _eventBus._queuedEvents.Enqueue(queuedEvent);
            }
            
            private sealed class DerivedQueuedEvent : QueuedEvent
            {
                public TEvent eventData;

                public override void Raise(EventBus<TBaseEvent> eventBus)
                {
                    eventBus.Raise(eventData);
                    
                    eventData = default;
                    s_queuedEventPool.Push(this);
                }
            }
        }
    }
}

