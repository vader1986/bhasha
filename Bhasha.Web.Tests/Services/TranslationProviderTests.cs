using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
	public class TranslationProviderTests
	{
		private IRepository<Translation> _repository = default!;
		private TranslationProvider _translationProvider = default!;

		[SetUp]
		public void Before()
        {
			_repository = A.Fake<IRepository<Translation>>();
			_translationProvider = new TranslationProvider(_repository);
		}

		[Test]
		public async Task GivenNoMatchingTranslation_WhenFind_ThenReturnNull()
        {
			// setup
			A.CallTo(() => _repository.Find(A<System.Linq.Expressions.Expression<Func<Translation, bool>>>.Ignored))
				.Returns(Array.Empty<Translation>());

			// act
			var result = await _translationProvider.Find(Guid.NewGuid(), Language.Bengali);

			// verify
			Assert.IsNull(result);
        }

		[Test, AutoData]
		public async Task GivenTranslation_WhenFind_ThenReturnTranslation(Translation translation)
        {
			// setup
			A.CallTo(() => _repository.Find(A<System.Linq.Expressions.Expression<Func<Translation, bool>>>.Ignored))
                .Returns(new[] { translation });

			// act
			var result = await _translationProvider.Find(Guid.NewGuid(), Language.Bengali);

			// verify
			Assert.AreEqual(translation, result);
		}

		[Test, AutoData]
		public void GivenSomeExpressions_WhenFindAll_ThenThrowException(Translation[] translations)
		{
			// setup
			A.CallTo(() => _repository.Find(A<System.Linq.Expressions.Expression<Func<Translation, bool>>>.Ignored))
				.Returns(translations);

			var guids = Enumerable.Range(0, translations.Length - 1).Select(_ => Guid.NewGuid()).ToArray();

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await _translationProvider.FindAll(Language.Bengali, guids));
		}

		[Test, AutoData]
		public async Task GivenAllExpressionsWithTranslations_WhenFindAll_ThenReturnTranslations(Translation[] translations)
		{
			// setup
			A.CallTo(() => _repository.Find(A<System.Linq.Expressions.Expression<Func<Translation, bool>>>.Ignored))
				.Returns(translations);

			var guids = Enumerable.Range(0, translations.Length).Select(_ => Guid.NewGuid()).ToArray();

			// act
			var result = await _translationProvider.FindAll(Language.Bengali, guids);

			// verify
			Assert.That(result.Count, Is.EqualTo(guids.Length));
		}
	}
}

