﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
using System;

namespace CopierAR
{
    public enum ViewMode
    {
        Showcase,
        LifeScale
    }

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

        public ViewMode ViewMode;

        public List<Copier> CopierList = new List<Copier>();
        public List<GameObject> ModelList = new List<GameObject>();
        public List<CopierController> ControllerList = new List<CopierController>();
        public List<ModelsDuraFreq> ModelDuraFreqList = new List<ModelsDuraFreq>();
        public static Dictionary<Copier, ModelsDuraFreq> ModelDataList = new Dictionary<Copier, ModelsDuraFreq>();

        private int m_viewIndex = 0;
        private int m_previousIndex = 0;

        public CopierTrackableEventHandler CopierTrackableEventHandler;

        [Header("Model Rotator")]
        [SerializeField]
        private Camera cam;

        [SerializeField]
        private Quaternion defaultAvatarRotation;

        [SerializeField]
        private float slowSpeedRotation = 0.03f;
        [SerializeField]
        private float speedRotation = 0.03f;

        private RaycastHit m_hit;

        private const string MODEL_TAG = "Model";
        
        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void OnDestroy()
        {
            // Remove listener
            CopierTrackableEventHandler.OnTrackingStateChanged -= CopierTrackableEventHandler_OnTrackingStateChanged;

            // Handle memory
            if (Instance != null)
                Instance = null;
        }

        // Use this for initialization
        private void Start()
        {
            // Listen to tracking state changed event
            CopierTrackableEventHandler.OnTrackingStateChanged += CopierTrackableEventHandler_OnTrackingStateChanged;

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

            // Clear lists
            ModelGroup.transform.Clear();
            CopierList.Clear();
            ControllerList.Clear();
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

                //model.SetActive(false);
            }

            // De-activate showcase camera
            if(ShowcaseCamera != null)
                ShowcaseCamera.enabled = false;

            ViewMode = ViewMode.Showcase;

            return true;
        }

        private void CopierTrackableEventHandler_OnTrackingStateChanged(object sender, TrackingStateChangedEventArgs args)
        {
            switch (args.Status)
            {
                //case TrackableBehaviour.Status.DETECTED:
                case TrackableBehaviour.Status.EXTENDED_TRACKED:
                case TrackableBehaviour.Status.TRACKED:
                    OnTrackingFound();
                    break;
                case TrackableBehaviour.Status.NOT_FOUND:
                case TrackableBehaviour.Status.UNDEFINED:
                case TrackableBehaviour.Status.UNKNOWN:
                    OnTrackingLost();
                    break;
            }
        }

        public void Showcase()
        {
            ViewMode = ViewMode.Showcase;
            ModelGroup.transform.parent = null;
            ShowcaseCamera.enabled = true;

            ShowCurrentModel();
        }

        public void LifeScale()
        {
            ViewMode = ViewMode.LifeScale;
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
                //if (model == ModelList[index])
                //    continue;

                DisplayRenderers(model.GetComponentsInChildren<Renderer>(), false);
                model.transform.rotation = Quaternion.identity;
            }

            // Enable model by index
            // Show if image target is tracked
            Debug.Log(string.Format("{0} IsTracked {1}", Time.time.ToString(), CopierTrackableEventHandler.IsTracked));
            if (CopierTrackableEventHandler.IsTracked || ViewMode == ViewMode.Showcase)
            {
                DisplayRenderers(ModelList[index].GetComponentsInChildren<Renderer>(), true);
            }
            ModelList[index].transform.rotation = Quaternion.identity;

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

            // Check view mode
            //if (ViewMode == ViewMode.Showcase)
            //{
            //    ModelGroup.transform.parent = null;
            //    ShowcaseCamera.enabled = true;
            //}
            //else
            //{
            //    ModelGroup.transform.parent = LifeScaleParent;
            //    ShowcaseCamera.enabled = false;
            //}
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

        private void DisplayRenderers(Renderer[] renderers, bool value)
        {
            foreach (var rend in renderers)
            {
                rend.enabled = value;
            }
        }

        void Update()
        {
            // Update frequency view count
            if (UserInterface.IsDemoing())
            {
                ModelDuraFreqList[m_viewIndex].DemoDuration += Time.deltaTime;
            }

            // Check tracking status
            if (CopierTrackableEventHandler.GetCurrentStatus() == TrackableBehaviour.Status.TRACKED ||
                CopierTrackableEventHandler.GetCurrentStatus() == TrackableBehaviour.Status.EXTENDED_TRACKED ||
                ViewMode == ViewMode.Showcase)
            {
                DisplayRenderers(ModelList[m_viewIndex].GetComponentsInChildren<Renderer>(), true);
            }
            else
            {
                DisplayRenderers(ModelList[m_viewIndex].GetComponentsInChildren<Renderer>(), false);
            }
            // ROTATION
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                float x = -Input.GetAxis("Mouse X");
#elif UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)  
                {
                    float x = -Input.GetTouch(0).deltaPosition.x;
#endif
                //Quaternion newRot = Quaternion.AngleAxis(x * speedRotation, Vector3.up);
                //ModelList[m_viewIndex].transform.rotation = Quaternion.Slerp(ModelList[m_viewIndex].transform.rotation, newRot, Time.deltaTime * speedRotation);
                //ModelList[m_viewIndex].transform.rotation = Quaternion.Lerp(ModelList[m_viewIndex].transform.rotation, newRot, Time.deltaTime * speedRotation);

                ModelList[m_viewIndex].transform.rotation *= Quaternion.AngleAxis(x * speedRotation, Vector3.up);
            }
            /*
            // Rotation
            MouseButtonDown();
            MouseButtonUp();
#if UNITY_EDITOR
            if (Input.GetMouseButton(0) && m_isRotating)
            {
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && m_isRotating)
            {
                Touch touch = Input.GetTouch(0);
#endif
                RaycastHit dragingHit;

#if UNITY_EDITOR
                //Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));                
                Vector3 pos = cam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                Ray ray = cam.ViewportPointToRay(pos);
#elif UNITY_ANDROID || UNITY_IOS

                //Ray ray = cam.ScreenPointToRay(Input.touches[0].position);
                Vector3 pos = new Vector3(touch.position.x, touch.position.y, -cam.transform.position.z);
                Ray ray = cam.ScreenPointToRay(pos);
#endif
                Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);
                if (Physics.Raycast(ray, out dragingHit) && dragingHit.collider.tag == m_hit.collider.tag)
                {
                    if (m_hit.collider.tag == MODEL_TAG)
                    {
#if UNITY_EDITOR
                        float x = -Input.GetAxis("Mouse X");
#elif UNITY_ANDROID || UNITY_IOS

                        float x = -touch.deltaPosition.x;
                        DebugLog.Log(string.Format("{0} Touch delta x: {1}", Time.time, x));
#endif
                        ModelList[m_viewIndex].transform.rotation *= Quaternion.AngleAxis(x * speedRotation, Vector3.up);
                        }
                    }
                }
                else
                {
                    if (ModelList[m_viewIndex].transform.rotation.y != defaultAvatarRotation.y)
                    {
                        //SlowRotation();
                    }
                }
                */
        }

        #region Model rotation
        private void MouseButtonDown()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //DebugLog.Log(string.Format("{0} Touch {1}: {2}", Time.time, Input.touchCount, Input.GetTouch(0).phase.ToString()));
