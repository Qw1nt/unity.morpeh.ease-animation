using Qw1nt.Morpeh.EaseAnimation.Runtime.Components;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;
using Scellecs.Morpeh;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Systems
{
    internal class InitEcsAnimatorSystem : ISystem
    {
        private Filter _filter;

        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InitEcsAnimatorRequest>()
                .With<EcsAnimatorHandler>()
                .Build();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var request = ref entity.GetComponent<InitEcsAnimatorRequest>();
                ref var animator = ref entity.GetComponent<EcsAnimatorHandler>();
                animator.Source = new EcsAnimator(request.UnityAnimator, request.EcsAnimatorData);
                
                EcsAnimatorContainer.Instance.Register(entity, animator.Source);
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}