
using System.Collections.Generic;
using BookCrossingBackEnd.Filters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Filters
{
    [TestFixture]
    internal class ModelValidationFilterTest
    {
        private ModelStateDictionary _modelState;
        private ModelValidationFilter _filter;
        private ActionContext _actionContext;
        private ActionExecutingContext _actionExecutingContext;


        [SetUp]
        public void Setup()
        {
            _modelState = new ModelStateDictionary();
            _filter = new ModelValidationFilter();

            _actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                _modelState
            );
            _actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );
        }
        [Test]
        public void OnActionExecuting_ValidModel_ResponseIsNotSet()
        {
            _actionExecutingContext.ModelState.Clear();

            _filter.OnActionExecuting(_actionExecutingContext);

            _actionExecutingContext.Result.Should().BeNull();
        }

        [Test]
        public void OnActionExecuting_InvalidModel_ResponseIsSetToBedRequestObject()
        {
            _modelState.AddModelError("unitTest", "Model is invalid");

            _filter.OnActionExecuting(_actionExecutingContext);

            _actionExecutingContext.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
