using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Pages.Additional.Validators
{
    public delegate void ValidationHandler<in T>(T validated);

    public class FieldValidationHandler<T>
    {
        private Predicate<T> Validator { get; set; }
        private ValidationHandler<T> InvalidHandler { get; set; }
        private ValidationHandler<T> ValidHandler { get; set; }

        public FieldValidationHandler(Predicate<T> validator, ValidationHandler<T> invalidHandler, ValidationHandler<T> validHandler = null)
        {
            Validator = validator;
            InvalidHandler = invalidHandler;
            ValidHandler = validHandler;
        }

        public bool Valiadte(T validated)
        {
            if (Validator == null)
                return true;

            bool valid = Validator(validated);

            if (valid && ValidHandler != null)
                ValidHandler(validated);
            else if (!valid && InvalidHandler != null)
                InvalidHandler(validated);

            return valid;
        }
    }
}
