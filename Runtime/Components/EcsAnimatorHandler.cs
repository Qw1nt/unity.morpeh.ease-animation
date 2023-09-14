using System;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;
using Scellecs.Morpeh;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Components
{
    [Serializable]
    public struct EcsAnimatorHandler : IComponent
    {
        public EcsAnimator Source;
    }
}