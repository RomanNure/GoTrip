using System;

using Xamarin.Forms;
using Xamarin.Essentials;

using Android.Views;

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

        public const string DEFAULT_AVATAR_SOURCE = "DefaultAvatar.png";
        public const string DEFAULT_TOUR_IMAGE_SOURCE = "DefaultTourIcon.png";
        public const string DEFAULT_COMPANY_AVATAR_IMAGE_SOURCE = "DefaultTourIcon.png";

        public const string FILE_SELECTION_ERROR = "File selection error";
        public const string UNKNOWN_FILED_VALUE = "unknown";
        
        public const string CURRENCY_SYMBOL = "$";

        public const int ACTIVITY_INDICATOR_START_TIMEOUT = 100;

        public const int PHOTO_POPUP_X_TRANSLATION_BORDER = -100;
        public const int PHOTO_POPUP_Y_TRANSLATION_BORDER = -100;
    }
}
