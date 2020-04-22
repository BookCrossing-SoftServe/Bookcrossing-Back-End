using Application.Dto.Comment.Book;
using BookCrossingBackEnd.Validators.Comment.Book;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System.Collections.Generic;

namespace ApplicationTest.Validators.Comment.Book
{
    [TestFixture]
    class ChildInsertValidatorTest
    {
        private ChildInsertValidator _validator;

        [OneTimeSetUp]
        public void Setup()
        {
            _validator = new ChildInsertValidator();
        }

        [Test]
        public void ChildDeleteValidator_ModelInvalid_ThrowsException()
        {
            var invalidDto = new ChildInsertDto();
            var result = _validator.TestValidate(invalidDto);
            result.ShouldHaveAnyValidationError();
        }

        [Test]
        public void ChildDeleteValidator_ModelValid_DontThrowsException()
        {
            var validDto = new ChildInsertDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0" },
                OwnerId = 1,
                Text = "Text"

            };
            var result = _validator.TestValidate(validDto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        #region Ids

        [Test]
        public void Ids_ElementsBatFormat_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Ids, new List<string>() { "1", "2", "3" });
        }

        [Test]
        public void Ids_Null_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Ids, null as List<string>);
        }

        [Test]
        public void Ids_ContainNullElement_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Ids, new List<string>() { null, "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf0" });
        }

        [Test]
        public void Ids_CountLessThen1Elements_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Ids, new List<string>() { });
        }

        [Test]
        public void Ids_CountGreaterThen1Elements_ShouldNotThrowsException()
        {
            _validator.ShouldNotHaveValidationErrorFor(dto => dto.Ids, new List<string>() { "5e9c9ee859231a63bc853bf1" });
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