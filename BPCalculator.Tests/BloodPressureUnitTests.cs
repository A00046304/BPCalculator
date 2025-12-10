using System;
using BPCalculator;
using Xunit;

namespace BPCalculator.Tests
{
    public class BloodPressureUnitTests
    {
        // ---------- VALIDATION TESTS ----------

        [Fact]
        public void Validate_DoesNotThrow_ForValidReading()
        {
            // All values inside allowed min/max range
            var bp = new BloodPressure
            {
                Systolic = 120,
                Diastolic = 80
            };

            // If any of the three checks failed, this would throw.
            bp.Validate();
        }

        [Fact]
        public void Validate_Throws_WhenSystolicLessOrEqualDiastolic_WithinRange()
        {
            // Both within min/max range so only the third check should fire
            var bp = new BloodPressure
            {
                Systolic = 90,
                Diastolic = 95
            };

            var ex = Assert.Throws<InvalidOperationException>(() => bp.Validate());
            Assert.Contains("Systolic must be greater than diastolic", ex.Message);
        }

        // ---------- CATEGORY TESTS ----------

        [Fact]
        public void Category_ReturnsHigh()
        {
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };
            Assert.Equal(BPCategory.High, bp.Category);
        }

        [Fact]
        public void Category_ReturnsPreHigh()
        {
            var bp = new BloodPressure { Systolic = 130, Diastolic = 85 };
            Assert.Equal(BPCategory.PreHigh, bp.Category);
        }

        [Fact]
        public void Category_ReturnsIdeal()
        {
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };
            Assert.Equal(BPCategory.Ideal, bp.Category);
        }

        [Fact]
        public void Category_ReturnsLow()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 55 };
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        // ---------- MEDICATION MESSAGE TESTS (cheap extra coverage) ----------

        [Fact]
        public void MedicationMessage_ReturnsExpected_ForHigh()
        {
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };
            Assert.Contains("consulting a doctor", bp.MedicationMessage);
        }

        [Fact]
        public void MedicationMessage_ReturnsExpected_ForPreHigh()
        {
            var bp = new BloodPressure { Systolic = 130, Diastolic = 85 };
            Assert.Contains("Monitor regularly", bp.MedicationMessage);
        }

        [Fact]
        public void MedicationMessage_ReturnsExpected_ForIdeal()
        {
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };
            Assert.Contains("No medication needed", bp.MedicationMessage);
        }

        [Fact]
        public void MedicationMessage_ReturnsExpected_ForLow()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 55 };
            Assert.Contains("Increase fluids or salt", bp.MedicationMessage);
        }
    }
}
