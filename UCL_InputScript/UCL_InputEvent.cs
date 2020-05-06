using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.InputLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_InputEvent : MonoBehaviour {
        [System.Serializable]
        public struct KeyDownEvent {
            public KeyCode m_Key;
            public UnityEngine.Events.UnityEvent m_Events;
#if UNITY_EDITOR
            [Core.PA.UCL_Button("InvokeEvents")] public bool m_InvokeEvent;
#endif
            public void InvokeEvents() {
                m_Events?.Invoke();
            }
        }
        public List<KeyDownEvent> m_KeyDownEvents;
        virtual protected void CheckInput() {
            if(m_KeyDownEvents == null) return;
            foreach(var e in m_KeyDownEvents) {
                if(Input.GetKeyDown(e.m_Key)) {
                    e.InvokeEvents();
                }
            }
        }
        void Update() {
            CheckInput();
        }
    }
}

