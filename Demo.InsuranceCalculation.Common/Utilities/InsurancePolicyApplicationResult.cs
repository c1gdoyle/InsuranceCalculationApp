namespace Demo.InsuranceCalculation.Utilities
{
    /// <summary>
    /// Represents the result of an application for an insurance policy.
    /// </summary>
    public class InsurancePolicyApplicationResult
    {
        /// <summary>
        /// Gets or sets whether or not the policy application was approved. 
        /// False indicates that policy was declined.
        /// </summary>
        public bool IsPolicyApproved
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets details of the policy application to display to the user.
        /// </summary>
        /// <remarks>
        /// If policy was declined this property should provide the reason why.
        /// </remarks>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the final premium calculated for the policy application.
        /// </summary>
        /// <remarks>
        /// If application was declined this property should be null.
        /// </remarks>
        public decimal? PremiumAmount
        {
            get;
            set;
        }
    }
}
