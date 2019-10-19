using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.Validators
{
    public delegate void ValidationHandler<in T>(T validated);

    public class FieldValidationHandler<T>
    {
        private Func<Entry, bool> p;

        private Predicate<T> Validator { get; set; }
        private ValidationHandler<T> InvalidHandler { get; set; }
        private ValidationHandler<T> ValidHandler { get; set; }

        public FieldValidationHandler(Predicate<T> validator, ValidationHandler<T> invalidHandler, ValidationHandler<T> validHandler = null)
        {
            Validator = validator;
            InvalidHandler = invalidHandler;
            ValidHandler = validHandler;
        }

        public FieldValidationHandler(Func<Entry, bool> p)
        {
            this.p = p;
        }

        public bool Validate(T validated)
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
