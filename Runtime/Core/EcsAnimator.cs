using System;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    public class EcsAnimator
    {
        private readonly Animator _unityAnimator;
        private readonly EcsAnimatorData _animatorData;

        public EcsAnimator(Animator unityAnimator, EcsAnimatorData animatorData)
        {
            _unityAnimator = unityAnimator;
            _animatorData = animatorData;
            Init();
        }

        private AnimationHashMap _animationHashMap;

        public EcsAnimatorData Data => _animatorData;

        private HashedEcsAnimation PlayableAnimation { get; set; }

        private EcsAnimationBuffer EcsAnimationBuffer { get; } = new();

        public IEcsAnimationBuffer AnimationBuffer => EcsAnimationBuffer;

        private bool _forcePlay;

        private void Init()
        {
            if (_unityAnimator is null)
                throw new NullReferenceException("Unity animator not installed");

            if (_animatorData is null)
                throw new NullReferenceException("Ecs animator data not set");

            _unityAnimator.runtimeAnimatorController = _animatorData.AnimatorController;
            _animationHashMap = new AnimationHashMap(_animatorData.Animations);
        }

        public HashedEcsAnimation GetAnimation(string animationName)
        {
            return _animationHashMap[animationName];
        }

        public EcsAnimator SetAnimation(string animationName)
        {
            var filledAnimation = _animationHashMap[animationName];

            if (filledAnimation?.Priority > EcsAnimationBuffer.PlayableAnimation?.Priority)
                EcsAnimationBuffer.Fill(filledAnimation);

            if (filledAnimation > EcsAnimationBuffer.PlayableAnimation)
                EcsAnimationBuffer?.Fill(filledAnimation);

            return this;
        }

        public EcsAnimator SetFloat(string parameterName, float value)
        {
            _unityAnimator.SetFloat(parameterName, value);
            return this;
        }    
        
        public EcsAnimator SetFloat(string parameterName, float value, float dampTime, float deltaTime)
        {
            _unityAnimator.SetFloat(parameterName, value, dampTime, deltaTime);
            return this;
        }

        public EcsAnimator SetInteger(string parameter, int value)
        {
            _unityAnimator.SetInteger(parameter, value);
            return this;
        }

        public EcsAnimator SetBool(string parameterName, bool value)
        {
            _unityAnimator.SetBool(parameterName, value);
            return this;
        }
        
        internal bool NeedPlayAnimation()
        {
            if (_forcePlay == true)
            {
                _forcePlay = false;
                return true;
            }

            if (PlayableAnimation != EcsAnimationBuffer.PlayableAnimation)
                return true;

            return EcsAnimationBuffer.PlayableAnimation.Priority > PlayableAnimation.Priority;
        }

        internal void Play()
        {
            var animation = EcsAnimationBuffer.PlayableAnimation;

            if (PlayableAnimation == animation)
                _unityAnimator.Play(animation.Hash, -1, 0f);
            else
                _unityAnimator.CrossFade(animation.Hash, animation.TransitionDuration);

            ApplyLayerSettings(animation);
            PlayableAnimation = animation;
        }

        private void ApplyLayerSettings(HashedEcsAnimation animation)
        {
            if (PlayableAnimation?.LayerSettings.Index != animation.LayerSettings.Index)
                PlayableAnimation?.LayerSettings.Reset(_unityAnimator);

            _unityAnimator.SetLayerWeight(animation.LayerSettings.Index, animation.LayerSettings.Weight);
        }
    }
}