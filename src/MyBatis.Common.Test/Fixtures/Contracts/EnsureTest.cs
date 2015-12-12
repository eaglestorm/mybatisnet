using MyBatis.Common.Contracts;
using MyBatis.Common.Contracts.Exceptions;
using Is = MyBatis.Common.Contracts.Constraints.Is;
using NUnit.Common;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MyBatis.Common.Test.Fixtures.Contracts
{
    [TestFixture]
    public class EnsureTest
    {

        [Test]        
        public void Boolean_condition_should_raise_exception_on_when_false()
        {
            Assert.Throws<PostConditionException>(delegate { Contract.Ensure.That(1 == 2).When("checking false value"); });
        }

        [Test]       
        public void Should_raise_exception_when_and_constraints_not_verified()
        {
            Assert.Throws<PostConditionException>(delegate { Contract.Ensure.That("toto", Is.EqualTo("titi") & Is.Empty).When("checking string"); });
        }

    }
}
