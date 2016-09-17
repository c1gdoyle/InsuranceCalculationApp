using System.Collections.Generic;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.PolicyDeclination;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.Services
{
    /// <summary>
    /// An implementation of <see cref="IPolicyDeclinationService"/>.
    /// </summary>
    public class PolicyDeclinationService : IPolicyDeclinationService
    {
        private readonly IList<IPolicyDeclinationRule> _rules;

        /// <summary>
        /// Intiailises a new default instance of <see cref="PolicyDeclinationService"/>
        /// </summary>
        public PolicyDeclinationService()
        {
            _rules = new List<IPolicyDeclinationRule>();
        }

        #region IPolicyDeclinationService Members
        public void RegisterPolicyDeclinationRule(IPolicyDeclinationRule rule)
        {
            _rules.Add(rule);
        }

        public InsurancePolicyApplicationResult AssessPolicy(InsurancePolicy policy)
        {
            InsurancePolicyApplicationResult result = new InsurancePolicyApplicationResult { IsPolicyApproved = true };

            foreach(var rule in _rules)
            {
                result = rule.AssessPolicy(policy);
                if(!result.IsPolicyApproved)
                {
                    return result;
                }
            }
            return result;
        }
        #endregion IPolicyDeclinationService Members
    }
}
