using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CopierAR
{
    public class ModelViewer : MonoBehaviour
    {
        public string CopierDatabasePath = "Data/CopierDatabase";
        public CopierDatabase CopierDatabase = null;

        public Dictionary<int, Copier> CopierList = new Dictionary<int, Copier>();
        public Dictionary<int, GameObject> ModelList = new Dictionary<int, GameObject>();

        private int m_viewIndex = 0;
        private int m_maxViewIndex = 0;

        // Use this for initialization
        void Start()
        {
            if (CopierDatabase == null)
            {
                CopierDatabase = Resources.Load(CopierDatabasePath) as CopierDatabase;
            }

            m_viewIndex = 0;
            m_maxViewIndex = CopierDatabase.copiers.Length - 1;

            // Clear all model children
            this.transform.Clear();

            // Initialize copier and model list
            CopierList.Clear();
            ModelList.Clear();
            for (int i = 0; i < CopierDatabase.copiers.Length; i++)
            {
                CopierList.Add(i, CopierDatabase.copiers[i]);

                GameObject model = (GameObject)Instantiate(CopierDatabase.copiers[i].CopierPrefab, this.transform);
                ModelList.Add(i, model);
                model.SetActive(false);
            }
        }

        public void NextModel()
        {

        }

        private void SelectModel(int index)
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

