using System;
using System.ComponentModel;

namespace Demo.InsuranceCalculation.Data
{
    /// <summary>
    /// Represents a claim taken out by a customer.
    /// </summary>
    public class Claim
    {
        /// <summary>
        /// Gets or sets the date the claim was taken out.
        /// </summary>
        [DisplayName("Date Of Claim")]
        public DateTime DateOfClaim { get; set; }
    }
}
