using UnityEditor;
using UnityEngine;
using System.IO;

public class AutoSuffixAssets : AssetPostprocessor
{
    // =========================
    // ALLOWED FOLDERS
    // Only assets inside these folders will be renamed
    // =========================
    static readonly string[] AllowedFolders =
    {
        "Assets/Game/",
        "Assets/Scenes/",
        "Assets/Audio/"
    };

    static bool IsInAllowedFolder(string path)
    {
        foreach (var folder in AllowedFolders)
        {
            if (path.StartsWith(folder))
                return true;
        }

        return false;
    }

    // =========================
    // AUTO RENAME ON IMPORT
    // =========================
    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string path in importedAssets)
        {
            ApplySuffix(path);
        }
    }

    // =========================
    // MENU: TOOLS BAR
    // =========================
    [MenuItem("Tools/Auto Suffix/Rename Selected Folder")]
    static void RenameFolderAssetsMenu()
    {
        RenameSelectedFolder();
    }

    // =========================
    // RIGHT CLICK MENU
    // =========================
    [MenuItem("Assets/Auto Suffix/Rename Folder Assets")]
    static void RenameFolderAssetsRightClick()
    {
        RenameSelectedFolder();
    }

    // Show menu only for folders
    [MenuItem("Assets/Auto Suffix/Rename Folder Assets", true)]
    static bool ValidateRenameFolderAssets()
    {
        if (Selection.activeObject == null)
            return false;

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return AssetDatabase.IsValidFolder(path);
    }

    // =========================
    // FOLDER ITERATION
    // =========================
    static void RenameSelectedFolder()
    {
        Object selected = Selection.activeObject;

        if (selected == null)
        {
            Debug.LogWarning("Select a folder first.");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(selected);

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning("Selected object is not a folder.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (AssetDatabase.IsValidFolder(path))
                continue;

            ApplySuffix(path);
        }

        Debug.Log("Finished renaming folder assets.");
    }

    // =========================
    // CORE SUFFIX LOGIC
    // =========================
    static void ApplySuffix(string path)
    {
        if (!IsInAllowedFolder(path))
            return;

        string extension = Path.GetExtension(path).ToLower();
        string fileName = Path.GetFileNameWithoutExtension(path);

        string suffix = "";

        switch (extension)
        {
            // Materials
            case ".mat": suffix = "_mat"; break;

            // Textures
            case ".png":
            case ".jpg":
            case ".jpeg":
            case ".tga":
            case ".psd":
            case ".exr":
            case ".hdr":
                suffix = "_tex"; break;

            // Models
            case ".fbx":
            case ".obj":
            case ".blend":
                suffix = "_model"; break;

            // Prefabs
            case ".prefab": suffix = "_prefab"; break;

            // Scenes
            case ".unity": suffix = "_scene"; break;

            // Animations
            case ".anim": suffix = "_anim"; break;

            // Animator Controller
            case ".controller": suffix = "_ctrl"; break;

            // Shaders
            case ".shader":
            case ".shadergraph":
                suffix = "_shader"; break;

            // Scriptable Objects
            case ".asset": suffix = "_asset"; break;

            // Audio
            case ".wav":
            case ".mp3":
            case ".ogg":
            case ".aiff":
                suffix = "_audio"; break;

            // Video
            case ".mp4":
            case ".mov":
                suffix = "_video"; break;

            // Data files
            case ".txt":
            case ".json":
            case ".xml":
                suffix = "_data"; break;

            // Fonts
            case ".ttf":
            case ".otf":
                suffix = "_font"; break;

            // Timeline
            case ".playable":
                suffix = "_timeline"; break;

            // Lighting
            case ".lighting":
                suffix = "_lighting"; break;
        }

        if (string.IsNullOrEmpty(suffix)) return;
        if (fileName.EndsWith(suffix)) return;

        string newName = fileName + suffix;

        AssetDatabase.RenameAsset(path, newName);

        string newPath = Path.GetDirectoryName(path) + "/" + newName + extension;

        EditorApplication.delayCall += () =>
        {
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(newPath);
            if (obj != null)
            {
                obj.name = newName;
                EditorUtility.SetDirty(obj);
            }
        };
    }
}
