using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;
using Scellecs.Morpeh;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Components
{
    public struct InitEcsAnimatorRequest : IComponent
    {
        public Animator UnityAnimator;
        public EcsAnimatorData EcsAnimatorData;
    }
}