using Application.Dto.Comment.Book;
using BookCrossingBackEnd.Validators.Comment.Book;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace ApplicationTest.Validators.Comment.Book
{
    [TestFixture]
    class RootInsertValidatorTest
    {
        private RootInsertValidator _validator;

        [OneTimeSetUp]
        public void Setup()
        {
            _validator = new RootInsertValidator();
        }

        [Test]
        public void ChildDeleteValidator_ModelInvalid_ThrowsException()
        {
            var invalidDto = new RootInsertDto();
            var result = _validator.TestValidate(invalidDto);
            result.ShouldHaveAnyValidationError();
        }

        [Test]
        public void ChildDeleteValidator_ModelValid_DontThrowsException()
        {
            var validDto = new RootInsertDto()
            {
                OwnerId = 1,
                BookId = 1,
                Text = "Text"

            };
            var result = _validator.TestValidate(validDto);
            result.ShouldNotHaveAnyValidationErrors();
        }


        #region BookId

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void BookId_LessOrEqualToZero_ThrowsException(int value)
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.BookId, value);
        }

        [Test]
        public void BookId_GreaterThanZero_ShouldNotThrowsException()
        {
            _validator.ShouldNotHaveValidationErrorFor(dto => dto.BookId, 1);
        }

        #endregion

        #region OwnerId

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void OwnerId_LessOrEqualToZero_ThrowsException(int value)
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.OwnerId, value);
        }

        [Test]
        public void OwnerId_GreaterThanZero_ShouldNotThrowsException()
        {
            _validator.ShouldNotHaveValidationErrorFor(dto => dto.OwnerId, 1);
        }

        #endregion

        #region Text

        [Test]
        public void Text_NotNull_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Text, null as string);
        }

        [Test]
        public void Text_ValidLength_ShouldNotThrowsException()
        {
            _validator.ShouldNotHaveValidationErrorFor(dto => dto.Text, "        Text          ");
        }

        [Test]
        public void Text_LengthLessThen1_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Text, "");
        }

        [Test]
        public void Text_OnlyWhiteSpaces_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Text, "        ");
        }
        #endregion
    }
}
