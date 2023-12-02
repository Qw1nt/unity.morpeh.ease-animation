using Qw1nt.Morpeh.EaseAnimation.Runtime.Components;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Extensions;
using Scellecs.Morpeh;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Systems
{
    internal class EcsAnimationSystem : ISystem
    {
        private Filter _filter;

        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<EcsAnimatorHandler>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var ecsAnimator = entity.GetAnimator();

                if (ecsAnimator.NeedPlayAnimation() == false)
                {
                    ecsAnimator.AnimationBuffer.Clear();
                    continue;
                }

                ecsAnimator.Play();
                ecsAnimator.AnimationBuffer.Clear();
            }
        }

        public void Dispose()
        {
        }
    }
}