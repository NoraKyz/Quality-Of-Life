using System;
using System.Linq;
using System.Reflection;
using Quality.Core.CustomAttribute;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quality.Editor.Quality.Quality.Editor.CustomAttribute
{
    [InitializeOnLoad]
    public static class SelfFillProcessor
    {
        static SelfFillProcessor()
        {
            ObjectFactory.componentWasAdded += OnComponentWasAdded;
        }

        [MenuItem("CONTEXT/Component/SelfFill Fields")]
        private static void FillContextMenu(MenuCommand command)
        {
            Fill(command.context as Component);
        }

        [MenuItem("Tools/CustomAttribute/Fill All SelfFill Fields in Project")]
        private static void FillAllInProject()
        {
            try
            {
                // Xử lý Prefabs
                var prefabGuids = AssetDatabase.FindAssets("t:Prefab");
                var totalPrefabs = prefabGuids.Length;
                for (var i = 0; i < totalPrefabs; i++)
                {
                    var path = AssetDatabase.GUIDToAssetPath(prefabGuids[i]);
                    if (EditorUtility.DisplayCancelableProgressBar("SelfFill in Project", $"Processing Prefabs: {path}", (float)i / totalPrefabs))
                    {
                        throw new OperationCanceledException("User cancelled the operation.");
                    }

                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab != null)
                    {
                        var components = prefab.GetComponentsInChildren<Component>(true);
                        foreach (var component in components)
                        {
                            Fill(component);
                        }
                    }
                }

                // Xử lý Scenes
                var originalScene = SceneManager.GetActiveScene().path;
                var scenePaths = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
                var totalScenes = scenePaths.Length;

                for (var i = 0; i < totalScenes; i++)
                {
                    var scenePath = scenePaths[i];
                    if (EditorUtility.DisplayCancelableProgressBar("SelfFill in Project", $"Processing Scenes: {scenePath}", (float)i / totalScenes))
                    {
                         throw new OperationCanceledException("User cancelled the operation.");
                    }

                    Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                    var rootObjects = scene.GetRootGameObjects();
                    foreach (var go in rootObjects)
                    {
                        var components = go.GetComponentsInChildren<Component>(true);
                        foreach (var component in components)
                        {
                            Fill(component);
                        }
                    }
                    EditorSceneManager.SaveScene(scene);
                }

                // Quay lại scene ban đầu nếu có
                if (!string.IsNullOrEmpty(originalScene))
                {
                    EditorSceneManager.OpenScene(originalScene);
                }

                Debug.Log("SelfFill All Fields in Project completed successfully!");
            }
            catch (OperationCanceledException)
            {
                Debug.Log("SelfFill operation was cancelled by the user.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred during SelfFill operation: {ex}");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                AssetDatabase.SaveAssets();
            }
        }

        private static void OnComponentWasAdded(Component component)
        {
            Fill(component);
        }

        public static void Fill(Component component)
        {
            if (component == null) return;

            Type componentType = component.GetType();
            FieldInfo[] fields =
                componentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<SelfFillAttribute>();
                if (attribute == null) continue;

                if (field.GetValue(component) != null) continue;

                var fieldType = field.FieldType;

                var isComponentOrInterface = typeof(Component).IsAssignableFrom(fieldType) || fieldType.IsInterface;

                object value = null;
                switch (attribute.Target)
                {
                    case SearchTarget.SELF:
                        if (isComponentOrInterface) value = component.GetComponent(fieldType);
                        break;
                    case SearchTarget.PARENT:
                        if (isComponentOrInterface) value = component.GetComponentInParent(fieldType, attribute.IncludeInactive);
                        break;
                    case SearchTarget.CHILDREN:
                        if (isComponentOrInterface) value = component.GetComponentInChildren(fieldType, attribute.IncludeInactive);
                        break;
                    case SearchTarget.SCENE:
                        if (isComponentOrInterface) value = FindObjectOfType(fieldType, attribute.IncludeInactive);
                        break;
                    case SearchTarget.RESOURCE:
                        if (!string.IsNullOrEmpty(attribute.Path))
                        {
                            value = Resources.Load(attribute.Path, fieldType);
                        }
                        break;
                }

                if (value != null)
                {
                    field.SetValue(component, value);
                    EditorUtility.SetDirty(component);
                }
            }
        }

        private static object FindObjectOfType(Type type, bool includeInactive)
        {
#if UNITY_2023_1_OR_NEWER
            return UnityEngine.Object.FindAnyObjectByType(type, includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);
#else
            return UnityEngine.Object.FindObjectOfType(type, includeInactive);
#endif
        }
    }
}
