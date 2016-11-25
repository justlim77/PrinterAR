using UnityEngine;
using System.Collections;

namespace CopierAR
{
    public enum CopierSeries
    {
        X4000,
        X7000
    }

    public class CopierDatabase : ScriptableObject
    {
        public Copier[] copiers;
    }    

    [System.Serializable]
    public class Copier
    {
        public string CopierName;
        public CopierSeries CopierSeries;
        public GameObject CopierPrefab;
        public Sprite CopierLabel;
        public Sprite CopierInfo;
        public string PrintSpeedLink;
    }
}
