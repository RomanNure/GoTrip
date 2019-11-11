using NUnit.Framework;

using GoNTrip.Pages.Additional.Validators;

namespace UnitTests
{
    [TestFixture]
    public class FormValidatorTestClass
    {
        [Test]
        public void TestMethod_AddTwice()
        {
            FieldValidationHandler<string> fieldValidationHandler = new FieldValidationHandler<string>((z) => true, null);
            string validated = "true";

            FormValidator formValidator = new FormValidator();

            int x = formValidator.Add<string>(fieldValidationHandler, validated);
            int y = formValidator.Add<string>(fieldValidationHandler, validated);

            Assert.IsTrue(formValidator.GetId<string>(validated) == x);
            Assert.IsFalse(formValidator.GetId<string>(validated) == y);

            Assert.IsTrue(y == -1);
        }

        [Test]
        public void TestMethod_AddInt()
        {
            FieldValidationHandler<int> fieldValidationHandler = new FieldValidationHandler<int>((z) => true, null);
            int validated = 1;

            FormValidator formValidator = new FormValidator();

            int x = formValidator.Add<int>(fieldValidationHandler, validated);
            int y = formValidator.Add<int>(fieldValidationHandler, validated);

            Assert.IsTrue(formValidator.GetId<int>(validated) == x);
            Assert.IsFalse(formValidator.GetId<int>(validated) == y);

            Assert.IsTrue(y == -1);
        }

        [Test]
        public void RemoveTest()
        {
            FieldValidationHandler<int> fieldValidationHandler = new FieldValidationHandler<int>((z) => true, null);
            int validated1 = 1;
            int validated2 = 2;

            FormValidator formValidator = new FormValidator();

            int x = formValidator.Add<int>(fieldValidationHandler, validated1);
            int y = formValidator.Add<int>(fieldValidationHandler, validated2);

            Assert.IsTrue(formValidator.Remove(x));//removed
            Assert.IsFalse(formValidator.Remove(x));//not removed

            Assert.IsTrue(formValidator.ValidateId(x));
            Assert.IsTrue(formValidator.ValidateId(y));
        }

        [Test]
        public void ValidateAllFalseTest()
        {
            FieldValidationHandler<bool> fieldValidationHandler = new FieldValidationHandler<bool>(z => z, null);
            bool validated1 = true;
            bool validated2 = false;
            bool validated3 = true;

            FormValidator formValidator = new FormValidator();

            formValidator.Add<bool>(fieldValidationHandler, validated1);
            formValidator.Add<bool>(fieldValidationHandler, validated2);
            formValidator.Add<bool>(fieldValidationHandler, validated3);

            Assert.IsFalse(formValidator.ValidateAll());
        }

        [Test]
        public void ValidateAllTrueTest()
        {
            FieldValidationHandler<bool> fieldValidationHandler = new FieldValidationHandler<bool>(z => z, null);
            bool validated1 = true;
            bool validated2 = true;
            bool validated3 = true;

            FormValidator formValidator = new FormValidator();

            formValidator.Add<bool>(fieldValidationHandler, validated1);
            formValidator.Add<bool>(fieldValidationHandler, validated2);
            formValidator.Add<bool>(fieldValidationHandler, validated3);

            Assert.IsTrue(formValidator.ValidateAll());
        }

        [Test]
        public void ValidateToFirstFalseTest()
        {
            FieldValidationHandler<bool> fieldValidationHandler = new FieldValidationHandler<bool>(z => z, null);
            bool validated1 = true;
            bool validated2 = false;
            bool validated3 = true;

            FormValidator formValidator = new FormValidator();

            formValidator.Add<bool>(fieldValidationHandler, validated1);
            formValidator.Add<bool>(fieldValidationHandler, validated2);
            formValidator.Add<bool>(fieldValidationHandler, validated3);

            Assert.IsFalse(formValidator.ValidateToFirstInvalid());
        }

        [Test]
        public void ValidateToFirstTrueTest()
        {
            FieldValidationHandler<bool> fieldValidationHandler = new FieldValidationHandler<bool>(z => z, null);
            bool validated1 = true;
            bool validated2 = true;
            bool validated3 = true;

            FormValidator formValidator = new FormValidator();

            formValidator.Add<bool>(fieldValidationHandler, validated1);
            formValidator.Add<bool>(fieldValidationHandler, validated2);
            formValidator.Add<bool>(fieldValidationHandler, validated3);

            Assert.IsTrue(formValidator.ValidateToFirstInvalid());
        }

        [Test]
        public void AfterDeletingValidateTest()
        {
            FieldValidationHandler<bool> fieldValidationHandler = new FieldValidationHandler<bool>(z => z, null);
            bool validated1 = true;
            bool validated2 = false;
            bool validated3 = true;

            FormValidator formValidator = new FormValidator();

            int i1 = formValidator.Add<bool>(fieldValidationHandler, validated1);
            int i2 = formValidator.Add<bool>(fieldValidationHandler, validated2);
            int i3 = formValidator.Add<bool>(fieldValidationHandler, validated3);

            formValidator.Remove(i2);

            Assert.IsTrue(formValidator.ValidateAll());
            Assert.IsTrue(formValidator.ValidateToFirstInvalid());
            Assert.IsTrue(formValidator.ValidateId(i2));
        }
    }
}
