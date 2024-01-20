using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class AnimatorSlice : IComponent
    {
        internal AnimatorSlice()
        {
            
        }
        
        private HashedEcsAnimation PlayableAnimation { get; set; }

        private EcsAnimationBuffer EcsAnimationBuffer { get; } = new();

        public IEcsAnimationBuffer AnimationBuffer => EcsAnimationBuffer;
    }
}