using System;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Components
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsAnimatorHandler : IComponent
    {
        public EcsAnimator Source;
    }
}