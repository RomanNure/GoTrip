﻿using Xamarin.Forms;

using Android.Views;

using CustomControls;

using GoNTrip.Model;

namespace GoNTrip.Pages.Additional.Controls
{
    public class TourListItem : ClickableFrame
    {
        public Img image { get; private set; }
        private Label nameLabel { get; set; }
        private Label descriptionLabel { get; set; }

        private Label priceLabel { get; set; }
        private Label placesLabel { get; set; }
        private Label startEndLabel { get; set; }

        private const int MAX_NAME_SYMBOLS = 20;
        private const int MAX_DESCRIPTION_SYMBOLS = 80;
        private const string TOO_LONG_STRING_PROLONGATOR = "...";

        private const string PRICE_PLACEHOLDER_TEXT = "Price: ";
        private const string PLACES_PLACEHOLDER_TEXT = "Places: ";

        private const string OUTER_FRAME_CLASS_NAME = "OuterBorderFrame";
        private static readonly Thickness OUTER_FRAME_MARGIN = new Thickness(6, 3, 6, 3);

        private const string INNER_FRAME_CLASS_NAME = "InnerBorderFrame";
        private const string WRAPPER_CLASS_NAME = "TourPreviewWrapper";

        private const string TOUR_IMAGE_CLASS_NAME = "TourPreviewMainImage";
        private const int TOUR_IMAGE_ROW_START = 0;
        private const int TOUR_IMAGE_ROW_END = 3;
        private const int TOUR_IMAGE_COLUMN_START = 0;
        private const int TOUR_IMAGE_COLUMN_END = 1;
        private const bool TOUR_IMAGE_BORDER_ALWAYS = false;
        private const float TOUR_IMAGE_BORDERS_WIDTH = 0;
        private const int TOUR_IMAGE_BORDER_RADIUS = 45;
        private const string TOUR_IMAGE_DEFAULT_SOURCE = Constants.DEFAULT_TOUR_IMAGE_SOURCE;

        private const string TOUR_NAME_CLASS_NAME = "TourPreviewNameLabel";
        private const int TOUR_NAME_ROW = 0;
        private const int TOUR_NAME_COLUMN = 1;
        private static readonly LayoutOptions TOUR_NAME_HORISONTAL_OPTIONS = LayoutOptions.FillAndExpand;

        private const string TOUR_DESCRIPTION_CLASS_NAME = "PlaceholderLabel";
        private const int TOUR_DESCRIPTION_ROW = 1;
        private const int TOUR_DESCRIPTION_COLUMN = 1;
        private static readonly LayoutOptions TOUR_DESCRIPTION_HORISONTAL_OPTIONS = LayoutOptions.FillAndExpand;

        private const string TOUR_INFO_WRAPPER_CLASS_NAME = "TourPreviewInfoWrapper";
        private const int TOUR_INFO_WRAPPER_ROW = 2;
        private const int TOUR_INFO_WRAPPER_COLUMN = 1;

        private const string TOUR_PRICE_PLACEHOLDER_CLASS_NAME = "PlaceholderLabel";
        private const int TOUR_PRICE_PLACEHOLDER_ROW = 0;
        private const int TOUR_PRICE_PLACEHOLDER_COLUMN = 0;
        private static readonly LayoutOptions TOUR_PRICE_PLACEHOLDER_HORISONTAL_OPTIONS = LayoutOptions.FillAndExpand;

        private const string TOUR_PRICE_CLASS_NAME = "InfoLabel";
        private const int TOUR_PRICE_ROW = 0;
        private const int TOUR_PRICE_COLUMN = 1;
        private static readonly LayoutOptions TOUR_PRICE_HORISONTAL_OPTIONS = LayoutOptions.FillAndExpand;

        private const string TOUR_PLACES_PLACEHOLDER_CLASS_NAME = "PlaceholderLabel";
        private const int TOUR_PLACES_PLACEHOLDER_ROW = 0;
        private const int TOUR_PLACES_PLACEHOLDER_COLUMN = 2;
        private static readonly LayoutOptions TOUR_PLACES_PLACEHOLDER_HORISONTAL_OPTIONS = LayoutOptions.FillAndExpand;

        private const string TOUR_PLACES_CLASS_NAME = "InfoLabel";
        private const int TOUR_PLACES_ROW = 0;
        private const int TOUR_PLACES_COLUMN = 3;
        private static readonly LayoutOptions TOUR_PLACES_HORISONTAL_OPTIONS = LayoutOptions.FillAndExpand;

        private const string TOUR_START_END_CLASS_NAME = "InfoLabel";
        private const int TOUR_START_END_ROW_START = 1;
        private const int TOUR_START_END_ROW_END = 2;
        private const int TOUR_START_END_COLUMN_START = 0;
        private const int TOUR_START_END_COLUMN_END = 3;
        private static readonly LayoutOptions TOUR_START_END_HORISONTAL_OPTIONS = LayoutOptions.FillAndExpand;

        public TourListItem() => BuildLayout();

        public TourListItem(Tour tour, int numInList)
        {
            BuildLayout();
            Fill(tour, numInList);
        }

