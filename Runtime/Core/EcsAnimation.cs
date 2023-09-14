using System;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Serializable]
    public class EcsAnimation
    {
        [SerializeField] private string _name;
        [SerializeField] private int _priority;
        
        [Space]
        [SerializeField] private float _transitionDuration;
        [SerializeField] private AnimationClip _animationClip;
        
        [Space]
        [SerializeField] private LayerSettings _layerSettings;

        public EcsAnimation()
        {
        }

        public EcsAnimation(string name, int priority, float transitionDuration, AnimationClip animationClip, LayerSettings layerSettings)
        {
            _name = name;
            _priority = priority;
            _transitionDuration = transitionDuration;
            _animationClip = animationClip;
            _layerSettings = layerSettings;
        }

        public string Name => _name;

        public int Priority => _priority;

        public float TransitionDuration => _transitionDuration;

        public AnimationClip AnimationClip => _animationClip;

        public ref LayerSettings LayerSettings => ref _layerSettings;
    }
}