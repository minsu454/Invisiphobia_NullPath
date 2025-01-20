using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class PlayerPrefsEditor
{
    [MenuItem("Tools/PlayerPrefs/DeleteAll")]
    static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    
}
