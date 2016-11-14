using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CopierAR
{
    public class ModelSelectedEventArgs : System.EventArgs
    {
        public Copier Copier;
        public ModelFrequency ModelFrequency;
    }

    public struct ModelFrequency
    {
        public string Models;
        public string Frequency;
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
        public Dictionary<Copier, int> ViewCountList = new Dictionary<Copier, int>();

        private int m_viewIndex = 0;
        private int m_previousIndex = 0;

        private ModelFrequency m_modelFrequency = new ModelFrequency();

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
            m_previousIndex = 0;
            m_modelFrequency = new ModelFrequency();

            // Clear lists
            ModelGroup.transform.Clear();
            CopierList.Clear();
            ModelList.Clear();
            ViewCountList.Clear();

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

                ViewCountList.Add(CopierDatabase.copiers[i], 0);

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

            ShowCurrentModel();
        }

        public void LifeScale()
        {
            ModelGroup.transform.parent = LifeScaleParent;
            ShowcaseCamera.enabled = false;

            ShowCurrentModel();
        }

        public void NextModel()
        {
            m_previousIndex = m_viewIndex;
            m_viewIndex++;
            m_viewIndex %= CopierDatabase.copiers.Length;
            SelectModel(m_viewIndex); 
        }

        public void PreviousModel()
        {
            m_previousIndex = m_viewIndex;
            m_viewIndex--;
            if (m_viewIndex < 0)
            {
                m_viewIndex = CopierDatabase.copiers.Length - 1;
            }
            SelectModel(m_viewIndex);
        }

        private void SelectModel(int index)
        {
            // Reset previous model
            StartCoroutine(ControllerList[m_previousIndex].ResetCopier());
            //ControllerList[m_previousIndex].ResetCopierToDefault();

            // Disable all models
            foreach (GameObject model in ModelList.Values)
            {
                //model.SetActive(false);
                model.transform.position = new Vector3(0, -5, 0);
            }

            // Enable model by index
            GameObject _model = null;
            if (ModelList.TryGetValue(index, out _model))
            {
                ModelList[index].transform.position = Vector3.zero;
                ModelList[index].SetActive(true);
            }

            Copier copier = CopierList[index];

            // Increment view count by 1
            ViewCountList[copier] = ViewCountList[copier] + 1;

            if (OnModelSelected != null)
            {
                OnModelSelected(this, new ModelSelectedEventArgs
                {
                    Copier = CopierList[index],
                    ModelFrequency = GetModelFrequency() 
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

        public ModelFrequency GetModelFrequency()
        {
            string models = "";
            foreach (Copier copier in ViewCountList.Keys)
            {
                models += string.Format(",{0}", copier.CopierName);
            }

            string frequencies = "";
            foreach (int viewCount in ViewCountList.Values)
            {
                frequencies += string.Format(",{0}", viewCount);
            }

            m_modelFrequency.Models = models;
            m_modelFrequency.Frequency = frequencies;

            return m_modelFrequency;
        }
    }
}

