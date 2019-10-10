using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Pages.Additional.Validators
{
    public class FormValidator
    {
        public delegate bool ActionPredicate();

        private Dictionary<object, int> Registry { get; set; }
        private Dictionary<int, ActionPredicate> Validation { get; set; }
        private int id = 0;

        public FormValidator()
        {
            Validation = new Dictionary<int, ActionPredicate>();
            Registry = new Dictionary<object, int>();
        }

        public int Add<T>(FieldValidationHandler<T> validationHandler, T validated)
        {
            Validation.Add(++id, () => validationHandler.Valiadte(validated));
            Registry.Add(validated, id);

            return id;
        }

        public bool Remove(int id)
        {
            if (Validation.ContainsKey(id))
            {
                Validation.Remove(id);
                return true;
            }
            return false;
        }

        public int GetId<T>(T obj)
        {
            return Registry.ContainsKey(obj) ? Registry[obj] : -1;
        }

        public bool ValidateToFirstInvalid()
        {
            foreach(KeyValuePair<int, ActionPredicate> predicate in Validation)
                if (!predicate.Value())
                    return false;
            return true;
        }
        
        public bool ValidateAll()
        {
            bool isValid = true;
            foreach (KeyValuePair<int, ActionPredicate> predicate in Validation)
                isValid &= predicate.Value();
            return isValid;
        }

        public bool ValidateId(int id)
        {
            return Validation.ContainsKey(id) ? Validation[id]() : true;
        }
    }
}
