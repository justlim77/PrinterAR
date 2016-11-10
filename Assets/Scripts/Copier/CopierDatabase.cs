using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public class CopierDatabase : ScriptableObject
    {
        public Copier[] copiers;
    }    

    [System.Serializable]
    public class Copier
    {
        public string CopierName;
        public GameObject CopierPrefab;
        public Sprite CopierLabel;
        public Sprite CopierInfo;
        public string PrintSpeedLink;
    }
}
