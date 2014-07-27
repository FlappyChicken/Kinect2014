using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace Helper
{
    internal class EventPump : UnityEngine.MonoBehaviour
    {
        private static object s_Lock = new object();
        private Queue<Action> m_Queue = new Queue<Action>();

        public static EventPump Instance 
        {
            get;
            private set;
        }

        public static void EnsureInitialized()
        {
            try
            {
                if (EventPump.Instance == null)
                {
                    lock (s_Lock)
                    {
                        if (EventPump.Instance == null)
                        {
                            UnityEngine.GameObject parent = null;

                            if (UnityEngine.Camera.main != null)
                            {
                                parent = UnityEngine.Camera.main.gameObject;
                            }

                            if (parent == null)
                            {
                                parent = new UnityEngine.GameObject("Desktop Event Pump");
                            }

                            // Add the script to the main camera, which should exist in every scene
                            EventPump.Instance = parent.AddComponent<Helper.EventPump>();
                        }
                    }
                }
            }
            catch
            {
                UnityEngine.Debug.LogError("Events must be registered on the main thread.");
                return;
            }
        }

        private void Update()
        {
            lock (m_Queue)
            {
                while (m_Queue.Count > 0)
                {
                    var action = m_Queue.Dequeue();
                    action.Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            lock (m_Queue)
            {
                m_Queue.Enqueue(action);
            }
        }
    }
}