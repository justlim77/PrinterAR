using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CopierAR
{
    public class ModelSelectedEventArgs : System.EventArgs
    {
        public Copier Copier;
    }

    public class ModelViewer : MonoBehaviour
    {
        public delegate void ModelSelectedEventHandler(object sender, ModelSelectedEventArgs args);
        public static event ModelSelectedEventHandler OnModelSelected;

        public string CopierDatabasePath = "Data/CopierDatabase";
        public CopierDatabase CopierDatabase = null;

        public Transform LifeScaleParent;
        public GameObject ModelGroup;
        public Camera ShowcaseCamera;

        public Dictionary<int, Copier> CopierList = new Dictionary<int, Copier>();
        public Dictionary<int, GameObject> ModelList = new Dictionary<int, GameObject>();

        private int m_viewIndex = 0;
        private int m_maxViewIndex = 0;

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        public bool Initialize()
        {
            if (CopierDatabase == null)
            {
                CopierDatabase = Resources.Load(CopierDatabasePath) as CopierDatabase;
            }

            m_viewIndex = 0;
            m_maxViewIndex = CopierDatabase.copiers.Length - 1;

            // Clear lists
            ModelGroup.transform.Clear();
            CopierList.Clear();
            ModelList.Clear();

            // Initialize copier and model list
            for (int i = 0; i < CopierDatabase.copiers.Length; i++)
            {
                CopierList.Add(i, CopierDatabase.copiers[i]);

                GameObject model = (GameObject)Instantiate(CopierDatabase.copiers[i].CopierPrefab, ModelGroup.transform);
                ModelList.Add(i, model);
                model.SetActive(false);
            }

            // De-activate showcase camera
            ShowcaseCamera.enabled = false;

            return true;
        }        

        public void Showcase()
        {            
            ModelGroup.transform.SetParent(null);
            ShowcaseCamera.enabled = true;
        }

        public void LifeScale()
        {
            ModelGroup.transform.SetParent(LifeScaleParent);
            ShowcaseCamera.enabled = false;
        }

        public void NextModel()
        {
            m_viewIndex++;
            m_viewIndex %= CopierDatabase.copiers.Length;
            SelectModel(m_viewIndex); 
        }

        public void PreviousModel()
        {
            m_viewIndex--;
            m_viewIndex %= CopierDatabase.copiers.Length;
            SelectModel(m_viewIndex);
        }

        private void SelectModel(int index)
        {
            foreach (GameObject model in ModelList.Values)
            {
                model.SetActive(false);
            }

            GameObject _model = null;
            if (ModelList.TryGetValue(index, out _model))
            {
                ModelList[index].SetActive(true);
            }

            if (OnModelSelected != null)
            {
                OnModelSelected(this, new ModelSelectedEventArgs
                {
                    Copier = CopierList[index]
                });
            }
        }
    }
}

