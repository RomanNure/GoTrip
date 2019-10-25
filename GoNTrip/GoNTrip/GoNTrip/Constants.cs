using System;

using Xamarin.Forms;

using CustomControls;
using GoNTrip.Pages.Additional.Validators;

namespace GoNTrip
{
    public static class Constants
    {
        public delegate void Callback<T>(T obj);
        public static readonly Clicked CLICK_IGNORE = (e, sender) => true;

        public static readonly ValidationHandler<InputView> INVALID_HANDLER = Input => Input.BackgroundColor = (Color)Application.Current.Resources["InvalidColor"];
        public static readonly ValidationHandler<InputView> VALID_HANDLER = Input => Input.BackgroundColor = (Color)Application.Current.Resources["ContentBackColor"];

        public static readonly Random R = new Random();

        public const int MAX_PHOTO_WIDTH_HEIGHT = 768;

        public const string FILE_SELECTION_ERROR = "File selection error";
        public const string UNKNOWN_FILED_VALUE = "unknown";
        public const char FIRST_LAST_NAME_SPLITTER = ' ';
        public const string DEFAULT_AVATAR_SOURCE = "DefaultAvatar.png";
        public const string CURRENCY_SYMBOL = "$";
    }
}
