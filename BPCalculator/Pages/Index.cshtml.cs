using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        [BindProperty]
        public BloodPressure BP { get; set; } = new BloodPressure();

        public string? ErrorMessage { get; set; }

        public void OnPost()
        {
            try
            {
                BP.Validate();
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
