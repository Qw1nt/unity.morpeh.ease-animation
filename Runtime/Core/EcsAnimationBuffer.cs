using System;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    public class EcsAnimationBuffer : IEcsAnimationBuffer
    {
        private HashedEcsAnimation InitialAnimation { get; set; } = null;

        private HashedEcsAnimation LoopAnimation { get; set; } = null;

        public HashedEcsAnimation PlayableAnimation => LoopAnimation == null ? InitialAnimation : LoopAnimation;

        public void SetInitial(HashedEcsAnimation animation)
        {
            if (InitialAnimation != null)
                throw new InvalidOperationException();
            
            InitialAnimation = animation;
        }

        internal void Fill(HashedEcsAnimation animation)
        {
            LoopAnimation = animation;
        }

        public void Clear()
        {
            InitialAnimation = null;
            LoopAnimation = null;
        }
    }
}