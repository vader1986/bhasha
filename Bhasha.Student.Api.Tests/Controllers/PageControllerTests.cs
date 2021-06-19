using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Bhasha.Student.Api.Controllers;
using Bhasha.Student.Api.Services;
using LazyCache;
using Moq;
using NUnit.Framework;

namespace Bhasha.Student.Api.Tests.Controllers
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
            _cache
                .Setup(x => x.DefaultCachePolicy)
                .Returns(new CacheDefaults());
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
        public async Task Submit_PageSolutionForProfile([Values] Result result)
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

        [Test]
        public async Task Submit_ForChangingProfileStats_UpdatesProfile([Values] Result result)
        {
            // setup
            var profile = ProfileBuilder.Default.Build();

            _profiles
                .Setup(x => x.Get(profile.Id, _controller.UserId))
                .ReturnsAsync(profile);

            var updatedProfile = ProfileBuilder.Default
                .WithLevel(profile.Level + 1)
                .Build();

            var submit = SubmitBuilder.Default.Build();
            var evaluation = new Evaluation(result, submit, updatedProfile);

            _evaluator
                .Setup(x => x.Evaluate(profile, submit))
                .ReturnsAsync(evaluation);

            // act
            await _controller.Submit(
                profile.Id, submit.ChapterId, submit.PageIndex, submit.Solution);

            // assert
            _cache
                .Verify(x => x.Remove(profile.Id.ToString()), Times.Once);
        }

        [Test]
        public async Task Tip_ForChapterPage()
        {
            // setup
            const string expectedTip = "hello world";
            var profile = ProfileBuilder.Default.Build();

            _profiles
                .Setup(x => x.Get(profile.Id, _controller.UserId))
                .ReturnsAsync(profile);

            var chapterId = Guid.NewGuid();
            var pageIndex = Rnd.Create.Next(0, 10);

            _tipsProvider
                .Setup(x => x.GetTip(profile, chapterId, pageIndex))
                .ReturnsAsync(expectedTip);

            // act
            var result = await _controller.Tip(profile.Id, chapterId, pageIndex);

            // assert
            Assert.That(result, Is.EqualTo(expectedTip));
        }
    }
}
