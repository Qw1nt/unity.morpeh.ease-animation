using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    public class EcsAnimatorData : ScriptableObject
    {
        [SerializeField] private RuntimeAnimatorController _animatorController;

        [Space] [SerializeField] private string _initialAnimationName;
        [SerializeField] private List<EcsAnimation> _animations;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var serializedObject = new SerializedObject(this);
            var animations = serializedObject.FindProperty("_animations");
            for (int i = 0; i < animations.arraySize; i++)
            {
                var animation = animations.GetArrayElementAtIndex(i);
                var nameProperty = animation.FindPropertyRelative("_name");

                if (string.IsNullOrEmpty(nameProperty.stringValue) == false)
                    continue;

                // var clip = (AnimationClip) animation.FindPropertyRelative("_animationClip").objectReferenceValue;
                nameProperty.stringValue = animation.FindPropertyRelative("_animationClip").objectReferenceValue.name;
            }

            serializedObject.ApplyModifiedProperties();
        }
#endif

        public RuntimeAnimatorController AnimatorController => _animatorController;

        public string InitialAnimationName => _initialAnimationName;

        public IReadOnlyList<EcsAnimation> Animations => _animations;
    }
}