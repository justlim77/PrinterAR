#if UNITY_ANDROID
using UnityEngine;
using DeadMosquito.AndroidGoodies;

namespace AndroidGoodiesExamples
{
    public class AlertDialogTest : MonoBehaviour
    {
        private static readonly string[] Colors = { "Red", "Green", "Blue" };

        #region message_dialog

        public void OnMessageSingleButtonDialog()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidAlertDialog.ShowMessageDialog("Single Button", "This dialog has only positive button", "Ok",
                () => AndroidGoodiesMisc.ShowToast("Positive button Click"), () => AndroidGoodiesMisc.ShowToast("Dismissed"));
        }

        public void OnMessageTwoButtonDialog()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidAlertDialog.ShowMessageDialog("Two Buttons", "This dialog has positive and negative button",
                "Ok", () => AndroidGoodiesMisc.ShowToast("Positive button Click"),
                "No!", () => AndroidGoodiesMisc.ShowToast("Negative button Click"),
                () => AndroidGoodiesMisc.ShowToast("Dismissed"));
        }

        public void OnMessageThreeButtonDialog()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidAlertDialog.ShowMessageDialog("Three Buttons",
                "This dialog has positive, negative and neutral buttons button",
                "Ok", () => AndroidGoodiesMisc.ShowToast("Positive button Click"),
                "No!", () => AndroidGoodiesMisc.ShowToast("Negative button Click"),
                "Maybe!", () => AndroidGoodiesMisc.ShowToast("Neutral button Click"),
                () => AndroidGoodiesMisc.ShowToast("Dismissed"));
        }

        public void OnShowDialogChooseItem()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            AndroidAlertDialog.ShowChooserDialog("Choose color", Colors,
                colorIndex => AndroidGoodiesMisc.ShowToast(Colors[colorIndex] + " selected"));
        }

        #endregion

        public void OnShowDialogSingleChooseItem()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            const int defaultSelectedItemIndex = 1;
            AndroidAlertDialog.ShowSingleItemChoiceDialog("Choose color", Colors, defaultSelectedItemIndex,
                colorIndex => AndroidGoodiesMisc.ShowToast(Colors[colorIndex] + " selected"),
                "OK", () => AndroidGoodiesMisc.ShowToast("OK!"));
        }

        public void OnShowDialogMultiChooseItem()
        {
            if (AndroidGoodiesExampleUtils.IfNotAndroid())
            {
                return;
            }

            bool[] initiallyCheckedItems = { false, true, false }; // second item is selected when dialog is shown
            AndroidAlertDialog.ShowMultiItemChoiceDialog("Choose color", Colors,
                initiallyCheckedItems,
                (colorIndex, isChecked) => AndroidGoodiesMisc.ShowToast(Colors[colorIndex] + " selected? " + isChecked), "OK",
                () => AndroidGoodiesMisc.ShowToast("OK!"));
        }
    }
}
#endif
