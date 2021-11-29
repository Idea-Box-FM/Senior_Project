using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/*Flower Box
 * Programmer Patrick Naatz
 * Intention: Make a Post Build Process to copy the simulations folder to the correct build path. when making a build from unity.
 * 
 * NOTE: THIS SCRIPT HAS TO BE IN THE EDITOR FOLDER OR IT WILL CAUSE ERRORS AT BUILD TIME
 */ 

public static class UnityBuilderEditor
{
    [PostProcessBuild]
    static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        //Todo maybe make the simulations path into a scriptables, I do not know if scriptables will even work here especially since they have to be accessed via Asset manager
        string path = Path.GetDirectoryName(pathToBuiltProject) + "\\" + "Simulations";
        FileUtil.CopyFileOrDirectory(Directory.GetCurrentDirectory() + "\\" + "Simulations", path);
    }
}
