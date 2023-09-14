namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    public interface IEcsAnimationBuffer
    {
        void SetInitial(HashedEcsAnimation ecsAnimation);

        void Clear();
    }
}