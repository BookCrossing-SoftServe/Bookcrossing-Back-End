using Application.Dto.Comment.Book;
using BookCrossingBackEnd.Validators.Comment.Book;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace ApplicationTest.Validators.Comment.Book
{
    [TestFixture]
    class RootDeleteValidatorTest
    {
        private RootDeleteValidator _validator;

        [OneTimeSetUp]
        public void Setup()
        {
            _validator = new RootDeleteValidator();
        }

        [Test]
        public void ChildDeleteValidator_ModelInvalid_ThrowsException()
        {
            var invalidDto = new RootDeleteDto();
            var result = _validator.TestValidate(invalidDto);
            result.ShouldHaveAnyValidationError();
        }

        [Test]
        public void ChildDeleteValidator_ModelValid_DontThrowsException()
        {
            var validDto = new RootDeleteDto()
            {
                Id = "5e9c9ee859231a63bc853bf0",
                OwnerId = 1,
            };
            var result = _validator.TestValidate(validDto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        #region Id

        [Test]
        public void Id_NotNull_ThrowsException()
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Id, null as string);
        }

        [Test]
        [TestCase("")]
        [TestCase("1")]
        [TestCase("                        ")]
        [TestCase("QQQQ12311111311111111111")]
        [TestCase("1r1231111131wee1111111111")]
        public void Id_BedFormat_ShouldNotThrowsException(string value)
        {
            _validator.ShouldHaveValidationErrorFor(dto => dto.Id, value);
        }

        [Test]
        [TestCase("111112311111311111111111")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase("5e9c9ee859231a63bc853bf0")]
        public void Id_LengthLessThen1_ThrowsException(string value)
        {
            _validator.ShouldNotHaveValidationErrorFor(dto => dto.Id, value);
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
    }
}
