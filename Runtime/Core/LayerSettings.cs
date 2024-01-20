using System;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct LayerSettings
    {
        [SerializeField] private int _index;
        [SerializeField, Range(0f, 1f)] private float _weight;
        
        public LayerSettings(int index, float weight)
        {
            _index = index;
            _weight = weight;
        }

        public int Index => _index;

        public float Weight => _weight;

        public void Apply(Animator animator)
        {
            animator.SetLayerWeight(_index, _weight);
        }

        public void Reset(Animator animator)
        {
            animator.SetLayerWeight(_index, 0f);
        }

        public static bool operator ==(LayerSettings left, LayerSettings right)
        {
            return left._index == right._index && left._weight == right._weight;
        }

        public static bool operator !=(LayerSettings left, LayerSettings right)
        {
            return !(left == right);
        }

        public static LayerSettings Null => new LayerSettings(-1, -1f);
    }
}