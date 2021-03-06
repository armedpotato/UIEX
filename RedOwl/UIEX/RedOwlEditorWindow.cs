﻿using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace RedOwl.Editor
{
    public enum WindowDockStyles
    {
        DontChange = 0,
        Floating,
        Inspector,
        Docked,
    }
    
    [USS("RedOwl/Editor/Styles")]//, USSClass("container", "column")]
    public abstract class RedOwlEditorWindow<T> : EditorWindow where T : RedOwlEditorWindow<T>
    {
        private static T _instance;
        public static T instance {
            get {
                if (_instance == null) EnsureWindow(WindowDockStyles.DontChange);
                return _instance;
            }
        }
        
        protected static T EnsureWindow(WindowDockStyles dockStyle = WindowDockStyles.Docked, int width = 600, int height = 400)
        {
            if (_instance == null)
            {
                switch (dockStyle)
                {
                case WindowDockStyles.DontChange:
                    _instance = GetWindow<T>();
                    break;
                case WindowDockStyles.Floating:
                    _instance = GetWindow<T>(true);
                    _instance.position = new Rect(Screen.currentResolution.width / 2 - (width / 2), Screen.currentResolution.height / 2 - (height / 2), width, height);
                    break;
                case WindowDockStyles.Inspector:
                    _instance = GetWindow<T>(typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow"));
                    break;
                default:
                case WindowDockStyles.Docked:
                    _instance = GetWindow<T>(typeof(SceneView));
                    break;
                }
                if (_instance != null)
                {
                    _instance.titleContent = new GUIContent(instance.GetWindowTitle());
                    _instance.Repaint();
                }
            }
            return _instance;
        }
        
        public abstract string GetWindowTitle();
        
        public bool IsInitalized { get; protected set; }
        
        protected virtual void OnEnable()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            if (IsInitalized) return;
            rootVisualElement.RegisterCallback<MouseEnterEvent>(e => { instance.Focus(); });
            RedOwlUtils.Setup(this, rootVisualElement);
            BuildUI();
            IsInitalized = true;
        }

        protected virtual void OnDisable()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        }

        public void OnBeforeAssemblyReload()
        {
            IsInitalized = false;
        }
        
        protected virtual void BuildUI() {}
    }
}
