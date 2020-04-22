using Application.Dto;
using BookCrossingBackEnd.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace ApplicationTest.Validators
{
    [TestFixture]
    public class AuthorValidatorTest
    {
        private AuthorValidator _validator;
        private static string[] _namesWithForbiddenCharacters = {"John@", "_John_", "J0hn", "J.", "..."};
        private static string[] _namesWithPermittedCharacters = {"De-John", "De'John"};

        [SetUp]
        public void Setup()
        {
            _validator = new AuthorValidator();
        }


        [Test]
        public void AuthorValidator_ModelInvalid_ThrowsException()
        {
            var invalidAuthorDto = new AuthorDto();
            var result = _validator.TestValidate(invalidAuthorDto);
            result.ShouldHaveAnyValidationError();
        }

        #region ID

        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        public void IdProperty_LessThanZero_ThrowsException(int value)
        {
            _validator.ShouldHaveValidationErrorFor(author => author.Id, value);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void IdProperty_GreaterThanOrEqualToZero_ShouldNotThrowsException(int value)
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.Id, value);
        }

        #endregion ID

        #region FirstName

        [Test]
        public void FirstName_IsNull_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.FirstName, null as string);
        }

        [Test]
        public void FirstName_IsNotNull_ShouldNotThrowException()
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.FirstName, "John");
        }

        [Test]
        public void FirstName_LessThanTwoCharacters_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.FirstName, "J");
        }

        [Test]
        public void FirstName_MoreThanTwentyCharacters_ShouldNotThrowException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.FirstName, "JohnJohnJohnJohnJohnJohn");
        }


        [Test, TestCaseSource(nameof(_namesWithForbiddenCharacters))]
        public void FirstName_ContainsForbiddenCharacters_ThrowsException(string value)
        {
            _validator.ShouldHaveValidationErrorFor(author => author.FirstName, value);
        }

        [Test, TestCaseSource(nameof(_namesWithPermittedCharacters))]
        public void FirstName_ContainsPermittedCharacters_ShouldNotThrowException(string value)
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.FirstName, value);
        }

        #endregion

        #region LastName

        [Test]
        public void LastName_IsNull_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.LastName, null as string);
        }

        [Test]
        public void LastName_IsNotNull_ShouldNotThrowException()
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.LastName, "John");
        }

        [Test]
        public void LastName_LessThanTwoCharacters_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.LastName, "J");
        }

        [Test]
        public void LastName_MoreThanTwentyCharacters_ShouldNotThrowException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.LastName, "JohnJohnJohnJohnJohnJohn");
        }

        [Test, TestCaseSource(nameof(_namesWithForbiddenCharacters))]
        public void LastName_ContainsForbiddenCharacters_ThrowsException(string value)
        {
            _validator.ShouldHaveValidationErrorFor(author => author.LastName, value);
        }

        [Test, TestCaseSource(nameof(_namesWithPermittedCharacters))]
        public void LastName_ContainsPremittedCharacters_ShouldNotThrowException(string value)
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.LastName, value);
        }

        #endregion

        #region MiddleName

        [Test]
        public void MiddleName_IsNull_ShouldNotThrowsException()
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.MiddleName, null as string);
        }

        [Test]
        public void MiddleName_IsNotNull_ShouldNotThrowException()
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.MiddleName, "John");
        }
        [Test]
        public void MiddleName_IsEmpty_ShouldThrowException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.MiddleName, "");
        }
        [Test]
        public void MiddleName_MoreThanThirtyCharacters_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(author => author.MiddleName, "JohnJohnJohnJohnJohnJohnJohnJohn");
        }

        [Test, TestCaseSource(nameof(_namesWithForbiddenCharacters))]
        public void MiddleName_ContainsForbiddenCharacters_ThrowsException(string value)
        {
            _validator.ShouldHaveValidationErrorFor(author => author.MiddleName, value);
        }

        [Test, TestCaseSource(nameof(_namesWithPermittedCharacters))]
        public void MiddleName_ContainsPermittedCharacters_ShouldNotThrowException(string value)
        {
            _validator.ShouldNotHaveValidationErrorFor(author => author.MiddleName, value);
        }

        #endregion
    }
}