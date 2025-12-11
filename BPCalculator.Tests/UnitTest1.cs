using System;
using Xunit;
using BPCalculator;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {

        [Theory]
        [InlineData(69, 60)]
        [InlineData(191, 80)]
        public void Validate_InvalidSystolicRange_Throws(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Throws<ArgumentOutOfRangeException>(() => bp.Validate());
        }

        [Theory]
        [InlineData(120, 39)]
        [InlineData(120, 101)]
        public void Validate_InvalidDiastolicRange_Throws(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Throws<ArgumentOutOfRangeException>(() => bp.Validate());
        }

        [Theory]
        [InlineData(100, 100)]
        [InlineData(80, 90)]
        public void Validate_SystolicNotGreater_Throws(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Throws<InvalidOperationException>(() => bp.Validate());
        }

        [Theory]
        [InlineData(150, 95)]
        [InlineData(140, 60)]
        [InlineData(120, 95)]
        public void Category_High(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(BPCategory.High, bp.Category);
        }

        [Theory]
        [InlineData(130, 70)]
        [InlineData(100, 85)]
        [InlineData(120, 79)]
        public void Category_PreHigh(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(BPCategory.PreHigh, bp.Category);
        }

        [Theory]
        [InlineData(100, 70)]
        [InlineData(95, 65)]
        [InlineData(110, 78)]
        public void Category_Ideal(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(BPCategory.Ideal, bp.Category);
        }

        [Theory]
        [InlineData(80, 55)]
        [InlineData(75, 50)]
        [InlineData(89, 59)]
        public void Category_Low(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        [Theory]
        [InlineData(150, 95, "Seek medical advice regarding BP medication.")]
        [InlineData(140, 92, "Seek medical advice regarding BP medication.")]
        public void MedicationMessage_High(int sys, int dia, string expected)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(expected, bp.MedicationMessage);
        }

        [Theory]
        [InlineData(130, 70, "Monitor closely, you may require medication soon.")]
        [InlineData(100, 85, "Monitor closely, you may require medication soon.")]
        public void MedicationMessage_PreHigh(int sys, int dia, string expected)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(expected, bp.MedicationMessage);
        }

        [Theory]
        [InlineData(100, 70, "No medication needed.")]
        [InlineData(95, 65, "No medication needed.")]
        public void MedicationMessage_Ideal(int sys, int dia, string expected)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(expected, bp.MedicationMessage);
        }

        [Theory]
        [InlineData(80, 55, "Increase fluids or salt if recommended by your doctor.")]
        [InlineData(75, 50, "Increase fluids or salt if recommended by your doctor.")]
        public void MedicationMessage_Low(int sys, int dia, string expected)
        {
            var bp = new BloodPressure(sys, dia);
            Assert.Equal(expected, bp.MedicationMessage);
        }

        [Fact]
        public void Constructor_SetsValues()
        {
            var bp = new BloodPressure(120, 80);
            Assert.Equal(120, bp.Systolic);
            Assert.Equal(80, bp.Diastolic);
        }
    }
}
