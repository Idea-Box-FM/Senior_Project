using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class UnityBuilderEditor
{
    [PostProcessBuild]
    static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        string path = Path.GetDirectoryName(pathToBuiltProject) + "\\" + "Simulations";
        Debug.Log(path);
        Debug.Log("Source" + Directory.GetCurrentDirectory() + "\\" + "Simulations");
        //FileUtil.CopyFileOrDirectory(Directory.GetCurrentDirectory() + "\\" + "Simulations", path);
        FileUtil.CopyFileOrDirectory(Directory.GetCurrentDirectory() + "\\" + "Simulations", path);
    }
}
