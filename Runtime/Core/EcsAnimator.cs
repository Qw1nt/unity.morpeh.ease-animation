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
        private readonly AnimatorSlice[] _slices;

        internal EcsAnimator(Animator unityAnimator, EcsAnimatorData animatorData)
        {
            _unityAnimator = unityAnimator;
            _animatorData = animatorData;
            _slices = new AnimatorSlice[_unityAnimator.layerCount];

            Init();
        }
        
        // TODO убрать | добавить изменение весов слоёв 
        public Animator UnityAnimator => _unityAnimator;

        public EcsAnimatorData Data => _animatorData;

        private bool _forcePlay;

        private void Init()
        {
            if (_unityAnimator is null)
                throw new NullReferenceException("Unity animator not installed");

            if (_animatorData is null)
                throw new NullReferenceException("Ecs animator data not set");

            _unityAnimator.runtimeAnimatorController = _animatorData.AnimatorController;

            for (int i = 0; i < _slices.Length; i++)
                _slices[i] = new AnimatorSlice(_animatorData.Animations, _unityAnimator, i);
        }

        public HashedEcsAnimation GetAnimation(string animationName, int layerIndex = 0)
        {
            return _slices[layerIndex].GetHashedAnimation(animationName);
        }

        internal EcsAnimator SetAnimation(string animationName, int index = 0)
        {
            _slices[index].TrySetAnimation(animationName);
            return this;
        }   
        
        internal EcsAnimator SetLockedAnimation(string animationName, int index = 0)
        {
            _slices[index].SetLocked(animationName);
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
        
        internal void Play()
        {
            foreach (var slice in _slices)
                slice.TryPlay(_unityAnimator);
        }
    }
}