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
		private IRepository<Expression> _expressionRepository = default!;
		private TranslationProvider _translationProvider;

		[SetUp]
		public void Before()
        {
			_expressionRepository = A.Fake<IRepository<Expression>>();
			_translationProvider = new TranslationProvider(_expressionRepository);
		}

		[Test]
		public async Task GivenNoExpression_WhenFind_ThenReturnNull()
        {
			// setup
			A.CallTo(() => _expressionRepository.Get(A<Guid>.Ignored)).Returns(default(Expression));

			// act
			var result = await _translationProvider.Find(Guid.NewGuid(), Language.Bengali);

			// verify
			Assert.IsNull(result);
        }

		[Test, AutoData]
		public async Task GivenNoMatchingTranslation_WhenFind_ThenReturnNull(Expression expression)
        {
			// setup
			A.CallTo(() => _expressionRepository.Get(A<Guid>.Ignored)).Returns(expression with { Translations = new Translation[0]});

			// act
			var result = await _translationProvider.Find(Guid.NewGuid(), Language.Bengali);

			// verify
			Assert.IsNull(result);
		}

		[Test, AutoData]
		public async Task GivenTranslation_WhenFind_ThenReturnTranslation(Expression expression, Translation translation)
        {
			// setup
			translation = translation with { Language = Language.Bengali };
			expression = expression with { Translations = expression.Translations.Append(translation).ToArray() };

			A.CallTo(() => _expressionRepository.Get(expression.Id)).Returns(expression);

			// act
			var result = await _translationProvider.Find(expression.Id, Language.Bengali);

			// verify
			Assert.AreEqual(translation, result);
		}

		[Test, AutoData]
		public void GivenSomeExpressions_WhenFindAll_ThenThrowException(Expression[] expressions)
		{
			// setup
			A.CallTo(() => _expressionRepository.GetMany(A<Guid[]>.Ignored))
				.Returns(Task.FromResult(expressions));

			var guids = expressions.Select(x => x.Id).Append(Guid.NewGuid()).ToArray();

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await _translationProvider.FindAll(Language.Bengali, guids));
		}

		[Test, AutoData]
		public void GivenAllExpressionsWithMissingTranslations_WhenFindAll_ThenThrowException(Expression[] expressions)
		{
			// setup
			expressions = expressions.Select(x => x with { Translations = new Translation[0] }).ToArray();
			A.CallTo(() => _expressionRepository.GetMany(A<Guid[]>.Ignored))
				.Returns(Task.FromResult(expressions));

			var guids = expressions.Select(x => x.Id).ToArray();

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await _translationProvider.FindAll(Language.Bengali, guids));
		}

		[Test, AutoData]
		public async Task GivenAllExpressionsWithTranslations_WhenFindAll_ThenReturnTranslations(Expression[] expressions, Translation translation)
		{
			// setup
			expressions = expressions.Select(x => x with {
				Translations = new[] { translation with { Language = Language.Bengali } } }).ToArray();

			A.CallTo(() => _expressionRepository.GetMany(A<Guid[]>.Ignored))
				.Returns(Task.FromResult(expressions));

			var guids = expressions.Select(x => x.Id).ToArray();

			// act
			var result = await _translationProvider.FindAll(Language.Bengali, guids);

			// verify
			Assert.That(result.Count, Is.EqualTo(guids.Length));
		}
	}
}

