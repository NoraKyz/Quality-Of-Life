using UnityEngine;

namespace Quality.Core.EventBus
{
    public static class Messenger
    {
        private static EventBus<IEvent> s_eventBus = new();
        
#if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDontLoadDomain()
        {
            Dispose();
        }

#endif
        
        public static void Raise<TEvent>(in TEvent @event) where TEvent : IEvent
        {
            s_eventBus.Raise(in @event);
        }
        
        public static void RaiseImmediately<TEvent>(ref TEvent @event) where TEvent : IEvent
        {
            s_eventBus.RaiseImmediately(ref @event);
        }
        
        public static void SubcribeTo(EventBus<IEvent>.EventHandler<IEvent> handler)
        {
            s_eventBus.SubscribeTo(handler);
        }
        
        public static void UnsubcribeFrom(EventBus<IEvent>.EventHandler<IEvent> handler)
        {
            s_eventBus.UnsubscribeFrom(handler);
        }

        public static void Dispose()
        {
            s_eventBus.Dispose();
            s_eventBus = new EventBus<IEvent>();
        }
    }
}
