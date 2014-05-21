﻿using Demo.Library.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Results
{
    [TestFixture]
    public class AuthorizeActionResultTests
    {
        private AuthorizeActionResult _result;

        [SetUp]
        public void Setup()
        {
            var action = new Moq.Mock<IAction>();
            action.Setup(a => a.Description).Returns("Mock");
            _result = new AuthorizeActionResult(action.Object);
        }

        [Test]
        public void Process_authorized_has_no_targets()
        {
            Assert.True(_result.IsAuthorized);
            Assert.AreEqual(0, _result.AuthorizationFailures.Count());
        }

        [Test]
        public void Process_authorized_has_no_failures()
        {
            var target = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeTargetResult(target.Object);

            Assert.AreEqual(0, _result.AuthorizationFailures.Count());
        }

        [Test]
        public void Process_not_authorized_has_one_failure()
        {
            var target = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target.Setup(x => x.IsAuthorized).Returns(false);

            _result.ProcessAuthorizeTargetResult(target.Object);

            Assert.AreEqual(1, _result.AuthorizationFailures.Count());
        }

        [Test]
        public void Process_authorized_is_authorized()
        {
            var target = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeTargetResult(target.Object);

            Assert.True(_result.IsAuthorized);
        }

        [Test]
        public void Process_not_authorized_is_not_authorized()
        {
            var target = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target.Setup(x => x.IsAuthorized).Returns(false);

            _result.ProcessAuthorizeTargetResult(target.Object);

            Assert.False(_result.IsAuthorized);
        }

        [Test]
        public void Authorized_has_no_failed_messages()
        {
            var target = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeTargetResult(target.Object);

            Assert.AreEqual(0, _result.BuildFailedAuthorizationMessages().Count());
        }

        [Test]
        public void Not_authorized_has_one_failed_messages()
        {
            var target = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target.Setup(x => x.IsAuthorized).Returns(false);
            target.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test" });

            _result.ProcessAuthorizeTargetResult(target.Object);

            Assert.AreEqual(1, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void Not_authorized_has_many_failed_messages()
        {
            var target = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target.Setup(x => x.IsAuthorized).Returns(false);
            target.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test", "test2" });

            _result.ProcessAuthorizeTargetResult(target.Object);

            Assert.AreEqual(2, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void One_authorized_one_not()
        {
            var target1 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target1.Setup(x => x.IsAuthorized).Returns(false);
            var target2 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeTargetResult(target1.Object);
            _result.ProcessAuthorizeTargetResult(target2.Object);

            Assert.False(_result.IsAuthorized);
        }

        [Test]
        public void One_authorized_one_not_one_failure()
        {
            var target1 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target1.Setup(x => x.IsAuthorized).Returns(false);
            var target2 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeTargetResult(target1.Object);
            _result.ProcessAuthorizeTargetResult(target2.Object);

            Assert.AreEqual(1, _result.AuthorizationFailures.Count());
        }
        [Test]
        public void One_authorized_one_not_one_message()
        {
            var target1 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target1.Setup(x => x.IsAuthorized).Returns(false);
            target1.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test" });
            var target2 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeTargetResult(target1.Object);
            _result.ProcessAuthorizeTargetResult(target2.Object);

            Assert.AreEqual(1, _result.BuildFailedAuthorizationMessages().Count());
        }
        [Test]
        public void One_authorized_one_not_two_message()
        {
            var target1 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target1.Setup(x => x.IsAuthorized).Returns(false);
            target1.Setup(x => x.BuildFailedAuthorizationMessages()).Returns(new List<String> { "test", "test1" });
            var target2 = new Moq.Mock<AuthorizeTargetResult>(Moq.It.IsAny<ITarget>());
            target2.Setup(x => x.IsAuthorized).Returns(true);

            _result.ProcessAuthorizeTargetResult(target1.Object);
            _result.ProcessAuthorizeTargetResult(target2.Object);

            Assert.AreEqual(2, _result.BuildFailedAuthorizationMessages().Count());
        }
    }
}