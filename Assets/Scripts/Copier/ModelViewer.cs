using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CopierAR
{
    public class ModelSelectedEventArgs : System.EventArgs
    {
        public Copier Copier;
        public ModelsDuraFreq ModelFrequency;
    }

    public class ModelsDuraFreq
    {
        public string Model;
        public float DemoDuration;
        public int Frequency;

        public string ModelString;
        public string DemoDurationString;
        public string FrequencyString;

        public ModelsDuraFreq()
        {
            Model = "";
            DemoDuration = 0;
            Frequency = 0;

            ModelString = "";
            DemoDurationString = "";
            FrequencyString = "";
        }
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

        public List<Copier> CopierList = new List<Copier>();
        public List<GameObject> ModelList = new List<GameObject>();
        public List<CopierController> ControllerList = new List<CopierController>();
        public List<ModelsDuraFreq> ModelDuraFreqList = new List<ModelsDuraFreq>();
        public static Dictionary<Copier, ModelsDuraFreq> ModelDataList = new Dictionary<Copier, ModelsDuraFreq>();

        private int m_viewIndex = 0;
        private int m_previousIndex = 0;

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

            // Clear lists
            ModelGroup.transform.Clear();
            CopierList.Clear();
            ModelList.Clear();
            ModelDuraFreqList.Clear();
            ModelDataList.Clear();

            // Initialize dictionaries
            for (int i = 0; i < CopierDatabase.copiers.Length; i++)
            {
                // Add copier data to dictionary
                CopierList.Add(CopierDatabase.copiers[i]);                

                // Add copier model to dictionary
                GameObject model = (GameObject)Instantiate(CopierDatabase.copiers[i].CopierPrefab, ModelGroup.transform);
                ModelList.Add(model);

                // Add copier controller to dictionary
                CopierController controller = model.GetComponent<CopierController>();
                ControllerList.Add(controller);

                ModelsDuraFreq mdf = new ModelsDuraFreq();
                ModelDuraFreqList.Add(mdf);
                mdf.Model = CopierDatabase.copiers[i].CopierName;
                ModelDataList.Add(CopierDatabase.copiers[i], mdf); 

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
            foreach (GameObject model in ModelList)
            {
                //model.SetActive(false);
                if (model == ModelList[index])
                    continue;
                model.transform.position = new Vector3(0, -5, 0);
            }

            // Enable model by index
            ModelList[index].transform.position = Vector3.zero;
            ModelList[index].SetActive(true);

            Copier copier = CopierList[index];

            // Increase view count
            ModelsDuraFreq mdf = ModelDataList[copier];
            mdf.Frequency += 1;
            ModelDataList[copier] = mdf;

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

        public ModelsDuraFreq GetActiveModelsDuraFreq()
        {
            return ModelDuraFreqList[m_viewIndex];
        }

        public static ModelsDuraFreq GetModelFrequency()
        {
            ModelsDuraFreq mdf = new ModelsDuraFreq();

            string models = "";
            string demoDurations = "";
            string frequencies = "";

            foreach (ModelsDuraFreq _mdf in ModelDataList.Values)
            {
                models += string.Format("{0} ", _mdf.ModelString);
                demoDurations += string.Format("{0} ", Converter.ToMinutesAndSeconds((int)_mdf.DemoDuration));
                frequencies += string.Format("{0} ", _mdf.Frequency);
            }

            mdf.ModelString = models;
            mdf.DemoDurationString = demoDurations;
            mdf.FrequencyString = frequencies;

            return mdf;
        }

        void Update()
        {
            if (UserInterface.IsDemoing())
            {
                ModelDuraFreqList[m_viewIndex].DemoDuration += Time.deltaTime;
            }
        }
    }
}

