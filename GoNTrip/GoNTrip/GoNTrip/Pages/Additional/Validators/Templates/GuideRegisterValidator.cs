using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;
using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Validators.Templates
{
    public class GuideRegisterValidator : FormValidator
    {
        public GuideRegisterValidator(Entry cardNumberInput, Editor keywordsInput, ValidationHandler<InputView> validHandler, ValidationHandler<InputView> invalidHandler)
        {
            FieldValidationHandler<Entry> cardNumberValidation = new FieldValidationHandler<Entry>(
                CN => CN.Text != null && CardFieldsPatterns.CARD_NUMBER_PATTERN.IsMatch(CN.Text),
                invalidHandler, validHandler
            );

            FieldValidationHandler<Editor> keyWordsValidation = 
                new FieldValidationHandler<Editor>(KW => !string.IsNullOrEmpty(KW.Text), invalidHandler, validHandler);

            Constants.Callback<Entry> SubscribeEventsEntry = T =>
            {
                T.Unfocused += (sender, e) => ValidateId(GetId(sender));
                T.TextChanged += (sender, e) => ValidateId(GetId(sender));
            };

            Constants.Callback<Editor> SubscribeEventsEditor = T =>
            {
                T.Unfocused += (sender, e) => ValidateId(GetId(sender));
                T.TextChanged += (sender, e) => ValidateId(GetId(sender));
            };


            Add<Entry>(cardNumberValidation, cardNumberInput, SubscribeEventsEntry);
            Add<Editor>(keyWordsValidation, keywordsInput, SubscribeEventsEditor);
        }
    }
}
