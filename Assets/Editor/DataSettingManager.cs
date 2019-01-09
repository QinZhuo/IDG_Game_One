using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class DataSettingManager : Editor {
    static int i=0;
    [MenuItem("CreateSetting/Gun")]
	static void CreateGun()
    {
        ScriptableObject gun = ScriptableObject.CreateInstance<GunSetting>();
        if (!gun)
        {
            Debug.LogWarning("GunSetting not found!!!");
        }
        string path = Application.dataPath + "/GunSetting";
        if (!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }

        path = string.Format("Assets/GunSetting/{0}.asset", (i++).ToString());
        AssetDatabase.CreateAsset(gun, path);
    }
  
}
