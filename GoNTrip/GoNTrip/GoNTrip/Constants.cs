using System;

using Xamarin.Forms;

using GoNTrip.Pages.Additional.Validators;

namespace GoNTrip
{
    public static class Constants
    {
        public delegate void Callback<T>(T obj);

        public static ValidationHandler<InputView> InvalidHandler = Input => Input.BackgroundColor = (Color)Application.Current.Resources["InvalidColor"];
        public static ValidationHandler<InputView> ValidHandler = Input => Input.BackgroundColor = (Color)Application.Current.Resources["ContentBackColor"];

        public static Random R = new Random();
        public const int MaxPhotoWidthHeight = 768;

        public const string FILE_SELECTION_ERROR = "File selection error";
        public const string UNKNOWN_FILED_VALUE = "unknown";
        public const char FIRST_LAST_NAME_SPLITTER = ' ';
    }
}
