using Xamarin.Forms;

using GoNTrip.Pages.Additional.Validators.ModelFieldsPatterns;

namespace GoNTrip.Pages.Additional.Validators.Templates
{
    public class CardValidator : FormValidator
    {
        public CardValidator(Entry cardNumberInput, Entry expMonthInput, Entry expYaerInput, Entry cvvInput, 
                                      ValidationHandler<InputView> validHandler, ValidationHandler<InputView> invalidHandler)
        {
            FieldValidationHandler<Entry> cardNumberValidation = new FieldValidationHandler<Entry>(
                CN => CN.Text != null && CardFieldsPatterns.CARD_NUMBER_PATTERN.IsMatch(CN.Text),
                invalidHandler, validHandler
            );

            FieldValidationHandler<Entry> expMonthValidation = new FieldValidationHandler<Entry>(
                EM => EM.Text != null && CardFieldsPatterns.CARD_MONTH_EXP_PATTERN.IsMatch(EM.Text),
                invalidHandler, validHandler
            );

            FieldValidationHandler<Entry> expYearValidation = new FieldValidationHandler<Entry>(
                EY => EY.Text != null && CardFieldsPatterns.CARD_YEAR_EXP_PATTERN.IsMatch(EY.Text),
                invalidHandler, validHandler
            );

            FieldValidationHandler<Entry> cvvValidation = new FieldValidationHandler<Entry>(
                CVV => CVV.Text != null && CardFieldsPatterns.CARD_CVV_PATTERN.IsMatch(CVV.Text),
                invalidHandler, validHandler
            );

            Constants.Callback<Entry> SubscribeEventsEntry = T =>
            {
                T.Unfocused += (sender, e) => ValidateId(GetId(sender));
                T.TextChanged += (sender, e) => ValidateId(GetId(sender));
            };

            Add<Entry>(cardNumberValidation, cardNumberInput, SubscribeEventsEntry);
            Add<Entry>(expMonthValidation, expMonthInput, SubscribeEventsEntry);
            Add<Entry>(expYearValidation, expYaerInput, SubscribeEventsEntry);
            Add<Entry>(cvvValidation, cvvInput, SubscribeEventsEntry);
        }
    }
}
