using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace BPCalculator.E2ETests
{
    public class BloodPressureE2ETests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public BloodPressureE2ETests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");

            _driver = new ChromeDriver(options);

            // READ QA URL FROM ENV VARIABLE IN PIPELINE
            _baseUrl = Environment.GetEnvironmentVariable("APP_URL")
                       ?? "https://bp-qa-webapp.azurewebsites.net";
        }

        [Theory]
        [InlineData(150, 95, "High")]
        [InlineData(130, 85, "PreHigh")]
        [InlineData(110, 70, "Ideal")]
        [InlineData(80, 55, "Low")]
        public void BP_Form_Submits_And_Shows_Category(int sys, int dia, string expectedCategory)
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            // Fill form
            _driver.FindElement(By.Id("BP_Systolic")).Clear();
            _driver.FindElement(By.Id("BP_Systolic")).SendKeys(sys.ToString());

            _driver.FindElement(By.Id("BP_Diastolic")).Clear();
            _driver.FindElement(By.Id("BP_Diastolic")).SendKeys(dia.ToString());

            // Click submit
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Read category
            var categoryText = _driver.FindElement(By.XPath("//p[strong[text()='Category:']]")).Text;

            Assert.Contains(expectedCategory, categoryText);
        }

        public void Dispose()
        {
            _driver?.Quit();
        }
    }
}
