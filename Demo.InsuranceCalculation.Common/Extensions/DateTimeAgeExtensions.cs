using System;

namespace Demo.InsuranceCalculation.Extensions
{
    /// <summary>
    /// Provides extension methods for calculating age in years.
    /// </summary>
    public static class DateTimeAgeExtensions
    {
        /// <summary>
        /// Calculates how old a person will be in years today given their date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The age in years.</returns>
        public static int GetAgeToday(this DateTime dateOfBirth)
        {
            return GetAgeAtDate(dateOfBirth, DateTime.Today);
        }

        /// <summary>
        /// Calculates how old a person will be in years on a specified date given their date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="targetDate">The specified date.</param>
        /// <returns>The age in years.</returns>
        public static int GetAgeAtDate(this DateTime dateOfBirth, DateTime targetDate)
        {
            //(yyyyMMdd - yyyyMMdd) / 10000
            var a = (targetDate.Year * 100 + targetDate.Month) * 100 + targetDate.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
