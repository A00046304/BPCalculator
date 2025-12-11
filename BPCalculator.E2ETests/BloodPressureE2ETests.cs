using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace BPCalculator.E2ETests
{
    public class BloodPressureE2ETests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl;

        public BloodPressureE2ETests()
        {
            _driver = CreateWebDriver();

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(90));
            _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            var envUrl = Environment.GetEnvironmentVariable("BP_E2E_BASEURL");
            // Ensure we always have a proper https:// URL
            _baseUrl = string.IsNullOrWhiteSpace(envUrl)
                ? "https://bp-qa-webapp.azurewebsites.net"
                : (envUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? envUrl
                    : "https://" + envUrl);
        }

        private static IWebDriver CreateWebDriver()
        {
            var browser = (Environment.GetEnvironmentVariable("BROWSER") ?? "chrome")
                .ToLowerInvariant();

            switch (browser)
            {
                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--headless=new");
                    edgeOptions.AddArgument("--no-sandbox");
                    edgeOptions.AddArgument("--disable-dev-shm-usage");
                    edgeOptions.AddArgument("--ignore-certificate-errors");
                    edgeOptions.AddArgument("--allow-insecure-localhost");
                    return new EdgeDriver(edgeOptions);

                default: // chrome
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--headless=new");
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    chromeOptions.AddArgument("--ignore-certificate-errors");
                    chromeOptions.AddArgument("--allow-insecure-localhost");
                    return new ChromeDriver(chromeOptions);
            }
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        private void FillAndSubmit(int systolic, int diastolic)
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            var sys = _wait.Until(d => d.FindElement(By.Id("BP_Systolic")));
            var dia = _driver.FindElement(By.Id("BP_Diastolic"));
            var submit = _driver.FindElement(By.CssSelector("button[type='submit']"));

            sys.Clear();
            sys.SendKeys(systolic.ToString());

            dia.Clear();
            dia.SendKeys(diastolic.ToString());

            submit.Click();
        }

        private string? WaitForCategory()
        {
            try
            {
                var element = _wait.Until(d =>
                    d.FindElement(By.XPath("//p[strong[contains(.,'Category')]]"))
                );
                return element.Text;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        private string? WaitForMedication()
        {
            try
            {
                var element = _wait.Until(d =>
                    d.FindElement(By.XPath("//p[strong[contains(.,'Medication Advice')]]"))
                );
                return element.Text;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        private string? WaitForErrorMessage()
        {
            try
            {
                var element = _wait.Until(d =>
                    d.FindElement(By.CssSelector("div.alert.alert-danger"))
                );
                return element.Text;
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        [Theory]
        [InlineData(150, 95, "High", "Consider consulting a doctor about BP medication.")]
        [InlineData(130, 85, "PreHigh", "Monitor regularly, medication may be needed soon.")]
        [InlineData(110, 70, "Ideal", "No medication needed.")]
        [InlineData(80, 55, "Low", "Increase fluids or salt if recommended by your doctor.")]
        public void ValidInputs_Should_Show_Correct_Category_And_Medication(
            int systolic,
            int diastolic,
            string expectedCategoryFragment,
            string expectedMedicationMessage)
        {
            FillAndSubmit(systolic, diastolic);

            var category = WaitForCategory();
            var medication = WaitForMedication();
            var error = WaitForErrorMessage();

            Assert.Null(error);
            Assert.NotNull(category);
            Assert.NotNull(medication);

            Assert.Contains(expectedCategoryFragment, category);
            Assert.Contains(expectedMedicationMessage, medication);
        }
    }
}
