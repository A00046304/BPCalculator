using System;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    public enum BPCategory
    {
        [Display(Name = "Low Blood Pressure")] Low,
        [Display(Name = "Ideal Blood Pressure")] Ideal,
        [Display(Name = "Pre-High Blood Pressure")] PreHigh,
        [Display(Name = "High Blood Pressure")] High
    }

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax)]
        public int Systolic { get; set; }

        [Range(DiastolicMin, DiastolicMax)]
        public int Diastolic { get; set; }

        public BloodPressure() { }

        public BloodPressure(int systolic, int diastolic)
        {
            Systolic = systolic;
            Diastolic = diastolic;
        }

        public void Validate()
        {
            if (Systolic < SystolicMin || Systolic > SystolicMax)
                throw new ArgumentOutOfRangeException(nameof(Systolic));

            if (Diastolic < DiastolicMin || Diastolic > DiastolicMax)
                throw new ArgumentOutOfRangeException(nameof(Diastolic));

            if (Systolic <= Diastolic)
                throw new InvalidOperationException("Systolic must be greater than diastolic.");
        }

        public BPCategory Category
        {
            get
            {
                if (Systolic >= 140 || Diastolic >= 90)
                    return BPCategory.High;

                if (Systolic >= 120 || Diastolic >= 80)
                    return BPCategory.PreHigh;

                if (Systolic >= 90 && Diastolic >= 60)
                    return BPCategory.Ideal;

                return BPCategory.Low;
            }
        }

        // NEW: Medication message
        public string MedicationMessage
        {
            get
            {
                return Category switch
                {
                    BPCategory.High => "Consider consulting a doctor about BP medication.",
                    BPCategory.PreHigh => "Monitor regularly, medication may be needed soon.",
                    BPCategory.Ideal => "No medication needed.",
                    BPCategory.Low => "Increase fluids or salt if recommended by your doctor.",
                    _ => string.Empty
                };
            }
        }
    }
}

