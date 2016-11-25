using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CopierAR
{
    public class RightPanel : MonoBehaviour
    {
        public Image CopierLabel;
        public Image CopierInfo;

        public GameObject InteractionButtonGroup;
        public ButtonGroup PrintButton;
        public ButtonGroup ScannerButton;
        public ButtonGroup PanelButton;
        public ButtonGroup TonerButton;
        public ButtonGroup PaperTrayButton;
        public ButtonGroup SideTrayButton;
        public Button PreviousButton;
        public Button NextButton;

        public ModelViewer ModelViewer;

        private CopierController m_copierController;
        private string m_printSpeedURL;

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

        void Start()
        {
            ModelViewer.OnModelSelected += ModelViewer_OnModelSelected;
        }

        void OnDestroy()
        {
            ModelViewer.OnModelSelected -= ModelViewer_OnModelSelected;
        }

        void OnEnable()
        {
            PreviousButton.onClick.AddListener(() => { ModelViewer.PreviousModel(); });
            NextButton.onClick.AddListener(() => { ModelViewer.NextModel(); });
        }

        void OnDisable()
        {
            PreviousButton.onClick.RemoveAllListeners();
            NextButton.onClick.RemoveAllListeners();
        }

        private void ModelViewer_OnModelSelected(object sender, ModelSelectedEventArgs args)
        {
            Copier copier = (Copier)args.Copier;
            CopierLabel.sprite = copier.CopierLabel;
            CopierInfo.sprite = copier.CopierInfo;
            m_printSpeedURL = copier.PrintSpeedLink;

            // Re-mapping animation buttons to current copier controller          
            ResetCopier();
        }

        public bool Initialize()
        {
            m_copierController = null;
            m_printSpeedURL = "";

            return true;
        }

        public void ToggleInfoPanel(bool value = true)
        {
            CopierInfo.enabled = value;
        }

        public void ToggleInteractionPanel(bool value = true)
        {
            InteractionButtonGroup.SetActive(value);
        }

        public void Show()
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            transform.SetAsLastSibling();
        }

        public void Hide()
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
        }

        public void SetProductMode(ProductMode mode)
        {
            switch(mode)
            {
                case ProductMode.Showcase:
                    ModelViewer.Showcase();
                    ToggleInfoPanel(true);
                    ToggleInteractionPanel(false);
                    break;
                case ProductMode.LifeScale:
                    ModelViewer.LifeScale();                    
                    ToggleInfoPanel(false);
                    ToggleInteractionPanel(true);
                    break;
            }
        }

        private void ResetCopier()
        {
            m_copierController = ModelViewer.GetActiveController();

            StartCoroutine(m_copierController.ResetCopier());

            // Remove onClick listeners
            PrintButton.onClick.RemoveAllListeners();
            ScannerButton.onClick.RemoveAllListeners();
            PanelButton.onClick.RemoveAllListeners();
            TonerButton.onClick.RemoveAllListeners();
            PaperTrayButton.onClick.RemoveAllListeners();
            SideTrayButton.onClick.RemoveAllListeners();

            // Reset buttons to inactive state
            PrintButton.DeselectButton();
            ScannerButton.DeselectButton();
            PanelButton.DeselectButton();
            TonerButton.DeselectButton();
            PaperTrayButton.DeselectButton();
            SideTrayButton.DeselectButton();

            // Add onClick listeners
            PrintButton.onClick.AddListener(delegate { m_copierController.ShowPrintSpeed(m_printSpeedURL); });
            //PrintButton.onClick.AddListener(PrintButton.ToggleState);

            ScannerButton.onClick.AddListener(m_copierController.AnimateScanner);
            ScannerButton.onClick.AddListener(ScannerButton.ToggleState);

            PanelButton.onClick.AddListener(m_copierController.AnimatePanel);
            PanelButton.onClick.AddListener(PanelButton.ToggleState);

            TonerButton.onClick.AddListener(m_copierController.AnimateToner);
            TonerButton.onClick.AddListener(TonerButton.ToggleState);

            PaperTrayButton.onClick.AddListener(m_copierController.AnimatePaperTray);
            PaperTrayButton.onClick.AddListener(PaperTrayButton.ToggleState);

            SideTrayButton.onClick.AddListener(m_copierController.AnimateSideTray);
            SideTrayButton.onClick.AddListener(SideTrayButton.ToggleState);
        }
    }

    public enum ProductMode
    {
        Showcase,
        LifeScale
    }
}
