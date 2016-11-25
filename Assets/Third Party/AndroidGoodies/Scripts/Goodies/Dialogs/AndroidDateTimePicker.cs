#if UNITY_ANDROID
using UnityEngine;
using System;

namespace DeadMosquito.AndroidGoodies
{
    public static class AndroidDateTimePicker
    {
        public delegate void OnDatePicked(int year,int month,int day);

        public delegate void OnTimePicked(int hourOfDay,int minute);

        /// <summary>
        /// Shows the default Android date picker.
        /// </summary>
        /// <param name="year">Year.</param>
        /// <param name="month">Month.</param>
        /// <param name="day">Day.</param>
        /// <param name="onDatePicked">Date was picked callback.</param>
        /// <param name="onCancel">Dialog was cancelled callback.</param>
        public static void ShowDatePicker(int year, int month, int day, OnDatePicked onDatePicked, Action onCancel)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    //  Month! (0-11 for compatibility with MONTH)
                    var dialog = new AndroidJavaObject("android.app.DatePickerDialog", AndroidUtils.Activity,
                                     new AndroidDateTimePicker.OnDateSetListenerPoxy(onDatePicked), year, month - 1, day);
                    dialog.Call("setOnCancelListener", new DialogOnCancelListenerPoxy(onCancel));
                    dialog.Call("show");
                });
        }

        /// <summary>
        /// Shows the default Android time picker.
        /// </summary>
        /// <param name="hourOfDay">Hour of day.</param>
        /// <param name="minute">Minute. Not Zero-base as on Android!</param>
        /// <param name="onTimePicked">Time was picked callback.</param>
        /// <param name="onCancel">Dialog was cancelled callback.</param>
        public static void ShowTimePicker(int hourOfDay, int minute, OnTimePicked onTimePicked, Action onCancel)
        {
            if (AndroidUtils.IsNotAndroidCheck())
            {
                return;
            }

            AndroidUtils.RunOnUiThread(() =>
                {
                    var dialog = new AndroidJavaObject("android.app.TimePickerDialog", AndroidUtils.Activity,
                                     new AndroidDateTimePicker.OnTimeSetListenerPoxy(onTimePicked), hourOfDay, minute, true);
                    dialog.Call("setOnCancelListener", new DialogOnCancelListenerPoxy(onCancel));
                    dialog.Call("show");
                });
        }

        private class OnDateSetListenerPoxy : AndroidJavaProxy
        {
            private readonly OnDatePicked _onDatePicked;

            public OnDateSetListenerPoxy(OnDatePicked onDatePicked)
                : base("android.app.DatePickerDialog$OnDateSetListener")
            {
                _onDatePicked = onDatePicked;
            }

            void onDateSet(AndroidJavaObject view, int year, int month, int dayOfMonth)
            {
                //  Month! (0-11 for compatibility with MONTH)
                GoodiesSceneHelper.Queue(() => _onDatePicked(year, month + 1, dayOfMonth));
            }
        }

        private class OnTimeSetListenerPoxy : AndroidJavaProxy
        {
            private readonly OnTimePicked _onTimePicked;

            public OnTimeSetListenerPoxy(OnTimePicked onTimePicked)
                : base("android.app.TimePickerDialog$OnTimeSetListener")
            {
                _onTimePicked = onTimePicked;
            }

            void onTimeSet(AndroidJavaObject view, int hourOfDay, int minute)
            {
                GoodiesSceneHelper.Queue(() => _onTimePicked(hourOfDay, minute));
            }
        }
    }
}
#endif
