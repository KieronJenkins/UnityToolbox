using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InitialFolderSetup : EditorWindow
{
    private readonly string[] _standardFolderNames =
    {
        "Models", "StaticPrefabs", "DynamicPrefabs", "Scripts", "Shaders", "Particles", "Animations", "Editor",
        "Textures", "Materials", "VFX", "Sounds"
    };

    private readonly bool[] _standardFolderToggle = Enumerable.Repeat(true, 12).ToArray();
    private bool _createFolders;
    private bool _customDir;
    
    private string _dirPath = string.Empty;
    private string _folderName = string.Empty;
    private string _fullDirPath;
    private string _rootDirPath;

    [MenuItem("Window/Folder Setup")]
    private static void ShowWindow()
    {
        GetWindow<InitialFolderSetup>("Folder Setup Window");
    }
    
    private void OnGUI()
    {
        minSize = new Vector2(400, 320);
        maxSize = minSize;
        EditorGUILayout.Space(5);
        
        EditorGUILayout.BeginHorizontal();
        _createFolders = EditorGUILayout.Toggle("Standard Folder Names:", _createFolders);
        _customDir = EditorGUILayout.Toggle("Custom Directory:", _customDir);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        if (_customDir)
        {
            _dirPath = EditorGUILayout.TextField("Directory Path:", _dirPath);
        }
        else
        {
            _folderName = EditorGUILayout.TextField("Folder Name:", _folderName);
        }

        if (_createFolders)
            for (var i = 0; i < _standardFolderNames.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                _standardFolderNames[i] =
                    EditorGUILayout.TextField(_dirPath + "/" + _standardFolderNames[i], _standardFolderNames[i]);
                _standardFolderToggle[i] = EditorGUILayout.Toggle(_standardFolderToggle[i]);
                EditorGUILayout.EndHorizontal();
            }
        
        EditorGUILayout.Space(5);
        if (GUILayout.Button("Create"))
        {
            GenerateNewFolders();
        }
    }

    private void GenerateNewFolders()
    {
        if (_customDir)
        {
            _rootDirPath = Application.dataPath + "/" + _dirPath;
            _folderName = string.Empty;
        }
        else
        {
            _rootDirPath = Application.dataPath;
        }

        for (var i = 0; i < _standardFolderNames.Length; i++)
        {
            if (!_standardFolderToggle[i]) continue;

            string _folderName;
            if (_createFolders)
                _folderName = _standardFolderNames[i];
            else
                _folderName = this._folderName;

            var _fullPath = _rootDirPath + "/" + _folderName;

            try
            {
                Directory.CreateDirectory(_fullPath);
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Error", "Error creating folder " + _folderName + ": " + e.Message, "OK");
            }
        }
    }
}