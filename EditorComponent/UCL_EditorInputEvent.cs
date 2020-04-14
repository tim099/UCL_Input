using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.InputLib.Editor {
    /// <summary>
    /// Editor version of UCL_InputEvent
    /// Can work on both Editor and RunTime!!
    /// </summary>
    [ExecuteInEditMode]
    public class UCL_EditorInputEvent : UCL_InputEvent {
#if UNITY_EDITOR
        virtual protected void OnEnable() {
            if(f_UpdateInEditorMode) {
                Rigister();
            }
        }
        virtual protected void OnDisable() {
            UnRigister();
        }
        private void OnDestroy() {
            UnRigister();
        }
        public bool f_UpdateInEditorMode = false;
        //string m_RigisterKey;
        bool f_Rigisterd = false;
        HashSet<KeyCode> m_KeyDown;
        public void Rigister() {
            if(f_Rigisterd) return;
            
            if(!UnityEditor.EditorApplication.isPlaying) {
                
                f_Rigisterd = true;
                //m_RigisterKey =  "UCL_EditorInputEvent_"+ GetInstanceID();
                //Debug.LogWarning("UCL_EditorInputEvent Rigister():" + name+ ",RigisterKey:"+ m_RigisterKey);
                //UCL.Core.Editor.UCL_EditorUpdateManager.AddEditorUpdateAct(m_RigisterKey, EditorUpdate);
                UCL.Core.Editor.UCL_EditorUpdateManager.AddEditorUpdateAct(EditorUpdate);
                //UCL.Core.Editor.UCL_EditorUpdateManager.m_EditorUpdateEvent += EditorUpdate;
                //UnityEditor.EditorApplication.update += EditorUpdate;
                UnityEditor.SceneView.duringSceneGui += DuringSceneGui;
                m_KeyDown = new HashSet<KeyCode>();
            }
        }
        public void UnRigister() {
            if(!f_Rigisterd) return;

            //Debug.LogWarning("UCL_EditorInputEvent UnRigister():" + name + ",RigisterKey:" + m_RigisterKey);
            f_Rigisterd = false;
            //UnityEditor.EditorApplication.update -= EditorUpdate;
            //UCL.Core.Editor.UCL_EditorUpdateManager.m_EditorUpdateEvent -= EditorUpdate;
            //UCL.Core.Editor.UCL_EditorUpdateManager.RemoveEditorUpdateAct(m_RigisterKey);
            UCL.Core.Editor.UCL_EditorUpdateManager.RemoveEditorUpdateAct(EditorUpdate);
            UnityEditor.SceneView.duringSceneGui -= DuringSceneGui;
            m_KeyDown = null;
        }
        virtual public void EditorUpdate() {
            if(!f_UpdateInEditorMode) {
                if(f_Rigisterd) UnRigister();
            } else {
                if(!f_Rigisterd) Rigister();
            }
            //CheckInput();
        }
        public void DuringSceneGui(UnityEditor.SceneView view) {
            var e = Event.current;
            if(e != null && e.keyCode != KeyCode.None) {
                if(e.type == EventType.KeyUp) {
                    m_KeyDown.Remove(e.keyCode);
                    //Debug.Log("Key KeyUp in editor: " + e.keyCode);
                } else if(e.type == EventType.KeyDown) {
                    if(m_KeyDown.Contains(e.keyCode)) {
                        //Debug.Log("Key KeyPressed in editor: " + e.keyCode);
                    } else {
                        m_KeyDown.Add(e.keyCode);
                        foreach(var ev in m_KeyDownEvents) {
                            if(ev.m_Key == e.keyCode) {
                                ev.m_Events?.Invoke();
                                UnityEditor.Selection.activeTransform = transform;
                            }
                        }

                        Debug.Log("Key KeyDown in editor: " + e.keyCode);
                    }
                }


            }

        }
#endif
    }
}