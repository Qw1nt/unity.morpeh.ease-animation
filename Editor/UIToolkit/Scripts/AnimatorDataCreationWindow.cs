using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Qw1nt.Morpeh.EaseAnimation.Runtime.Core;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Qw1nt.Morpeh.EaseAnimation.Editor.UIToolkit.CreationWindow
{
    public class AnimatorDataCreationWindow : EditorWindow
    {
        private const string TreeLoadKey = "AnimatorDataCreationWindow";
        private const string AnimationPreviewKey = "CreationAnimationPreview";

        private readonly List<EcsAnimation> _animations = new();

        private VisualTreeAsset _tree;
        private VisualTreeAsset _animationPreview;
        private Elements _elements;
        private CreateAnimationElements _createElements;

        [MenuItem("Window/Ecs Animation System/Create Animator Data")]
        public static void Open()
        {
            var window = GetWindow<AnimatorDataCreationWindow>();

            window.minSize = new Vector2(100f, 100f);
            window.titleContent = new GUIContent("Create Animator Data");
            window._tree = Resources.Load<VisualTreeAsset>(TreeLoadKey);
            window._animationPreview = Resources.Load<VisualTreeAsset>(AnimationPreviewKey);
        }

        public void CreateGUI()
        {
            _tree = Resources.Load<VisualTreeAsset>(TreeLoadKey);
            _animationPreview = Resources.Load<VisualTreeAsset>(AnimationPreviewKey);

            _tree.CloneTree(rootVisualElement);
            _elements = new Elements(rootVisualElement);
            _createElements = new CreateAnimationElements(rootVisualElement);

            _createElements.AddAnimationButton.clicked += AddAnimation;
            _elements.CreateAnimationParent.SetEnabled(false);

            _elements.SourceAnimator.RegisterValueChangedCallback(InitAnimator);
            _createElements.ClipReference.SetEnabled(false);
            _createElements.ClipName.RegisterValueChangedCallback(callback =>
            {
                var animatorController = (AnimatorController) _elements.SourceAnimator.value;
                var animation = animatorController.animationClips
                    .FirstOrDefault(x => x.name == callback.newValue);

                if (animation is null)
                    return;

                _createElements.ClipReference.value = animation;
            });

            _elements.SaveAssetButton.clicked += Create;
        }

        private void InitAnimator(ChangeEvent<Object> callback)
        {
            if (callback.newValue is not AnimatorController animator)
            {
                _elements.CreateAnimationParent.SetEnabled(false);
                return;
            }

            var clips = new List<string>(animator.animationClips.Length);
            clips.AddRange(animator.animationClips.Select(clip => clip.name));
            _createElements.ClipName.choices = clips;
            _elements.InitialAnimationDropdown.choices = new List<string>();
            _elements.CreateAnimationParent.SetEnabled(true);
        }

        private void AddAnimation()
        {
            if (StringIsEmpty(_createElements.AnimationKey.value, "Animation key field is empty") == true)
                return;

            if (StringIsEmpty(_createElements.ClipName.value, "Clip name is not select") == true)
                return;

            VisualElement element = _animationPreview.CloneTree();
            var ecsAnimation = SetupAddedAnimation(element);

            _createElements.AnimationKey.value = "";
            _createElements.ClipName.index = 1;
            _createElements.ClipReference.value = null;
            _createElements.ClipName.choices.Remove(ecsAnimation.AnimationClip.name);
            _elements.InitialAnimationDropdown.choices.Add(ecsAnimation.Name);

            _animations.Add(ecsAnimation);
            _elements.ScrollView.Add(element);
        }

        private EcsAnimation SetupAddedAnimation(VisualElement element)
        {
            var clip = (AnimationClip) _createElements.ClipReference.value;

            var animationClipField = element.Q<ObjectField>("AnimationClip");
            animationClipField.value = clip;
            animationClipField.SetEnabled(false);

            var ecsAnimation = _createElements.BuildEcsAnimation(_animations.Count + 1);
            element.Q<Button>("SelectAnimationClipButton").clicked += () =>
            {
                Selection.SetActiveObjectWithContext(animationClipField.value, null);
            };

            element.Q<Label>("AnimationKey").text = ecsAnimation.Name;
            element.Q<Label>("TransitionDuration").text = ecsAnimation.TransitionDuration.ToString(CultureInfo.InvariantCulture);
            element.Q<Label>("ClipName").text = ecsAnimation.AnimationClip.name;
            element.Q<Button>("DeleteAnimationButton").clicked += () =>
            {
                _animations.Remove(ecsAnimation);
                _elements.ScrollView.Remove(element);
                _createElements.ClipName.choices.Add(ecsAnimation.AnimationClip.name);
                _elements.InitialAnimationDropdown.choices.Remove(ecsAnimation.Name);
            };

            return ecsAnimation;
        }

        private void Create()
        {
            var assetPath = CreateEmptyAsset();
            if (string.IsNullOrEmpty(assetPath) == true)
                return;

            SetupAsset(assetPath);
        }

        private string CreateEmptyAsset()
        {
            string sourcePath = _elements.SavePath.value;
            string assetName = _elements.AssetName.value;

            if (StringIsEmpty(assetName, "Asset name is empty") == true)
                return null;

            string savePath = Path.Combine("Assets", sourcePath);
            string assetPath = Path.Combine(savePath, assetName);
            assetPath = Path.ChangeExtension(assetPath, "asset");

            if (Directory.Exists(savePath) == false)
            {
                EditorUtility.DisplayDialog("Error", savePath, "Ok");
                return null;
            }

            AssetDatabase.CreateAsset(CreateInstance<EcsAnimatorData>(), assetPath);
            return assetPath;
        }

        private void SetupAsset(string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<EcsAnimatorData>(assetPath);
            SerializedObject serializedObject = new SerializedObject(asset);
            serializedObject.FindProperty("_animatorController").objectReferenceValue = _elements.SourceAnimator.value;
            var animationsListProperty = serializedObject.FindProperty("_animations");

            animationsListProperty.arraySize = _animations.Count;

            for (int i = 0; i < animationsListProperty.arraySize; i++)
            {
                var arrayElement = animationsListProperty.GetArrayElementAtIndex(i);
                var ecsAnimation = _animations[i];

                arrayElement.FindPropertyRelative("_name").stringValue = ecsAnimation.Name;
                arrayElement.FindPropertyRelative("_priority").intValue = ecsAnimation.Priority;
                arrayElement.FindPropertyRelative("_transitionDuration").floatValue = ecsAnimation.TransitionDuration;
                arrayElement.FindPropertyRelative("_animationClip").objectReferenceValue = ecsAnimation.AnimationClip;
            }

            serializedObject.ApplyModifiedProperties();

            AssetDatabase.SaveAssets();

            _elements.AssetName.value = "";
            _elements.SavePath.value = "";
            _elements.ScrollView.Clear();
        }

        private bool StringIsEmpty(string value, string errorMessage)
        {
            if (string.IsNullOrEmpty(value) == false)
                return false;

            EditorUtility.DisplayDialog("ERROR", errorMessage, "Ok");
            return true;
        }

        private class CreateAnimationElements
        {
            public CreateAnimationElements(VisualElement root)
            {
                AnimationKey = root.Q<TextField>("AnimationKey");
                TransitionDuration = root.Q<FloatField>("TransitionDurationField");
                AnimationType = root.Q<EnumField>("AnimationType");
                ClipName = root.Q<DropdownField>("AnimationClipsDropdown");
                ClipReference = root.Q<ObjectField>("ReferenceClipField");
                LayerIndex = root.Q<IntegerField>("LayerIndex");
                LayerWeight = root.Q<Slider>("LayerWeight");
                AddAnimationButton = root.Q<Button>("AddAnimationButton");
            }

            public TextField AnimationKey { get; }

            public FloatField TransitionDuration { get; }

            public EnumField AnimationType { get; }
            
            public DropdownField ClipName { get; }

            public ObjectField ClipReference { get; }

            public IntegerField LayerIndex { get; }

            public Slider LayerWeight { get; }

            public Button AddAnimationButton { get; }

            public EcsAnimation BuildEcsAnimation(int animationPriority)
            {
                var clip = (AnimationClip) ClipReference.value;

                return new EcsAnimation(
                    AnimationKey.value,
                    animationPriority,
                    TransitionDuration.value,
                    Enum.Parse<AnimationType>(AnimationType.text),
                    clip,
                    GenerateLayerSettings()
                );
            }

            private LayerSettings GenerateLayerSettings()
            {
                return new LayerSettings(LayerIndex.value, LayerWeight.value);
            }
        }

        private class Elements
        {
            public Elements(VisualElement root)
            {
                SavePath = root.Q<TextField>("SavePathField");
                AssetName = root.Q<TextField>("AssetNameField");
                SourceAnimator = root.Q<ObjectField>("SourceAnimatorField");
                CreateAnimationParent = root.Q<VisualElement>("CreateAnimationFields");
                InitialAnimationDropdown = root.Q<DropdownField>("InitialAnimationDropdown");
                ScrollView = root.Q<ScrollView>("PreviewAnimationList");
                SaveAssetButton = root.Q<Button>("SaveAssetButton");
            }

            public TextField SavePath { get; }

            public TextField AssetName { get; }

            public ObjectField SourceAnimator { get; }

            public VisualElement CreateAnimationParent { get; }

            public DropdownField InitialAnimationDropdown { get; }

            public ScrollView ScrollView { get; }

            public Button SaveAssetButton { get; }
        }
    }
}