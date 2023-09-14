using Qw1nt.Morpeh.EaseAnimation.Runtime.Components;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;
using Scellecs.Morpeh;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Systems
{
    internal class InitEcsAnimatorSystem : ISystem
    {
        private Filter _filter;
        private Stash<InitEcsAnimatorRequest> _requestStash;

        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InitEcsAnimatorRequest>()
                .With<EcsAnimatorHandler>()
                .Build();
            _requestStash = World.GetStash<InitEcsAnimatorRequest>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var request = ref _requestStash.Get(entity);
                ref var animator = ref entity.GetComponent<EcsAnimatorHandler>();
                animator.Source = new EcsAnimator(request.UnityAnimator, request.EcsAnimatorData);
                
                EcsAnimatorContainer.Instance.Register(entity, animator.Source);
                _requestStash.Remove(entity);
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}