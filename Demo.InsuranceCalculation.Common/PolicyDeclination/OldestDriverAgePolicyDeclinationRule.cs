using System.Linq;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Extensions;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.PolicyDeclination
{
    /// <summary>
    /// An implementation of <see cref="IPolicyDeclinationRule"/> that checks if an insurance
    /// policy application is to be declined or not based on the age of the oldest driver on
    /// the policy. 
    /// </summary>
    public class OldestDriverAgePolicyDeclinationRule : IPolicyDeclinationRule
    {
        /// <summary>
        /// Assesses whether a given a insurance policy should be accepted or
        /// declined based on this rule.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>
        /// A <see cref="InsurancePolicyApplicationResult"/> that representing whether or not
        /// the supplied policy was declined based on this rule.
        /// 
        /// If the policy was not declined the <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will 
        /// be true.
        /// 
        /// If the policy was declined the <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will
        /// be false and the <see cref="InsurancePolicyApplicationResult.Message"/> will contain the details as to
        /// why the policy was declined.
        /// </returns>
        /// <remarks>
        /// If oldest driver is over the age of 75 at the start of the policy the policy will be declined
        /// with the message "Age of Oldest Driver" and the name of the driver appended onto the message.
        /// </remarks>
        public InsurancePolicyApplicationResult AssessPolicy(InsurancePolicy policy)
        {
            //get the youngest driver
            var oldestDriver = policy.Drivers.OrderBy(d => d.DateOfBirth).FirstOrDefault();

            //get their age at the start of the policy
            int age = oldestDriver.DateOfBirth.GetAgeAtDate(policy.StartDate);

            if (age > 75)
            {
                return new InsurancePolicyApplicationResult
                {
                    IsPolicyApproved = false,
                    Message = string.Format("Age of Oldest Driver, {0}", oldestDriver.Name)
                };
            }

            return new InsurancePolicyApplicationResult { IsPolicyApproved = true };
        }
    }
}
