using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Extensions
{
    public static class EcsAnimatorExtensions
    {
        public static void SetInitialAnimation(this EcsAnimator animator)
        {
            var animation = animator.GetAnimation(animator.Data.InitialAnimationName);
            animator.AnimationBuffer.SetInitial(animation);
        } 
    }
}