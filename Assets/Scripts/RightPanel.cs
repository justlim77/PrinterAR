using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CopierAR
{
    public class RightPanel : MonoBehaviour
    {
        public Image CopierLabel;
        public Image CopierInfo;

        public Button PreviousBtn;
        public Button NextBtn;

        public ModelViewer ModelViewer;

        public CopierController PhotocopierController;

        private CanvasGroup _canvasGroup;
        private CanvasGroup canvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = GetComponent<CanvasGroup>();
                }

                return _canvasGroup;
            }
        }

        void OnEnable()
        {
            ModelViewer.OnModelSelected += ModelViewer_OnModelSelected;

            PreviousBtn.onClick.AddListener(() => { ModelViewer.PreviousModel(); } );
            NextBtn.onClick.AddListener(() => { ModelViewer.NextModel(); });
        }

        void OnDisable()
        {
            ModelViewer.OnModelSelected -= ModelViewer_OnModelSelected;

            PreviousBtn.onClick.RemoveAllListeners();
            NextBtn.onClick.RemoveAllListeners();
        }

        private void ModelViewer_OnModelSelected(object sender, ModelSelectedEventArgs args)
        {
            Copier copier = (Copier)args.Copier;
            CopierLabel.sprite = copier.CopierLabel;
            CopierInfo.sprite = copier.CopierInfo;
        }

        public bool Initialize()
        {
            if (PhotocopierController != null)
            {
                return PhotocopierController.Initialize();
            }
            else
            {
                Debug.LogWarning("PhotocopierController is null!");
                return false;
            }
        }

        public void ToggleInfoPanel(bool value = true)
        {
            //if (CopierLabel != null)
            //{
            //    CopierLabel.CrossFadeAlpha(value == true ? 1 : 0, 0.5f, true);                
            //}
            //if (CopierInfo != null)
            //{
            //    CopierInfo.CrossFadeAlpha(value == true ? 1 : 0, 0.5f, true);
            //}

            canvasGroup.alpha = value ? 1 : 0;            
        }

        public void SetAsMain()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
    }

}
