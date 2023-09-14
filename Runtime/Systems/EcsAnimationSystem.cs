using Qw1nt.Morpeh.EaseAnimation.Runtime.Components;
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
                ref var component = ref entity.GetComponent<EcsAnimatorHandler>();
                var ecsAnimator = component.Source;

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