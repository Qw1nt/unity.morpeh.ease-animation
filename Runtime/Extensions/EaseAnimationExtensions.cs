﻿using Qw1nt.Morpeh.EaseAnimation.Runtime.Systems;
using Scellecs.Morpeh;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Extensions
{
    public static class EaseAnimationExtensions
    {
        public static void AddAnimationSystem(this World world, int order)
        {
            var group = world.CreateSystemsGroup();
            group.AddSystem(new SetInitialAnimationSystem());
            group.AddSystem(new EcsAnimationSystem());
            world.AddSystemsGroup(order, group);
        }

        public static void AddAnimationSystem(this SystemsGroup group)
        {
            group.AddSystem(new SetInitialAnimationSystem());
            group.AddSystem(new EcsAnimationSystem());
        }
    }
}