using System.Collections.Generic;
using UnityEngine;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    public class AnimationHashMap : Dictionary<string, HashedEcsAnimation>
    {
        public AnimationHashMap(IReadOnlyList<EcsAnimation> animations)
        {
            foreach (var animation in animations)
            {
                Add(animation.Name, new HashedEcsAnimation(GetAnimationHash(animation), animation));
            }
        }

        public AnimationHashMap(IReadOnlyList<EcsAnimation> animations, int layerIndex)
        {
            foreach (var animation in animations)
            {
                if (animation.LayerSettings.Index == layerIndex)
                    Add(animation.Name, new HashedEcsAnimation(GetAnimationHash(animation), animation));
            }
        }

        private int GetAnimationHash(EcsAnimation animation)
        {
            if (animation.Type == AnimationType.BlendTree)
                return Animator.StringToHash(animation.Name);

            var animationName = animation.AnimationClip != null
                ? animation.AnimationClip.name
                : animation.Name;

            return Animator.StringToHash(animationName);
        }
    }
}