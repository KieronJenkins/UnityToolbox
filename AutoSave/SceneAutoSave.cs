using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Creates a ScriptableObject that can be configured to autosave every x amount of minutes
/// </summary>

namespace AutoSaving
{
    [CustomEditor(typeof(AutoSaveConfig))]
    public class SceneAutoSave : Editor
    {
        private static AutoSaveConfig _autoSaveConfig;
        private static List<string> _autoSaveConfigPath;
        private static string _getPathString;
        private static Task _asyncTask;
        private static bool _autoSaveEnabled;

        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            CheckForConfig();
            Debug.Log("AutoSave Initialized...");
            _asyncTask = RunAutoSaving();
        }

        [MenuItem("Tools/AutoSave/AutoSave Config")]
        public static void FindConfig()
        {
            CheckForConfig();
            _getPathString = CheckConfigPath();
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<AutoSaveConfig>(_getPathString).GetInstanceID());
        }

        [MenuItem("Tools/AutoSave/Reset Config")]
        public static void SetupConfig()
        {
            OnInitialize();
            Debug.Log("Resetting AutoSave...");
        }

        private static string CheckConfigPath()
        {
            _autoSaveConfigPath = AssetDatabase.FindAssets(nameof(AutoSaveConfig)).Select(AssetDatabase.GUIDToAssetPath)
                .Where(c => c.EndsWith(".asset")).ToList();
            return _autoSaveConfigPath.FirstOrDefault();
        }

        private static void CheckForConfig()
        {
            _getPathString = CheckConfigPath();
            _autoSaveConfig = AssetDatabase.LoadAssetAtPath<AutoSaveConfig>(_getPathString);

            if (_autoSaveConfig != null)
            {
                _autoSaveEnabled = true;
            }
            else
            {
                AssetDatabase.CreateAsset(CreateInstance<AutoSaveConfig>(),
                    $"Assets/Scripts/Editor/AutoSave/{nameof(AutoSaveConfig)}.asset");
                Debug.Log(
                    "<color=green>Success:</color>: AutoSave Config Created. Enabled by Default.\n If You Do Not See Any AutoSaving Please Enter Playmode or Restart Unity.");
                _autoSaveEnabled = true;
            }
        }

        private static async Task RunAutoSaving()
        {
            while (_autoSaveEnabled == true)
            {
                await Task.Delay(3000); // To Avoid Initial Errors on Unity Start
                try
                {
                    await Task.Delay(_autoSaveConfig._autoSaveInterval * 1000 * 60);
                    if (!Application.isPlaying && !EditorApplication.isCompiling)
                    {
                        EditorSceneManager.SaveOpenScenes();
                        Debug.Log($"<color=green>Success:</color> Scene AutoSaved At {DateTime.Now:h:mm:ss tt}");
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Error AutoSaving: Scene Not Saved, Please Check The Config.");
                    return;
                }
            }
        }
    }
}