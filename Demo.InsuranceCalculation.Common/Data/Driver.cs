using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Demo.InsuranceCalculation.Data
{
    /// <summary>
    /// Represents a driver associated with an insurance policy.
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// Gets or sets the name of this driver.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the occupation of this driver.
        /// </summary>
        public Occupation Occupation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date of birth of this driver.
        /// </summary>
        public DateTime DateOfBirth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the claims associated with this driver, if any.
        /// </summary>
        public IList<Claim> Claims
        {
            get;
            set;
        }
    }
}
