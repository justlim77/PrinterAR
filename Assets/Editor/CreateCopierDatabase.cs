using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CopierAR
{
    public class CreateCopierDatabase
    {
        [MenuItem("Assets/Create/CopierDatabase")]
        public static void CreateCopierDatabaseAsset()
        {
            CopierDatabase database = ScriptableObject.CreateInstance<CopierDatabase>();
            AssetDatabase.CreateAsset(database, "Assets/Resources/Data/CopierDatabase.asset");
            AssetDatabase.SaveAssets();
        }
    }
}

