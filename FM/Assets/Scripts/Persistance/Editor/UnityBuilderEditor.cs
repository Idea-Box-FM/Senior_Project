using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/*Flower Box
 * Programmer Patrick Naatz
 * Intention: Make a Post Build Process to copy the simulations folder to the correct build path. when making a build from unity.
 * 
 * Edited by: Patrick Naatz
 *  added the SDS folder to things that needed to be copied 2/10/2022
 * NOTE: THIS SCRIPT HAS TO BE IN THE EDITOR FOLDER OR IT WILL CAUSE ERRORS AT BUILD TIME
 * NOTE: When in development you must choose a new location to build in order to have the sds folder get created
 */ 

public static class UnityBuilderEditor
{
    [PostProcessBuild]
    static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        //Todo maybe make the simulations path into a scriptables, I do not know if scriptables will even work here especially since they have to be accessed via Asset manager
        string path = Path.GetDirectoryName(pathToBuiltProject) + "\\" + "Simulations";
        FileUtil.CopyFileOrDirectory(Directory.GetCurrentDirectory() + "\\" + "Simulations", path);

        //make the sds folder
        path = Path.GetDirectoryName(pathToBuiltProject) + "\\" + "SDS";
        FileUtil.CopyFileOrDirectory(Directory.GetCurrentDirectory() + "\\" + "SDS", path);
    }
}
