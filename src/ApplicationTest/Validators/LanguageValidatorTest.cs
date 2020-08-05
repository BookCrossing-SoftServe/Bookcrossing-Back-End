using BookCrossingBackEnd.Validators;
using Domain.RDBMS.Entities;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace ApplicationTest.Validators
{
    [TestFixture]
    public class LanguageValidatorTest
    {
        private LanguageValidator _validator;
        private static string[] _namesWithForbiddenCharacters = {"-English","_German_", "E'-glsh", "E.", "..." };

        [SetUp]
        public void Setup()
        {
            _validator = new LanguageValidator();
        }

        [Test]
        public void LanguageName_IsNull_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(lang =>lang.Name, null as string);
        }

        [Test]
        public void LanguageName_IsNotNull_ShouldNotThrowException()
        {
            _validator.ShouldNotHaveValidationErrorFor(language =>language.Name, "English");
        }

        [Test]
        public void LanguageName_LessThanTwoCharacters_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(language => language.Name, "J");
        }

        [Test]
        public void LanguageName_MoreThanTwentyCharacters_ShouldThrowException()
        {
            _validator.ShouldHaveValidationErrorFor(language => language.Name, "Ennnnnnnnnnnggggggllliiiiiissssshhhh");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        public void IdProperty_LessThanZero_ThrowsException(int value)
        {
            _validator.ShouldHaveValidationErrorFor(language => language.Id, value);
        }

        [Test, TestCaseSource(nameof(_namesWithForbiddenCharacters))]
        public void LanguageName_ContainsForbiddenCharacters_ThrowsException(string value)
        {
            _validator.ShouldHaveValidationErrorFor(language => language.Name, value);
        }
    }
}
