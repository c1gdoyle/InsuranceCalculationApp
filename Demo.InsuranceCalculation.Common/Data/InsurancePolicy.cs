using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Gets or sets the 
        /// </summary>
        public IEnumerable<Driver> Drivers
        {
            get;
            private set;
        }
    }
}
