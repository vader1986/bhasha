using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Controllers;
using Bhasha.Web.Services;
using LazyCache;
using Moq;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Controllers
{
    [TestFixture]
    public class PageControllerTests
    {
        private Mock<IAppCache> _cache;
        private Mock<IAuthorizedProfileLookup> _profiles;
        private Mock<IEvaluateSubmit> _evaluator;
        private Mock<IProvideTips> _tipsProvider;
        private PageController _controller;

        [SetUp]
        public void Before()
        {
            _cache = new Mock<IAppCache>();
            _profiles = new Mock<IAuthorizedProfileLookup>();
            _evaluator = new Mock<IEvaluateSubmit>();
            _tipsProvider = new Mock<IProvideTips>();
            _controller = new PageController(
                _cache.Object,
                _profiles.Object,
                _evaluator.Object,
                _tipsProvider.Object);
        }

        [Test]
        public async Task Submit_PageSolutionForProfile([Values]Result result)
        {
            // setup
            var profile = ProfileBuilder.Default.Build();

            _profiles
                .Setup(x => x.Get(profile.Id, _controller.UserId))
                .ReturnsAsync(profile);

            var submit = SubmitBuilder.Default.Build();
            var evaluation = new Evaluation(result, submit, profile);

            _evaluator
                .Setup(x => x.Evaluate(profile, submit))
                .ReturnsAsync(evaluation);

            // act
            var eval = await _controller.Submit(
                profile.Id, submit.ChapterId, submit.PageIndex, submit.Solution);

            // assert
            Assert.That(eval, Is.EqualTo(evaluation));
        }
        /*
        [Test]
        public async Task Submit_updated_profile([Values] Result result)
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 1, "something");
            var updatedProfile = new Profile(
                profile.Id,
                profile.UserId,
                profile.From,
                profile.To,
                profile.Level + 1,
                profile.CompletedChapters + 1);

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var evaluation = new Evaluation(result, submit, updatedProfile);

            A.CallTo(() => _evaluator.Evaluate(profile, A<Submit>.That.Matches(x => x.Equals(submit))))
                .Returns(Task.FromResult(evaluation));

            await _controller.Submit(
                profile.Id, submit.ChapterId, submit.PageIndex, submit.Solution);

            A.CallTo(() => _cache.Remove(profile.Id.ToString()))
                .MustHaveHappenedOnceExactly();
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
        */
    }
}
