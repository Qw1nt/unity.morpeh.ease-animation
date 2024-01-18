﻿using System;
using JetBrains.Annotations;
using Qw1nt._3rdParty.Qw1nt.unity.morpeh.ease_animation.Runtime.Plugins;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Components;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;
using Scellecs.Morpeh;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Extensions
{
    public static class EcsAnimatorExtensions
    {
        public static void SetInitialAnimation(this EcsAnimator animator)
        {
            var animation = animator.GetAnimation(animator.Data.InitialAnimationName);
            animator.AnimationBuffer.SetInitial(animation);
        }

        public static void SetAnimation(this Entity entity, string key)
        {
            var animator = entity.Animator();
            animator.SetAnimation(key);
        }

        public static EcsAnimator Animator(this Entity entity)
        {
            var stash = HandlerStorage.Instance.Stash;
            
            if (stash.Has(entity) == false)
                throw new ArgumentException($"{entity} does not contains {nameof(EcsAnimatorHandler)}");

            ref var handler = ref stash.Get(entity);
            handler.Source ??= new EcsAnimator(handler.Animator, handler.Data);
            return handler.Source;
        }

        [CanBeNull]
        public static EcsAnimator SetSafeFloat([CanBeNull] this EcsAnimator animator, string parameterName, float value)
        {
            return animator?.SetFloat(parameterName, value);
        }

        [CanBeNull]
        public static EcsAnimator SetSafeFloat([CanBeNull] this EcsAnimator animator, string parameterName, float value,
            float dampTime,
            float deltaTime)
        {
            return animator?.SetFloat(parameterName, value, dampTime, deltaTime);
        }

        [CanBeNull]
        public static EcsAnimator SetSafeXY([CanBeNull] this EcsAnimator animator, string xParameterName,
            string yParameterName, Vector2 value, float dampTime, float deltaTime)
        {
            animator?.SetFloat(xParameterName, value.x, dampTime, deltaTime);
            animator?.SetFloat(yParameterName, value.y, dampTime, deltaTime);
            return animator;
        }

        public static EcsAnimator SetSafeXZ(this EcsAnimator animator, string xParameterName, string zParameterName,
            Vector3 value, float dampTime, float deltaTime)
        {
            animator?.SetFloat(xParameterName, value.x, dampTime, deltaTime);
            animator?.SetFloat(zParameterName, value.z, dampTime, deltaTime);
            return animator;
        }

        [CanBeNull]
        public static EcsAnimator SetSafeInteger([CanBeNull] this EcsAnimator animator, string parameterName, int value)
        {
            return animator?.SetInteger(parameterName, value);
        }

        [CanBeNull]
        public static EcsAnimator SetSafeBool([CanBeNull] this EcsAnimator animator, string parameterName, bool value)
        {
            return animator?.SetBool(parameterName, value);
        }
    }
}