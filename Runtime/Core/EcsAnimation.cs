using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Serializable]
    public class EcsAnimation
    {
        [SerializeField] private string _name;
        [SerializeField] private int _priority;

        [Space] [SerializeField] private float _transitionDuration;
        [FormerlySerializedAs("_animationType")] [SerializeField] private AnimationType _type;
        [SerializeField] private AnimationClip _animationClip;

        [Space] [SerializeField] private LayerSettings _layerSettings;

        public EcsAnimation()
        {
        }

        public EcsAnimation(
            string name,
            int priority,
            float transitionDuration,
            AnimationType type,
            AnimationClip animationClip,
            LayerSettings layerSettings
        )
        {
            _name = name;
            _priority = priority;
            _transitionDuration = transitionDuration;
            _type = type;
            _animationClip = animationClip;
            _layerSettings = layerSettings;
        }

        public string Name => _name;

        public int Priority => _priority;

        public float TransitionDuration => _transitionDuration;

        public AnimationType Type => _type;
        
        public AnimationClip AnimationClip => _animationClip;

        public ref LayerSettings LayerSettings => ref _layerSettings;
    }
}