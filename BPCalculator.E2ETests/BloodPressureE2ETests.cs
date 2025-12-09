using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--allow-insecure-localhost");

            _driver = new ChromeDriver(options);

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            _baseUrl = Environment.GetEnvironmentVariable("BP_E2E_BASEURL")
                       ?? "bp-qa-webapp.azurewebsites.net";
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
            return _wait.Until(driver =>
            {
                try
                {
                    var element = _wait.Until(d =>
                        d.FindElement(By.XPath("//p[strong[contains(.,'Category')]]"))
                    );
                    return element.Text;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (WebDriverTimeoutException)
                {
                    return null;
                }

            }
        }

        private string? WaitForMedication()
        {

            return _wait.Until(driver =>
            {

                try
                {
                    var element = _wait.Until(d =>
                        d.FindElement(By.XPath("//p[strong[contains(.,'Medication Advice')]]"))
                    );
                    return element.Text;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (WebDriverTimeoutException)
                {
                    return null;
                }
            }
        }
        private string? WaitForErrorMessage()
        {
            return _wait.Until(driver =>
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
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            }
        }


        [Theory]
        [InlineData(150, 95, "High", "Consider consulting a doctor about BP medication.")]
        [InlineData(130, 85, "PreHigh", "Monitor regularly; medication may be needed soon.")]
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
