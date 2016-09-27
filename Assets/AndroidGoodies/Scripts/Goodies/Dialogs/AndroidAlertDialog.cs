#if UNITY_ANDROID
using UnityEngine;
using System;

namespace DeadMosquito.AndroidGoodies
{
    /// <summary>
    /// Android alert dialog. Contains statis methods to show different dialogs.
    /// </summary>
    public class AndroidAlertDialog
    {
        private readonly AndroidJavaObject _dialog;

        private AndroidAlertDialog(AndroidAlertDialog.Builder builder)
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                throw new System.InvalidOperationException("AndroidJavaObject can be created only on Android");
            }

            var javaBuilder = new AndroidJavaObject("android.app.AlertDialog$Builder", AndroidUtils.Activity);
            if (!string.IsNullOrEmpty(builder._title))
            {
                javaBuilder.Call<AndroidJavaObject>("setTitle", builder._title);
            }
            if (!string.IsNullOrEmpty(builder._message))
            {
                javaBuilder.Call<AndroidJavaObject>("setMessage", builder._message);
            }

            // Buttons
            if (builder._isPositiveButtonSet)
            {
                javaBuilder.Call<AndroidJavaObject>("setPositiveButton", builder._positiveButtonText, new DialogOnClickListenerProxy(builder._positiveButtonCallback));
            }
            if (builder._isNegativeButtonSet)
            {
                javaBuilder.Call<AndroidJavaObject>("setNegativeButton", builder._negativeButtonText, new DialogOnClickListenerProxy(builder._negativeButtonCallback));
            }
            if (builder._isNeutralButtonSet)
            {
                javaBuilder.Call<AndroidJavaObject>("setNeutralButton", builder._neutralButtonText, new DialogOnClickListenerProxy(builder._neutralButtonCallback));
            }

            // Items
            if (builder._areItemsSet)
            {
                javaBuilder.Call<AndroidJavaObject>("setItems", builder._items, new DialogOnClickListenerProxy(builder._itemClickCallback, true));
            }
            if (builder._areSingleChoiceItemsSet)
            {
                javaBuilder.Call<AndroidJavaObject>("setSingleChoiceItems", 
                    builder._singleChoiceItems,
                    builder._singleChoiceCheckedItem, 
                    new DialogOnClickListenerProxy(builder._singleChoiceItemClickCallback));
            }
            if (builder._areMultiChoiceItemsSet)
            {
                javaBuilder.Call<AndroidJavaObject>("setMultiChoiceItems", 
                    builder._multiChoiceItems,
                    builder._multiChoiceCheckedItems, 
                    new DialogOnMultiChoiceClickListenerProxy(builder._multiChoiceItemClickCallback));
            }

            // Cancel / Dismiss
            if (builder._onCancelCallback != null)
            {
                javaBuilder.Call<AndroidJavaObject>("setOnCancelListener", new DialogOnCancelListenerPoxy(builder._onCancelCallback));
            }
            if (builder._onDismissCallback != null)
            {
                javaBuilder.Call<AndroidJavaObject>("setOnDismissListener", new DialogOnDismissListenerProxy(builder._onDismissCallback));
            }

