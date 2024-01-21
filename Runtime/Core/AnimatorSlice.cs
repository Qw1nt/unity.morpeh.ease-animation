using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal class AnimatorSlice : IComponent
    {
        private readonly AnimationHashMap _animationHashMap;
        private readonly Animator _unityAnimator;
        private readonly int _sliceIndex;
        
        private bool _isLocked;
        private bool _isAlreadyLocked = false;
        private float _lockDuration;

        internal AnimatorSlice(IReadOnlyList<EcsAnimation> animations, Animator unityAnimator, int sliceIndex)
        {
            _animationHashMap = new AnimationHashMap(animations, sliceIndex);
            _unityAnimator = unityAnimator;
            _sliceIndex = sliceIndex;
        }

        private HashedEcsAnimation PlayableAnimation { get; set; }

        private EcsAnimationBuffer EcsAnimationBuffer { get; } = new();
        
        public HashedEcsAnimation GetHashedAnimation(string key)
        {
            return _animationHashMap[key];
        }

        public bool TrySetAnimation(string key)
        {
            var filledAnimation = _animationHashMap[key];

            if (filledAnimation <= EcsAnimationBuffer.PlayableAnimation)
                return false;

            EcsAnimationBuffer?.Fill(filledAnimation);
            return true;
        }

        public void SetLocked(string key)
        {
            if (TrySetAnimation(key) == false)
                return;

            var animation = _animationHashMap[key];

            _isLocked = true;
            _lockDuration = animation.ClipDuration;

            EcsAnimationBuffer.Fill(animation);
        }

        public void TryPlay(Animator animator)
        {
            if(_isAlreadyLocked == true)
                return;
            
            if (PlayableAnimation == EcsAnimationBuffer.PlayableAnimation)
            {
                EcsAnimationBuffer.Clear();
                return;
            }

            var animation = EcsAnimationBuffer.PlayableAnimation;

            if (animation == null)
                return;

            // if (PlayableAnimation == animation)
                // animator.Play(animation.Hash, -1, 0f);
                animator.Play(animation.Hash, _sliceIndex, 0f);
            // else
                // animator.CrossFade(animation.Hash, animation.TransitionDuration, animation.LayerSettings.Index);

            ApplyLayerSettings(animation, animator);
            PlayableAnimation = animation;

            if (_isLocked == true)
                Lock().Forget();
        }

        private async UniTaskVoid Lock()
        {
            _isLocked = false;
            _isAlreadyLocked = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_lockDuration));
            
            _isAlreadyLocked = false;
            _lockDuration = 0f;
            _unityAnimator.SetLayerWeight(_sliceIndex, 0f);

            PlayableAnimation = null;
            EcsAnimationBuffer.Clear();
            
        }

        private void ApplyLayerSettings(HashedEcsAnimation animation, Animator animator)
        {
            if (PlayableAnimation?.LayerSettings.Index != animation.LayerSettings.Index)
                PlayableAnimation?.LayerSettings.Reset(animator);

            animator.SetLayerWeight(animation.LayerSettings.Index, animation.LayerSettings.Weight);
        }
    }
}