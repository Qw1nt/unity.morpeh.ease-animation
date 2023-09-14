using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Qw1nt.Morpeh.EaseAnimation.Editor.UIToolkit
{
    public class AnimatorDataWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _tree;
        
        [MenuItem("Window/Ecs Animation System/Animator Data")]
        public static void Open()
        {
            var window = GetWindow<AnimatorDataWindow>();
            window.titleContent = new GUIContent("Animator Data");
        }

        public void CreateGUI()
        {
            _tree.CloneTree(rootVisualElement);
        }
    }
}