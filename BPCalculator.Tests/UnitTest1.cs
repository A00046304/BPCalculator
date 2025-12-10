using System;
using BPCalculator;
using Xunit;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        // ---------- VALIDATE() TESTS ----------

        // Cover both sides of the Systolic < min  OR  > max check
        [Theory]
        [InlineData(69, 80)]   // below min
        [InlineData(191, 80)]  // above max
        public void Validate_InvalidSystolic_ThrowsArgumentOutOfRange(int sys, int dia)
        {
            var bp = new BloodPressure
            {
                Systolic = sys,
                Diastolic = dia
            };

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => bp.Validate());
            Assert.Equal("Systolic", ex.ParamName);
        }

        // Cover both sides of the Diastolic < min  OR  > max check
        [Theory]
        [InlineData(120, 39)]  // below min
        [InlineData(120, 101)] // above max
        public void Validate_InvalidDiastolic_ThrowsArgumentOutOfRange(int sys, int dia)
        {
            var bp = new BloodPressure
            {
                Systolic = sys,
                Diastolic = dia
            };

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => bp.Validate());
            Assert.Equal("Diastolic", ex.ParamName);
        }

        // Cover the Systolic <= Diastolic branch when both values are in range
        [Theory]
        [InlineData(90, 90)]
        [InlineData(95, 100)]
        public void Validate_SystolicNotGreaterThanDiastolic_ThrowsInvalidOperation(int sys, int dia)
        {
            var bp = new BloodPressure
            {
                Systolic = sys,
                Diastolic = dia
            };

            var ex = Assert.Throws<InvalidOperationException>(() => bp.Validate());
            Assert.Contains("Systolic must be greater than diastolic", ex.Message);
        }

        // Valid case: all three checks are FALSE (no exception)
        [Fact]
        public void Validate_ValidReading_DoesNotThrow()
        {
            var bp = new BloodPressure
            {
                Systolic = 120,
                Diastolic = 80
            };

            bp.Validate(); // If anything is wrong, this will throw and fail the test.
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

        // ---------- MEDICATION MESSAGE TESTS ----------

        [Fact]
        public void MedicationMessage_ForHigh()
        {
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };
            Assert.Contains("doctor", bp.MedicationMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void MedicationMessage_ForPreHigh()
        {
            var bp = new BloodPressure { Systolic = 130, Diastolic = 85 };
            Assert.Contains("Monitor regularly", bp.MedicationMessage);
        }

        [Fact]
        public void MedicationMessage_ForIdeal()
        {
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };
            Assert.Contains("No medication needed", bp.MedicationMessage);
        }

        [Fact]
        public void MedicationMessage_ForLow()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 55 };
            Assert.Contains("Increase fluids or salt", bp.MedicationMessage);
        }

        // Basic constructor sanity test (nice extra coverage)
        [Fact]
        public void Constructor_SetsValues()
        {
            var bp = new BloodPressure(120, 80);

            Assert.Equal(120, bp.Systolic);
            Assert.Equal(80, bp.Diastolic);
        }
    }
}
