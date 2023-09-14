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
                var hash = Animator.StringToHash(animation.AnimationClip.name);
                Add(animation.Name, new HashedEcsAnimation(hash, animation));
            }
        }
    }
}