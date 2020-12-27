using System;
using System.Linq;
using TonSdk.Extensions.NodeSe;
using Xunit;

namespace TonSdk.Tests
{
    public class EnvDependentTheoryAttribute : TheoryAttribute
    {
        public string[] EnvVariableNames { get; set; }

        public EnvDependentTheoryAttribute(params string[] envVariableNames)
        {
            EnvVariableNames = envVariableNames;
        }

        public EnvDependentTheoryAttribute() : this(TonClientNodeSe.NodeSeNetworkAddressEnvVar)
        {
        }

        public override string Skip
        {
            get
            {
                return EnvVariableNames.Any(name => string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                    ? $"To enable this test, specify the following environment variables: {string.Join(", ", EnvVariableNames)}"
                    : base.Skip;
            }
            set => base.Skip = value;
        }
    }
}