#endif

#if UNITY_EDITOR
                //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
                Vector3 pos = cam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                Ray ray = cam.ViewportPointToRay(pos);

                //DebugLog.Log(string.Format("{0} Ray position: {1}", Time.time, Input.mousePosition));
#elif UNITY_ANDROID || UNITY_IOS
                Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
                DebugLog.Log(string.Format("{0} Ray position: {1}", Time.time, Input.GetTouch(0).position));
#endif
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
                if (Physics.Raycast(ray, out m_hit))
                {
                    if (m_hit.collider.tag == MODEL_TAG)
                    {
                        Debug.Log(string.Format("{0} Down > {1}", Time.time, m_hit.collider.name));
                        //DebugLog.Log(string.Format("{0} Down > {1}", Time.time, m_hit.collider.name));
                    }
                }
            }
        }


        private void MouseButtonUp()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
#endif
                //m_isRotating = false;
                m_hit = new RaycastHit();
            }
        }

        private void SlowRotation()
        {
            ModelList[m_viewIndex].transform.rotation = Quaternion.Slerp(ModelList[m_viewIndex].transform.rotation,
                                                  defaultAvatarRotation,
                                                  slowSpeedRotation * Time.deltaTime);
        }

        private void OnTrackingFound()
        {
            DisplayRenderers(ModelList[m_viewIndex].GetComponentsInChildren<Renderer>(true), true);
        }

        private void OnTrackingLost()
        {
            // Check if in Life scale mode
            DisplayRenderers(ModelList[m_viewIndex].GetComponentsInChildren<Renderer>(true),
                ViewMode == ViewMode.LifeScale ? false : true);

            //foreach (var model in ModelList)
            //{
            //    DisplayRenderers(model.GetComponentsInChildren<Renderer>(), false);
            //    //Debug.Log(string.Format("Disabling {0} children renderer", model.name));
            //}
        }
        #endregion
    }
}

#region Backup code
//public Transform cube;

//    void Update()
//    {
//        if (Input.touchCount == 1)
//        {
//            // GET TOUCH 0
//            Touch touch0 = Input.GetTouch(0);

//            // APPLY ROTATION
//            if (touch0.phase == TouchPhase.Moved)
//            {
//                cube.transform.Rotate(0f, touch0.deltaPosition.x, 0f);
//            }

//        }

//using UnityEngine;
//using System.Collections;

//[RequireComponent(typeof(MeshRenderer))]

//public class rotateController : MonoBehaviour
//{

//    #region ROTATE
//    private float _sensitivity = 0.4f;
//    private Vector3 _mouseReference;
//    private Vector3 _mouseOffset;
//    private Vector3 _rotation = Vector3.zero;
//    private bool _isRotating;


//    #endregion

//    void Update()
//    {
//        if (_isRotating)
//        {
//            // offset
//            _mouseOffset = (Input.mousePosition - _mouseReference);

//            // apply rotation
//            _rotation.z = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

//            // rotate
//            gameObject.transform.Rotate(_rotation);

//            // store new mouse position
//            _mouseReference = Input.mousePosition;
//        }
//    }

//    void OnMouseDown()
//    {
//        // rotating flag
//        _isRotating = true;

//        // store mouse position
//        _mouseReference = Input.mousePosition;
//    }

//    void OnMouseUp()
//    {
//        // rotating flag
//        _isRotating = false;
//    }

//}
#endregion