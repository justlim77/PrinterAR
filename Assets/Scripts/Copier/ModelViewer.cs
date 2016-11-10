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
        public Dictionary<int, CopierController> ControllerList = new Dictionary<int, CopierController>();

        private int m_viewIndex = 0;

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

            // Clear lists
            ModelGroup.transform.Clear();
            CopierList.Clear();
            ModelList.Clear();

            // Initialize dictionaries
            for (int i = 0; i < CopierDatabase.copiers.Length; i++)
            {
                // Add copier data to dictionary
                CopierList.Add(i, CopierDatabase.copiers[i]);

                // Add copier model to dictionary
                GameObject model = (GameObject)Instantiate(CopierDatabase.copiers[i].CopierPrefab, ModelGroup.transform);
                ModelList.Add(i, model);

                // Add copier controller to dictionary
                CopierController controller = model.GetComponent<CopierController>();
                ControllerList.Add(i, controller);

                model.SetActive(false);
            }

            // De-activate showcase camera
            ShowcaseCamera.enabled = false;

            return true;
        }        

        public void Showcase()
        {            
            ModelGroup.transform.parent = null;
            ShowcaseCamera.enabled = true;
        }

        public void LifeScale()
        {
            ModelGroup.transform.parent = LifeScaleParent;
            ShowcaseCamera.enabled = false;
        }

        public void NextModel()
        {
            ++m_viewIndex;
            m_viewIndex %= CopierDatabase.copiers.Length;
            SelectModel(m_viewIndex); 
        }

        public void PreviousModel()
        {
            --m_viewIndex;
            if (m_viewIndex < 0)
            {
                m_viewIndex = CopierDatabase.copiers.Length - 1;
            }
            SelectModel(m_viewIndex);
        }

        private void SelectModel(int index)
        {
            // Disable all models
            foreach (GameObject model in ModelList.Values)
            {
                model.SetActive(false);
            }

            // Enable model by index
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

        public void ShowCurrentModel()
        {
            SelectModel(m_viewIndex);
        }

        public CopierController GetActiveController()
        {
            return ControllerList[m_viewIndex];
        }
    }
}

