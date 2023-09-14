using Qw1nt.Morpeh.EaseAnimation.Runtime.Components;
using Scellecs.Morpeh;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Systems
{
    internal class DelInitRequestSystem : ICleanupSystem
    {
        private Filter _filter;
        private Stash<InitEcsAnimatorRequest> _stash;

        public World World { get; set; }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<InitEcsAnimatorRequest>()
                .Build();

            _stash = World.GetStash<InitEcsAnimatorRequest>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                _stash.Remove(entity);
            }
        }
        
        public void Dispose()
        {
            
        }
    }
}