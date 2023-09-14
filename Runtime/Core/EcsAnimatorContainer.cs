using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qw1nt.Morpeh.EaseAnimation.Runtime.Core
{
    public class EcsAnimatorContainer
    {
        private static EcsAnimatorContainer _instance;

        private EcsAnimatorContainer()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private Dictionary<int, EcsAnimator> Data { get; } = new();

        public static EcsAnimatorContainer Instance => _instance ??= new EcsAnimatorContainer();

        public void Register(Entity entity, EcsAnimator animator)
        {
            Data.Add(entity.ID.GetHashCode(), animator);
        }

        public EcsAnimator SetAnimation(int entity, string animationName)
        {
            return Get(entity).SetAnimation(animationName);
        }
        
        public EcsAnimator Get(int entity)
        {
            return Data[entity];
        }

        private void OnSceneChanged(Scene scene, Scene mode)
        {
                Debug.Log($"From {scene.buildIndex} to {mode.name}");
            Data.Clear();
        }
    }
}