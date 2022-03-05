using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PreventAllowAnonymous.Controllers;
using System;
using System.Linq;
using System.Reflection;

namespace PreventAllowAnonymous.Tests.Controllers
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        private static readonly string[] _allowAnonymousPermittedMethods = new string[]
        {
            $"{nameof(WeatherForecastController)}|{nameof(WeatherForecastController.Get)}"
        };

        [TestMethod]
        public void Ensure_Controllers_Do_Not_Have_AllowAnonymousAttribute()
        {
            // Arrange
            var controllerType = typeof(WeatherForecastController);
            var controllerAssembly = Assembly.GetAssembly(controllerType);

            var allowAnonymousDecoratedControllers = controllerAssembly
                .GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.GetCustomAttributes<AllowAnonymousAttribute>(true).Any())
                .ToList();

            // Assert
            CollectionAssert.AreEquivalent(Array.Empty<string>(), allowAnonymousDecoratedControllers);
        }

        [TestMethod]
        public void Ensure_Only_Permitted_Methods_Have_AllowAnonymousAttribute()
        {
            // Arrange
            var controllerType = typeof(WeatherForecastController);
            var controllerAssembly = Assembly.GetAssembly(controllerType);

            var prohibitedMethodsWithAllowAnonymous = controllerAssembly
                .GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(type =>
                {
                    var isAllowAnonymousPresent = type.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
                    var isAllowAnonymousPermitted = !_allowAnonymousPermittedMethods.Contains($"{type.DeclaringType?.Name}|{type.Name}");
                    return isAllowAnonymousPresent && isAllowAnonymousPermitted;
                })
                .ToList();

            // Assert
            CollectionAssert.AreEquivalent(Array.Empty<string>(), prohibitedMethodsWithAllowAnonymous);
        }
    }
}
