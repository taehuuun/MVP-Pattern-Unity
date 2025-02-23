using System.IO;
using UnityEditor;
using UnityEngine;

public class MVPCreator : MonoBehaviour
{
    [MenuItem("Assets/Create/MVP Scripts", false, 1)]
    public static void CreateMVPSciprts()
    {
        string folderName = GetSelectFolderName();

        if (string.IsNullOrEmpty(folderName))
        {
            Debug.LogError($"선택된 폴더가 없음");
            return;
        }

        string directoryPath = GetCurrentDirectory();

        string modelTemplatePath = "Assets/Editor/ScriptTemplates/ModelTemplate.txt";
        string modelFilePath = Path.Combine(directoryPath, $"{folderName}Model.cs");
        CreateScriptFromTemplate(modelTemplatePath, modelFilePath, folderName);
        
        string viewTemplatePath = "Assets/Editor/ScriptTemplates/ViewTemplate.txt";
        string viewFilePath = Path.Combine(directoryPath, $"{folderName}View.cs");
        CreateScriptFromTemplate(viewTemplatePath, viewFilePath, folderName);
        
        string presenterTemplatePath = "Assets/Editor/ScriptTemplates/PresenterTemplate.txt";
        string presenterFilePath = Path.Combine(directoryPath, $"{folderName}Presenter.cs");
        CreateScriptFromTemplate(presenterTemplatePath, presenterFilePath, folderName);
        
        AssetDatabase.Refresh();
    }
    
    private static void CreateScriptFromTemplate(string templatePath, string newScriptPath, string folderName)
    {
        if (!File.Exists(templatePath))
        {
            Debug.LogError($"템플릿을 찾을 수 없음: {templatePath}");
            return;
        }

        string templateContent = File.ReadAllText(templatePath);
        templateContent = templateContent.Replace("#SCRIPTNAME#", folderName);
        File.WriteAllText(newScriptPath, templateContent);
    }
    
    private static string GetCurrentDirectory()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            return "Assets";
        }

        if (Directory.Exists(path))
        {
            return path;
        }

        return Path.GetDirectoryName(path);
    }

    private static string GetSelectFolderName()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        if (Directory.Exists(path))
        {
            return new DirectoryInfo(path).Name;
        }

        return new DirectoryInfo(Path.GetDirectoryName(path)).Name;
    }
}
