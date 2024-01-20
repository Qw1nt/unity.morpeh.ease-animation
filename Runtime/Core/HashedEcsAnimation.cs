using System;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class HashedEcsAnimation
    {
        public HashedEcsAnimation(int animationHash, EcsAnimation animation)
        {
            Name = animation.Name;
            Hash = animationHash;
            Priority = animation.Priority;
            TransitionDuration = animation.TransitionDuration;
            ClipDuration = animation.AnimationClip == null ? 0f : animation.AnimationClip.length;
            LayerSettings = animation.LayerSettings;
            Type = animation.Type;
        }

        public HashedEcsAnimation(string name, int hash, int priority, float transitionDuration, AnimationType type,
            float clipDuration, LayerSettings layerSettings)
        {
            Name = name;
            Hash = hash;
            Priority = priority;
            TransitionDuration = transitionDuration;
            Type = type;
            ClipDuration = clipDuration;
            LayerSettings = layerSettings;
        }

        public string Name { get; }

        public int Hash { get; }

        public int Priority { get; }

        public float TransitionDuration { get; }

        public AnimationType Type { get; }

        public float ClipDuration { get; }

        public LayerSettings LayerSettings { get; }

        public override string ToString()
        {
            return $"{Name} | {Priority}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj.GetType() == GetType()
                   && Equals((HashedEcsAnimation) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hash, TransitionDuration);
        }

        /*public static bool operator ==(HashedEcsAnimation left, HashedEcsAnimation right)
        {
            var leftIsNull = ReferenceEquals(null, left);
            var rightIsNull = ReferenceEquals(null, right);

            if (leftIsNull == true && rightIsNull == false || leftIsNull == false && rightIsNull == true)
                return false;

            return left?.Hash == right?.Hash &&
                   left?.Type == right?.Type &&
                   left?.LayerSettings != right?.LayerSettings;
        }

        public static bool operator !=(HashedEcsAnimation left, HashedEcsAnimation right)
        {
            var leftIsNull = ReferenceEquals(null, left);
            var rightIsNull = ReferenceEquals(null, right);

            if (leftIsNull == true && rightIsNull == true)
                return false;

            return left?.Hash != right?.Hash ||
                   left?.Type != right?.Type ||
                   left?.LayerSettings != right?.LayerSettings;
        }*/

        public static bool operator >(HashedEcsAnimation left, HashedEcsAnimation right)
        {
            return right == null;

            return left.Priority > right?.Priority;
        }

        public static bool operator <(HashedEcsAnimation left, HashedEcsAnimation right)
        {
            if (left == null)
                return true;

            if (right == null)
                return false;

            return left.Priority < right.Priority;
        }
    }
}