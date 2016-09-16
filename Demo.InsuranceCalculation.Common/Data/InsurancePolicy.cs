using System;
using System.Collections.Generic;

namespace Demo.InsuranceCalculation.Data
{
    /// <summary>
    /// Represents an insurance policy.
    /// </summary>
    public class InsurancePolicy
    {
        /// <summary>
        /// Intialises a new instance of <see cref="InsurancePolicy"/> with a specified
        /// start date and collection of drivers associated with the policy.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="drivers">The drivers.</param>
        public InsurancePolicy(DateTime startDate, IEnumerable<Driver> drivers)
        {
            StartDate = startDate;
            Drivers = drivers;
            RegisterAllClaims();
        }

        /// <summary>
        /// Gets or sets the date this policy should start from.
        /// </summary>
        public DateTime StartDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the drivers applying for this policy.
        /// </summary>
        public IEnumerable<Driver> Drivers
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets any prior claims taken out by the drivers 
        /// applying for this policy.
        /// </summary>
        public IEnumerable<Claim> PriorClaims
        {
            get;
            private set;
        }

        private void RegisterAllClaims()
        {
            List<Claim> claims = new List<Claim>();
            foreach(Driver driver in Drivers)
            {
                if(driver.Claims != null && driver.Claims.Count != 0)
                {
                    claims.AddRange(driver.Claims);
                }
            }
            PriorClaims = claims;
        }
    }
}
