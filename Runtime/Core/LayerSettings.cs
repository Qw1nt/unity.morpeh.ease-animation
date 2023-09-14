using System;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Serializable]
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

        public static LayerSettings Null => new LayerSettings(-1, -1f);
    }
}