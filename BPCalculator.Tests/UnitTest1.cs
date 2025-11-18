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
        [InlineData(100, 120)]
        [InlineData(120, 120)]
        public void Validate_InvalidRelation_Throws(int sys, int dia)
        {
            var bp = new BloodPressure(sys, dia);


            Assert.Throws<ArgumentOutOfRangeException>(() => bp.Validate());
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
    }
}