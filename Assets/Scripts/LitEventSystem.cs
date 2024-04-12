using System;
using System.Collections.Generic;
using UnityEngine;

namespace LitFramework
{

    #region IUnRegister
    public interface IUnRegister
    {
        void UnRegister();
    }

    public interface IUnRegisterList
    {
        List<IUnRegister> UnregisterList { get; }
    }

    public static class UnRegisterListExtension
    {
        public static void AddToUnregisterList(this IUnRegister self, IUnRegisterList unRegisterList)
        {
            unRegisterList.UnregisterList.Add(self);
        }

        public static void UnRegisterAll(this IUnRegisterList self)
        {
            foreach (var unRegister in self.UnregisterList)
            {
                unRegister.UnRegister();
            }

            self.UnregisterList.Clear();
        }
    }
    public static class UnRegisterExtension
    {
        public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);
            
            return unRegister;
        }
    }
    
    /// <summary>
    /// 自定义可注销的类
    /// </summary>
    public struct CustomUnRegister : IUnRegister
    {
        /// <summary>
        /// 委托对象
        /// </summary>
        private Action OnUnRegister { get; set; }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        public CustomUnRegister(Action onUnRegsiter)
        {
            OnUnRegister = onUnRegsiter;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void UnRegister()
        {
            OnUnRegister.Invoke();
            OnUnRegister = null;
        }
    }
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnRegister> m_UnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            m_UnRegisters.Add(unRegister);
        }

        public void RemoveUnRegister(IUnRegister unRegister)
        {
            m_UnRegisters.Remove(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in m_UnRegisters)
            {
                unRegister.UnRegister();
            }

            m_UnRegisters.Clear();
        }
    }
    
    #endregion
    #region LitEventSystem
    public class LitEventSystem
    {
        private readonly EasyEvents m_Events = new EasyEvents();


        public static readonly LitEventSystem Global = new LitEventSystem();

        public void Send<T>() where T : new()
        {
            m_Events.GetEvent<EasyEvent<T>>()?.Trigger(new T());
        }

        public void Send<T>(T e)
        {
            m_Events.GetEvent<EasyEvent<T>>()?.Trigger(e);
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var e = m_Events.GetOrAddEvent<EasyEvent<T>>();
            return e.Register(onEvent);
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var e = m_Events.GetEvent<EasyEvent<T>>();
            if (e != null)
            {
                e.UnRegister(onEvent);
            }
        }
    }

    #endregion
    #region EasyEvent

    public interface IEasyEvent
    {
    }
    
    public class EasyEvent : IEasyEvent
    {
        private Action m_OnEvent = () => { };

        public IUnRegister Register(Action onEvent)
        {
            //如果同一个委托已经添加则先移除
            m_OnEvent -= onEvent;
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action onEvent)
        {
            m_OnEvent -= onEvent;
        }

        public void Trigger()
        {
            m_OnEvent?.Invoke();
        }
    }

    public class EasyEvent<T> : IEasyEvent
    {
        private Action<T> m_OnEvent = e => { };

        public IUnRegister Register(Action<T> onEvent)
        {
            //如果同一个委托已经添加则先移除
            m_OnEvent -= onEvent;
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T> onEvent)
        {
            m_OnEvent -= onEvent;
        }

        public void Trigger(T t)
        {
            m_OnEvent?.Invoke(t);
        }
    }

    public class EasyEvent<T, TK> : IEasyEvent
    {
        private Action<T, TK> m_OnEvent = (t, k) => { };

        public IUnRegister Register(Action<T, TK> onEvent)
        {
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, TK> onEvent)
        {
            m_OnEvent -= onEvent;
        }

        public void Trigger(T t, TK k)
        {
            m_OnEvent?.Invoke(t, k);
        }
    }

    public class EasyEvent<T, TK, TS> : IEasyEvent
    {
        private Action<T, TK, TS> m_OnEvent = (t, k, s) => { };

        public IUnRegister Register(Action<T, TK, TS> onEvent)
        {
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, TK, TS> onEvent)
        {
            m_OnEvent -= onEvent;
        }

        public void Trigger(T t, TK k, TS s)
        {
            m_OnEvent?.Invoke(t, k, s);
        }
    }

    public class EasyEvents
    {
        private static EasyEvents _mGlobalEvents = new EasyEvents();

        public static T Get<T>() where T : IEasyEvent
        {
            return _mGlobalEvents.GetEvent<T>();
        }
        

        public static void Register<T>() where T : IEasyEvent, new()
        {
            _mGlobalEvents.AddEvent<T>();
        }

        private Dictionary<Type, IEasyEvent> m_TypeEvents = new Dictionary<Type, IEasyEvent>();
        
        public void AddEvent<T>() where T : IEasyEvent, new()
        {
            m_TypeEvents.Add(typeof(T), new T());
        }

        public T GetEvent<T>() where T : IEasyEvent
        {
            IEasyEvent e;

            if (m_TypeEvents.TryGetValue(typeof(T), out e))
            {
                return (T)e;
            }

            return default;
        }

        public T GetOrAddEvent<T>() where T : IEasyEvent, new()
        {
            var eType = typeof(T);
            if (m_TypeEvents.TryGetValue(eType, out var e))
            {
                return (T)e;
            }

            var t = new T();
            m_TypeEvents.Add(eType, t);
            return t;
        }
    }

    #endregion
    #region EnumEventSystem
    
    public class EnumEventSystem 
    {
        public static readonly EnumEventSystem Global = new EnumEventSystem();
        
        private readonly Dictionary<int, IEasyEvent> m_Events = new Dictionary<int, IEasyEvent>(50);
        
        protected EnumEventSystem(){}

        #region 功能函数

        public IUnRegister Register<T>(T key, Action<int,object[]> onEvent) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (m_Events.TryGetValue(kv, out var e))
            {
                var easyEvent = e as EasyEvent<int,object[]>;
                return easyEvent?.Register(onEvent);
            }
            else
            {
                var easyEvent = new EasyEvent<int,object[]>();
                m_Events.Add(kv, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }

        public void UnRegister<T>(T key, Action<int,object[]> onEvent) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (m_Events.TryGetValue(kv, out var e))
            {
                var ee = e as EasyEvent<int,object[]>;
                ee?.UnRegister(onEvent);
            }
        }

        public void UnRegister<T>(T key) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (m_Events.ContainsKey(kv))
            {
                m_Events.Remove(kv);
            }
        }

        public void UnRegisterAll()
        {
            m_Events.Clear();
        }

        public void Send<T>(T key, params object[] args) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (m_Events.TryGetValue(kv, out var e))
            {
                var ee = e as EasyEvent<int, object[]>;
                ee?.Trigger(kv,args);
            }
        }

        #endregion
        
    }
    #endregion
    #region StringEventSystem
    public class StringEventSystem
    {
        public static readonly StringEventSystem Global = new StringEventSystem();
        
        private Dictionary<string, IEasyEvent> m_Events = new Dictionary<string, IEasyEvent>();
        
        public  IUnRegister Register(string key, Action onEvent)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e as EasyEvent;
                return easyEvent?.Register(onEvent);
            }
            else
            {
                var easyEvent = new EasyEvent();
                m_Events.Add(key,easyEvent);
                return easyEvent.Register(onEvent);
            }
        }

        public void UnRegister(string key, Action onEvent)
        {
            
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e as EasyEvent;
                easyEvent?.UnRegister(onEvent);
            }
        }

        public void Send(string key)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e as EasyEvent;
                easyEvent?.Trigger();
            }
        }
        
        
        public IUnRegister Register<T>(string key, Action<T> onEvent)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e as EasyEvent<T>;
                return easyEvent?.Register(onEvent);
            }
            else
            {
                var easyEvent = new EasyEvent<T>();
                m_Events.Add(key,easyEvent);
                return easyEvent.Register(onEvent);
            }
        }
        

        public void UnRegister<T>(string key, Action<T> onEvent)
        {
            
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e as EasyEvent<T>;
                easyEvent?.UnRegister(onEvent);
            }
        }

        public void Send<T>(string key, T data)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e as EasyEvent<T>;
                easyEvent?.Trigger(data);
            }
        }
    }
    
    #endregion
}