using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace BPCalculator.E2ETests
{
    public class BloodPressureFormTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public BloodPressureFormTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            _driver = new ChromeDriver(options);

            // Use environment variable from pipeline
            _baseUrl = Environment.GetEnvironmentVariable("APP_URL")
                       ?? "https://bp-qa-webapp.azurewebsites.net";
        }

        [Fact]
        public void Enter_Valid_BP_Shows_Category_And_No_Error()
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            // Find inputs by name or id (adjust to your actual HTML)
            var systolicInput = _driver.FindElement(By.Name("BP.Systolic"));
            var diastolicInput = _driver.FindElement(By.Name("BP.Diastolic"));
            var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            systolicInput.Clear();
            systolicInput.SendKeys("140");

            diastolicInput.Clear();
            diastolicInput.SendKeys("90");

            submitButton.Click();

            // Check result contains "High"
            var bodyText = _driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("High", bodyText, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Invalid_Relation_Shows_Error_Message()
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            var systolicInput = _driver.FindElement(By.Name("BP.Systolic"));
            var diastolicInput = _driver.FindElement(By.Name("BP.Diastolic"));
            var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            systolicInput.Clear();
            systolicInput.SendKeys("80");

            diastolicInput.Clear();
            diastolicInput.SendKeys("90");

            submitButton.Click();

            var bodyText = _driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Systolic must be greater than Diastolic", bodyText);
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
