using System;
using System.Collections.Generic;
using System.Linq;
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
        private IAuthorizedProfileLookup _profiles;
        private IEvaluateSubmit _evaluator;
        private IUpdateStatsForTip _stateUpdater;
        private PageController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _profiles = A.Fake<IAuthorizedProfileLookup>();
            _evaluator = A.Fake<IEvaluateSubmit>();
            _stateUpdater = A.Fake<IUpdateStatsForTip>();
            _controller = new PageController(_database, _profiles, _evaluator, _stateUpdater);
        }

        [Test]
        public async Task Submit([Values]Result result)
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 1, "something");

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var evaluation = new Evaluation(result, submit, profile);

            A.CallTo(() => _evaluator.Evaluate(profile, A<Submit>.That.Matches(x => x.Equals(submit))))
                .Returns(Task.FromResult(evaluation));

            var eval = await _controller.Submit(
                profile.Id, submit.ChapterId, submit.PageIndex, submit.Solution);

            Assert.That(eval, Is.EqualTo(evaluation));
        }

        [Test]
        public async Task Tip()
        {
            var chapterId = Guid.NewGuid();
            var profile = ProfileBuilder.Default.Build();

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var from = TranslationBuilder.Default.Build();

            A.CallTo(() => _database.QueryTranslationByTokenId(A<Guid>._, profile.From))
                .Returns(Task.FromResult(from));

            var to = TranslationBuilder.Default.Build();

            A.CallTo(() => _database.QueryTranslationByTokenId(A<Guid>._, profile.To))
                .Returns(Task.FromResult(to));

            var tipIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
            var expectedResult = $"{to.Native} ({to.Spoken}) = {from.Native}";
            
            var result = await _controller.Tip(profile.Id, chapterId, tipIds);

            Assert.That(result, Is.EqualTo(expectedResult));

            A.CallTo(() => _stateUpdater.UpdateStats(chapterId, profile))
                .MustHaveHappenedOnceExactly();
        }
    }
}
