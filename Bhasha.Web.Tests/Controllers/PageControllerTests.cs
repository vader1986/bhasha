using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Controllers;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Controllers
{
    [TestFixture]
    public class PageControllerTests
    {
        private IDatabase _database;
        private IStore<ChapterStats> _stats;
        private IStore<GenericChapter> _chapters;
        private IAuthorizedProfileLookup _profiles;
        private IEvaluateSubmit _evaluator;
        private PageController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _chapters = A.Fake<IStore<GenericChapter>>();
            _profiles = A.Fake<IAuthorizedProfileLookup>();
            _evaluator = A.Fake<IEvaluateSubmit>();

            _controller = new PageController(_database, _stats, _profiles, _evaluator);
        }

        [Test]
        public async Task Submit([Values]Result result)
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 1, "something");

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var evaluation = new Evaluation(result, submit);

            A.CallTo(() => _evaluator.Evaluate(profile, A<Submit>.That.Matches(x => x.Equals(submit))))
                .Returns(Task.FromResult(evaluation));

            var eval = await _controller.Submit(
                profile.Id, submit.ChapterId, submit.PageIndex, submit.Solution);

            Assert.That(eval, Is.EqualTo(evaluation));
        }

        [Test]
        public async Task Tip()
        {
            var profile = ProfileBuilder.Default.Build();

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));


        }
    }
}