        public void Fill(Tour tour, int numInList)
        {
            image.Source = tour.mainPictureUrl == null ? TOUR_IMAGE_DEFAULT_SOURCE : tour.mainPictureUrl;

            nameLabel.Text = tour.name == null ? string.Empty : (tour.name.Length > MAX_NAME_SYMBOLS ? 
                tour.name.Substring(0, MAX_NAME_SYMBOLS - TOO_LONG_STRING_PROLONGATOR.Length) + TOO_LONG_STRING_PROLONGATOR : tour.name);

            nameLabel.Text += " " + tour.id;

            descriptionLabel.Text = tour.description == null ? string.Empty : (tour.description.Length > MAX_DESCRIPTION_SYMBOLS ?
                tour.description.Substring(0, MAX_DESCRIPTION_SYMBOLS - TOO_LONG_STRING_PROLONGATOR.Length) + TOO_LONG_STRING_PROLONGATOR : tour.description);

            priceLabel.Text = tour.pricePerPerson + Constants.CURRENCY_SYMBOL;
            placesLabel.Text = tour.participatingList.Count + "/" + tour.maxParticipants;
            startEndLabel.Text = tour.startDateTime.ToShortDateString() + " - " + tour.finishDateTime.ToShortDateString();
        }

        private void BuildLayout()
        {
            this.Style = (Style)App.Current.Resources[OUTER_FRAME_CLASS_NAME];
            this.Margin = OUTER_FRAME_MARGIN;

            Frame innerFrame = new Frame();
            innerFrame.Style = (Style)App.Current.Resources[INNER_FRAME_CLASS_NAME];
            this.Content = innerFrame;

            Grid tourPreviewWrapper = new Grid();
            tourPreviewWrapper.Style = (Style)App.Current.Resources[WRAPPER_CLASS_NAME];
            innerFrame.Content = tourPreviewWrapper;

            image = new Img();
            image.Style = (Style)App.Current.Resources[TOUR_IMAGE_CLASS_NAME];
            image.BorderAlways = TOUR_IMAGE_BORDER_ALWAYS;
            image.ClickedBorderWidth = TOUR_IMAGE_BORDERS_WIDTH;
            image.BorderRadius = TOUR_IMAGE_BORDER_RADIUS;
            image.Source = TOUR_IMAGE_DEFAULT_SOURCE;
            tourPreviewWrapper.Children.Add(image, TOUR_IMAGE_COLUMN_START, TOUR_IMAGE_COLUMN_END, TOUR_IMAGE_ROW_START, TOUR_IMAGE_ROW_END);

            nameLabel = new Label();
            nameLabel.Style = (Style)App.Current.Resources[TOUR_NAME_CLASS_NAME];
            nameLabel.HorizontalOptions = TOUR_NAME_HORISONTAL_OPTIONS;
            tourPreviewWrapper.Children.Add(nameLabel, TOUR_NAME_COLUMN, TOUR_NAME_ROW);

            descriptionLabel = new Label();
            descriptionLabel.Style = (Style)App.Current.Resources[TOUR_DESCRIPTION_CLASS_NAME];
            descriptionLabel.HorizontalOptions = TOUR_DESCRIPTION_HORISONTAL_OPTIONS;
            tourPreviewWrapper.Children.Add(descriptionLabel, TOUR_DESCRIPTION_COLUMN, TOUR_DESCRIPTION_ROW);

            Grid tourPreviewInfoWrapper = new Grid();
            tourPreviewInfoWrapper.Style = (Style)App.Current.Resources[TOUR_INFO_WRAPPER_CLASS_NAME];
            tourPreviewWrapper.Children.Add(tourPreviewInfoWrapper, TOUR_INFO_WRAPPER_COLUMN, TOUR_INFO_WRAPPER_ROW);

            Label pricePlaceholderLabel = new Label();
            pricePlaceholderLabel.Style = (Style)App.Current.Resources[TOUR_PRICE_PLACEHOLDER_CLASS_NAME];
            pricePlaceholderLabel.HorizontalOptions = TOUR_PRICE_PLACEHOLDER_HORISONTAL_OPTIONS;
            pricePlaceholderLabel.Text = PRICE_PLACEHOLDER_TEXT;
            tourPreviewInfoWrapper.Children.Add(pricePlaceholderLabel, TOUR_PRICE_PLACEHOLDER_COLUMN, TOUR_PRICE_PLACEHOLDER_ROW);

            priceLabel = new Label();
            priceLabel.Style = (Style)App.Current.Resources[TOUR_PRICE_CLASS_NAME];
            priceLabel.HorizontalOptions = TOUR_PRICE_HORISONTAL_OPTIONS;
            tourPreviewInfoWrapper.Children.Add(priceLabel, TOUR_PRICE_COLUMN, TOUR_PRICE_ROW);

            Label placesPlaceholderLabel = new Label();
            placesPlaceholderLabel.Style = (Style)App.Current.Resources[TOUR_PLACES_PLACEHOLDER_CLASS_NAME];
            placesPlaceholderLabel.HorizontalOptions = TOUR_PLACES_PLACEHOLDER_HORISONTAL_OPTIONS;
            placesPlaceholderLabel.Text = PLACES_PLACEHOLDER_TEXT;
            tourPreviewInfoWrapper.Children.Add(placesPlaceholderLabel, TOUR_PLACES_PLACEHOLDER_COLUMN, TOUR_PLACES_PLACEHOLDER_ROW);

            placesLabel = new Label();
            placesLabel.Style = (Style)App.Current.Resources[TOUR_PLACES_CLASS_NAME];
            placesLabel.HorizontalOptions = TOUR_PLACES_HORISONTAL_OPTIONS;
            tourPreviewInfoWrapper.Children.Add(placesLabel, TOUR_PLACES_COLUMN, TOUR_PLACES_ROW);

            startEndLabel = new Label();
            startEndLabel.Style = (Style)App.Current.Resources[TOUR_START_END_CLASS_NAME];
            startEndLabel.HorizontalOptions = TOUR_START_END_HORISONTAL_OPTIONS;
            tourPreviewInfoWrapper.Children.Add(startEndLabel, TOUR_START_END_COLUMN_START, TOUR_START_END_COLUMN_END, TOUR_START_END_ROW_START, TOUR_START_END_ROW_END);
        }
    }
}