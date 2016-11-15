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

        public static ModelViewer Instance { get; private set; }

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
        private int m_lastViewedModel = -1;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void OnDestroy()
        {
            if (Instance != null)
                Instance = null;
        }

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
            m_previousIndex = -1;
            m_lastViewedModel = -1;

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
                mdf.Model = CopierDatabase.copiers[i].CopierName;
                ModelDuraFreqList.Add(mdf);
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
            if (m_previousIndex != -1)
            {
                StartCoroutine(ControllerList[m_previousIndex].ResetCopier());
            }

            // Hide models
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

            // Increase view count if new index selected
            Debug.Log(string.Format("prevIndex: {0}, viewIndex: {1}", m_previousIndex, m_viewIndex));

            if (index != m_previousIndex)
            {
                ModelDataList[copier].Frequency += 1;
                m_previousIndex = index;
            }

            // Debug information
            Debug.Log(string.Format("Model: {0} Duration: {1} Frequency: {2}", ModelDataList[copier].Model, ModelDataList[copier].DemoDuration, ModelDataList[copier].Frequency));

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
                models += string.Format("{0} ", _mdf.Model);
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

