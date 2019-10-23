using NUnit.Framework;

using GoNTrip.Pages.Additional.Validators;

namespace UnitTests
{
    [TestFixture]
    public class ValidatorTestClass
    {
        [Test]
        public void TestMethod_ValidInt()
        {
            FieldValidationHandler<int> fieldValidationHandler = new FieldValidationHandler<int>((x) => true, null);

            Assert.IsTrue(fieldValidationHandler.Validate(1));
        }

        [Test]
        public void TestMethod_ValidString()
        {
            FieldValidationHandler<string> fieldValidationHandler = new FieldValidationHandler<string>((x) => true, null);

            Assert.IsTrue(fieldValidationHandler.Validate("12345"));
        }

        [Test]
        public void TestMethod_ValidNullString()
        {
            FieldValidationHandler<string> fieldValidationHandler = new FieldValidationHandler<string>((x) => true, null);

            Assert.IsTrue(fieldValidationHandler.Validate(null as string));
        }

        [Test]
        public void TestMethod_ValidNullPredicate()
        {
            FieldValidationHandler<string> fieldValidationHandler = new FieldValidationHandler<string>(null, null);

            Assert.IsTrue(fieldValidationHandler.Validate("test"));
        }
		//nothing
    }
}