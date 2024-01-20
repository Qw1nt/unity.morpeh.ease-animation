using System;
using System.Threading;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class EcsAnimator
    {
        private readonly Animator _unityAnimator;
        private readonly EcsAnimatorData _animatorData;

        private AnimationHashMap _animationHashMap;

        internal EcsAnimator(Animator unityAnimator, EcsAnimatorData animatorData)
        {
            _unityAnimator = unityAnimator;
            _animatorData = animatorData;

            Init();
        }

        // TODO убрать | добавить изменение весов слоёв 
        public Animator UnityAnimator => _unityAnimator;

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

        internal EcsAnimator SetAnimation(string animationName)
        {
            var filledAnimation = _animationHashMap[animationName];

            if (filledAnimation > EcsAnimationBuffer.PlayableAnimation)
                EcsAnimationBuffer?.Fill(filledAnimation);

            return this;
        }

        internal EcsAnimator SetFloat(string parameterName, float value)
        {
            _unityAnimator.SetFloat(parameterName, value);
            return this;
        }

        internal EcsAnimator SetFloat(string parameterName, float value, float dampTime, float deltaTime)
        {
            _unityAnimator.SetFloat(parameterName, value, dampTime, deltaTime);
            return this;
        }

        internal EcsAnimator SetInteger(string parameter, int value)
        {
            _unityAnimator.SetInteger(parameter, value);
            return this;
        }

        internal EcsAnimator SetBool(string parameterName, bool value)
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

            return EcsAnimationBuffer.PlayableAnimation?.Priority > PlayableAnimation?.Priority;
        }

        internal void Play()
        {
            var animation = EcsAnimationBuffer.PlayableAnimation;

            // Debug.Log(animation.LayerSettings.Index);

            if (PlayableAnimation == animation)
                _unityAnimator.Play(animation.Hash, -1, 0f);
            else
                _unityAnimator.CrossFade(animation.Hash, animation.TransitionDuration, animation.LayerSettings.Index);

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