using System;
using BPCalculator;
using Reqnroll;
using Xunit;

namespace BPCalculator.BDDTests.Steps
{
    [Binding]
    public class BloodPressureSteps
    {
        private BloodPressure _bp;
        private BPCategory _category;
        private Exception _caughtException;


        [Given(@"I enter a systolic value of (.*)")]
        public void GivenIEnterASystolicValueOf(int systolic)
        {
            _bp ??= new BloodPressure();
            _bp.Systolic = systolic;
        }

        [Given(@"I enter a diastolic value of (.*)")]
        public void GivenIEnterADiastolicValueOf(int diastolic)
        {
            _bp ??= new BloodPressure();
            _bp.Diastolic = diastolic;
        }


        [When(@"I calculate the blood pressure category")]
        public void WhenICalculateTheBloodPressureCategory()
        {
            _category = _bp.Category;
        }

        [When(@"I try to validate the reading")]
        public void WhenITryToValidateTheReading()
        {
            _caughtException = null;

            try
            {
                _bp.Validate();
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
        }


        [Then(@"the result should be ""(.*)""")]
        public void ThenTheResultShouldBe(string expectedCategory)
        {
            Assert.Null(_caughtException);

            Assert.Equal(expectedCategory, _category.ToString());
        }

        [Then(@"an error should be shown")]
        public void ThenAnErrorShouldBeShown()
        {
            Assert.NotNull(_caughtException);
        }
    }
}
