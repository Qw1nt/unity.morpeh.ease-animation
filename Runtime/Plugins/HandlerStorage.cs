using Qw1nt.Morpeh.EaseAnimation.Runtime.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Qw1nt._3rdParty.Qw1nt.unity.morpeh.ease_animation.Runtime.Plugins
{
    internal class HandlerStorage : IWorldPlugin
    {
        public Stash<EcsAnimatorHandler> Stash { get; private set; }

        private static HandlerStorage _instance;

        public static HandlerStorage Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var obj = new object();

                lock (obj)
                {
                    if (_instance != null)
                        return _instance;

                    return _instance = new HandlerStorage();
                }
            }
        }

        private HandlerStorage()
        {
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void RuntimeInitialize()
        {
            WorldExtensions.AddWorldPlugin(Instance);
        }

        public void Initialize(World world)
        {
            Stash = world.GetStash<EcsAnimatorHandler>();
            Debug.Log(Stash);
        }

        public void Deinitialize(World world)
        {
        }
    }
}