            _dialog = javaBuilder.Call<AndroidJavaObject>("create");
        }

        private class Builder
        {
            internal string _title;
            internal string _message;

            // buttons
            internal bool _isPositiveButtonSet = false;
            internal string _positiveButtonText;
            internal Action _positiveButtonCallback;

            internal bool _isNegativeButtonSet = false;
            internal string _negativeButtonText;
            internal Action _negativeButtonCallback;

            internal bool _isNeutralButtonSet = false;
            internal string _neutralButtonText;
            internal Action _neutralButtonCallback;

            // items
            internal bool _areItemsSet = false;
            internal string[] _items;
            internal Action<int> _itemClickCallback;

            // single choice
            internal bool _areSingleChoiceItemsSet = false;
            internal string[] _singleChoiceItems;
            internal int _singleChoiceCheckedItem;
            internal Action<int> _singleChoiceItemClickCallback;

            // multi choice
            internal bool _areMultiChoiceItemsSet = false;
            internal string[] _multiChoiceItems;
            internal bool[] _multiChoiceCheckedItems;
            internal Action<int, bool> _multiChoiceItemClickCallback;

            internal Action _onCancelCallback;
            internal Action _onDismissCallback;

            internal Builder()
            {
            }
                
            public Builder SetTitle(string title)
            {
                _title = title;
                return this;
            }

            public Builder SetMessage(string message)
            {
                _message = message;
                return this;
            }

            public Builder SetPositiveButton(string buttonText, Action callback)
            { 
                _isPositiveButtonSet = true;
                _positiveButtonText = buttonText;
                _positiveButtonCallback = callback;
                return this;
            }

            public Builder SetNegativeButton(string buttonText, Action callback)
            { 
                _isNegativeButtonSet = true;
                _negativeButtonText = buttonText;
                _negativeButtonCallback = callback;
                return this;
            }

            public Builder SetNeutralButton(string buttonText, Action callback)
            { 
                _isNeutralButtonSet = true;
                _neutralButtonText = buttonText;
                _neutralButtonCallback = callback;
                return this;
            }

            public Builder SetItems(string[] items, Action<int> onItemClick)
            {
                _areItemsSet = true;
                _items = items;
                _itemClickCallback = onItemClick;
                return this;
            }

            public Builder SetMultiChoiceItems(string[] items, bool[] checkedItems, Action<int, bool> onItemClick)
            {
                _areMultiChoiceItemsSet = true;
                _multiChoiceItems = items;
                _multiChoiceCheckedItems = checkedItems;
                _multiChoiceItemClickCallback = onItemClick;
                return this;
            }

            public Builder SetSingleChoiceItems(string[] items, int checkedItem, Action<int> onItemClick)
            {
                _areSingleChoiceItemsSet = true;
                _singleChoiceItems = items;
                _singleChoiceCheckedItem = checkedItem;
                _singleChoiceItemClickCallback = onItemClick;
                return this;
            }

            /// <summary>
            /// Sets the on cancel listener.
            /// </summary>
            /// <returns>The on cancel listener.</returns>
            /// <param name="onCancel">On cancel.</param>
            public Builder SetOnCancelListener(Action onCancel)
            {
                _onCancelCallback = onCancel;
                return this;
            }

            /// <summary>
            /// Sets the on dismiss listener.
            /// </summary>
            /// <returns>The on dismiss listener.</returns>
            /// <param name="onDismiss">On dismiss.</param>
            public Builder SetOnDismissListener(Action onDismiss)
            {
                _onDismissCallback = onDismiss;
                return this;
            }

            /// <summary>
            /// Creates an AndroidAlertDialog with the arguments supplied to this builder.
            /// </summary>
            public AndroidAlertDialog Create()
            {
                return new AndroidAlertDialog(this);
            }
        }

        private void Show()
        {
            _dialog.Call("show");
        }

        private void Dismiss()
        {
            _dialog.Call("dismiss");
        }

        #region show_dialog

        /// <summary>
        /// Displays message dialog with positive button only
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="positiveButtonText">Text for positive button</param>
        /// <param name="onPositiveButtonClick">Positive button callback</param>
        /// <param name="onDismiss">On dismiss callback</param>
        public static void ShowMessageDialog(string title, string message, string positiveButtonText,
                                             Action onPositiveButtonClick,
                                             Action onDismiss = null)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            if (onPositiveButtonClick == null)
            {
                throw new ArgumentException("Button callback cannot be null");
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    var builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss);
                    var dialog = builder.Create();
                    dialog.Show();
                });
        }


        /// <summary>
        /// Displays message dialog with positive and negative buttons
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="positiveButtonText">Text for positive button</param>
        /// <param name="onPositiveButtonClick">Positive button callback</param>
        /// <param name="negativeButtonText">Text for negative button</param>
        /// <param name="onNegativeButtonClick">Negative button callback</param>
        /// <param name="onDismiss">On dismiss callback</param>
        public static void ShowMessageDialog(string title, string message, 
                                             string positiveButtonText, Action onPositiveButtonClick,
                                             string negativeButtonText, Action onNegativeButtonClick,
                                             Action onDismiss = null)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            if (onPositiveButtonClick == null)
            {
                throw new ArgumentException("Button callback cannot be null");
            }
            if (onNegativeButtonClick == null)
            {
                throw new ArgumentException("Button callback cannot be null");
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    var builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss);
                    builder.SetNegativeButton(negativeButtonText, onNegativeButtonClick);
                    var dialog = builder.Create();
                    dialog.Show();
                });
        }

        /// <summary>
        /// Displays message dialog with positive, negative buttons and neutral buttons
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="positiveButtonText">Text for positive button</param>
        /// <param name="onPositiveButtonClick">Positive button callback</param>
        /// <param name="negativeButtonText">Text for negative button</param>
        /// <param name="onNegativeButtonClick">Negative button callback</param>
        /// <param name="neutralButtonText">Text for neutral button</param>
        /// <param name="onNeutralButtonClick">Neutral button callback</param>
        /// <param name="onDismiss">On dismiss callback</param>
        public static void ShowMessageDialog(string title, string message, 
                                             string positiveButtonText, Action onPositiveButtonClick,
                                             string negativeButtonText, Action onNegativeButtonClick,
                                             string neutralButtonText, Action onNeutralButtonClick,
                                             Action onDismiss = null)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            if (onPositiveButtonClick == null)
            {
                throw new ArgumentException("Button callback cannot be null");
            }
            if (onNegativeButtonClick == null)
            {
                throw new ArgumentException("Button callback cannot be null");
            }
            if (onNeutralButtonClick == null)
            {
                throw new ArgumentException("Button callback cannot be null");
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    var builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss);
                    builder.SetNegativeButton(negativeButtonText, onNegativeButtonClick);
                    builder.SetNeutralButton(neutralButtonText, onNeutralButtonClick);
                    var dialog = builder.Create();
                    dialog.Show();
                });
        }

        private static Builder CreateMessageDialogBuilder(string title, string message,
                                                          string positiveButtonText, Action onPositiveButtonClick, Action onDismiss)
        {
            var builder = new Builder().SetTitle(title).SetMessage(message);
            builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
            if (onDismiss != null)
            {
                builder.SetOnDismissListener(onDismiss);
            }
            return builder;
        }

        /// <summary>
        /// Shows the chooser dialog to choose one item from the list. Dialog is dismissed on item click.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="items">Itmes to choose from</param>
        /// <param name="onClickCallback">Invoked on item click</param>
        public static void ShowChooserDialog(string title, string[] items, Action<int> onClickCallback)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    var builder = new Builder().SetTitle(title);
                    builder.SetItems(items, onClickCallback);
                    var dialog = builder.Create();
                    dialog.Show();
                });
        }

        /// <summary>
        /// Shows the single item choice dialog.
        /// </summary>
        /// <returns>The single item choice dialog.</returns>
        /// <param name="title">Title.</param>
        /// <param name="items">Items.</param>
        /// <param name="checkedItem">Checked item.</param>
        /// <param name="onItemClicked">On item clicked.</param>
        /// <param name="positiveButtonText">Positive button text.</param>
        /// <param name="onPositiveButtonClick">On positive button click.</param>
        public static void ShowSingleItemChoiceDialog(string title, string[] items, int checkedItem, Action<int> onItemClicked,
                                                      string positiveButtonText, Action onPositiveButtonClick)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            if (items == null)
            {
                throw new ArgumentException("Items cannot be null");
            }
            if (onItemClicked == null)
            {
                throw new ArgumentException("Callback cannot be null");
            }
            if (onPositiveButtonClick == null)
            {
                throw new ArgumentException("Callback cannot be null");
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    var builder = CreateSingleChoiceDialogBuilder(title, items, checkedItem, onItemClicked);
                    builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
                    var dialog = builder.Create();
                    dialog.Show();
                });
        }

        private static AndroidAlertDialog.Builder CreateSingleChoiceDialogBuilder(string title, string[] items, int checkedItem, Action<int> onItemClicked)
        {
            var builder = new AndroidAlertDialog.Builder().SetTitle(title);
            builder.SetSingleChoiceItems(items, checkedItem, onItemClicked);
            return builder;
        }

        /// <summary>
        /// Shows the multi item choice dialog.
        /// </summary>
        /// <returns>The multi item choice dialog.</returns>
        /// <param name="title">Title.</param>
        /// <param name="items">Items.</param>
        /// <param name="checkedItems">Checked items.</param>
        /// <param name="onItemClicked">On item clicked.</param>
        /// <param name="positiveButtonText">Positive button text.</param>
        /// <param name="onPositiveButtonClick">On positive button click.</param>
        public static void ShowMultiItemChoiceDialog(string title, string[] items, bool[] checkedItems, Action<int, bool> onItemClicked,
                                                     string positiveButtonText, Action onPositiveButtonClick)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            if (items == null)
            {
                throw new ArgumentException("Items cannot be null");
            }
            if (checkedItems == null)
            {
                throw new ArgumentException("Checked items cannot be null");
            }
            if (onItemClicked == null)
            {
                throw new ArgumentException("Callback cannot be null");
            }
            if (onPositiveButtonClick == null)
            {
                throw new ArgumentException("Callback cannot be null");
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    var builder = CreateMultiChoiceDialogBuilder(title, items, checkedItems, onItemClicked);
                    builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
                    var dialog = builder.Create();
                    dialog.Show();
                });
        }

        private static AndroidAlertDialog.Builder CreateMultiChoiceDialogBuilder(string title, string[] items, bool[] checkedItems, Action<int, bool> onItemClicked)
        {
            var builder = new AndroidAlertDialog.Builder().SetTitle(title);
            builder.SetMultiChoiceItems(items, checkedItems, onItemClicked);
            return builder;
        }

        #endregion
    }
}
#endif