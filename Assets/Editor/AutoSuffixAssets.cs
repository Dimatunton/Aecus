using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

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

            //// Models
            //case ".fbx":
            //case ".obj":
            //case ".blend":
            //    suffix = "_model"; break;

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

        string oldPath = path;
        string ext = extension; // capture for closure

        // Delay rename until after Unity finishes import to avoid the "Main Object Name ... does not match filename" warning.
        EditorApplication.delayCall += () =>
        {
            // Ensure asset still exists
            Object mainBefore = AssetDatabase.LoadMainAssetAtPath(oldPath);
            if (mainBefore == null)
                return;

            // Directory
            string directory = Path.GetDirectoryName(oldPath).Replace("\\", "/");
            if (string.IsNullOrEmpty(directory))
                directory = "Assets";

            // Start from the incoming filename without extension
            string currentName = Path.GetFileNameWithoutExtension(oldPath);

            // Determine basePart and existingNumber robustly.
            // Handles:
            // - "Base"
            // - "Base 1"
            // - "Base_suffix"           -> suffix present at end
            // - "Base_suffix 1"         -> Unity duplicate pattern (suffix then number)
            // - "Base 1_suffix"         -> number before suffix
            string basePart;
            int existingNumber = 0;

            int suffixIdx = currentName.LastIndexOf(suffix, System.StringComparison.Ordinal);
            if (suffixIdx >= 0)
            {
                // There is a suffix somewhere in the name.
                string left = currentName.Substring(0, suffixIdx).TrimEnd();
                string right = currentName.Substring(suffixIdx + suffix.Length).Trim();

                if (Regex.IsMatch(right, @"^\d+$"))
                {
                    // Pattern: left + suffix + " " + duplicateNumber
                    // Example: "New Material 2_mat 1"
                    // We want to take the trailing number from 'left' (2) as the existingNumber.
                    var leftMatch = Regex.Match(left, @"^(.*?)(?:\s+(\d+))?$");
                    basePart = leftMatch.Success ? leftMatch.Groups[1].Value.TrimEnd() : left;
                    existingNumber = (leftMatch.Success && leftMatch.Groups[2].Success) ? int.Parse(leftMatch.Groups[2].Value) : 0;
                }
                else
                {
                    // Pattern: left + suffix  (maybe left contains trailing number)
                    // Example: "New Material 2_mat" or "New Material_mat"
                    var leftMatch = Regex.Match(left, @"^(.*?)(?:\s+(\d+))?$");
                    basePart = leftMatch.Success ? leftMatch.Groups[1].Value.TrimEnd() : left;
                    existingNumber = (leftMatch.Success && leftMatch.Groups[2].Success) ? int.Parse(leftMatch.Groups[2].Value) : 0;
                }
            }
            else
            {
                // No suffix present. The current name might include a trailing number.
                var m = Regex.Match(currentName, @"^(.*?)(?:\s+(\d+))?$");
                basePart = m.Success ? m.Groups[1].Value.TrimEnd() : currentName;
                existingNumber = (m.Success && m.Groups[2].Success) ? int.Parse(m.Groups[2].Value) : 0;
            }

            // Choose starting counter:
            // - if there is already a trailing number, start from existingNumber + 1
            // - otherwise start from 0 (meaning no number)
            int counter = existingNumber > 0 ? existingNumber + 1 : 0;
            const int maxAttempts = 1000;
            string candidateName = null;
            string candidatePath = null;

            // Try candidates until one is unused
            for (int attempt = 0; attempt <= maxAttempts; attempt++)
            {
                string counterPart = counter == 0 ? "" : " " + counter.ToString();
                candidateName = basePart + counterPart + suffix; // number before suffix
                candidatePath = directory + "/" + candidateName + ext;

                if (AssetDatabase.LoadMainAssetAtPath(candidatePath) == null)
                    break;

                counter++;
            }

            if (string.IsNullOrEmpty(candidatePath) || AssetDatabase.LoadMainAssetAtPath(candidatePath) != null)
            {
                Debug.LogError($"AutoSuffix: failed to find unique name for '{oldPath}' after {maxAttempts} attempts.");
                return;
            }

            // Attempt rename to the chosen candidate (pass only filename without extension)
            string candidateFileNameWithoutExt = Path.GetFileNameWithoutExtension(candidatePath);
            string renameError = AssetDatabase.RenameAsset(oldPath, candidateFileNameWithoutExt);
            if (!string.IsNullOrEmpty(renameError))
            {
                Debug.LogError($"Failed to rename asset '{oldPath}' to '{candidateFileNameWithoutExt}': {renameError}");
                return;
            }

            // Load the asset at the final path and make sure its main object's name matches the filename
            Object mainAfter = AssetDatabase.LoadMainAssetAtPath(candidatePath);
            if (mainAfter != null)
            {
                mainAfter.name = candidateFileNameWithoutExt;
                EditorUtility.SetDirty(mainAfter);
                AssetDatabase.SaveAssets();
            }

            // Refresh to update Project window
            AssetDatabase.Refresh();
        };
    }
}
