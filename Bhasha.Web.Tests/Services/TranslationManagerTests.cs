using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services
{
	public class TranslationManagerTests
	{
		private readonly TranslationManager _translationManager;
		private readonly IRepository<Translation> _translations;
		private readonly IRepository<Expression> _expressions;
		private readonly IFactory<Expression> _expressionFactory;

		public TranslationManagerTests()
        {
			_expressions = Substitute.For<IRepository<Expression>>();
			_expressionFactory = Substitute.For<IFactory<Expression>>();
			_translations = Substitute.For<IRepository<Translation>>();
			_translationManager = new TranslationManager(_translations, _expressions, _expressionFactory);
		}

		[Theory, AutoData]
		public void GivenNonReferenceTranslation_WhenAddOrUpdateWithoutReference_ThenThrowException(Translation translation)
        {
			// setup
			var translationNoRef = translation with { Language = Language.Bengali };

			// act & verify
			Assert.ThrowsAsync<ArgumentException>(async () =>
				await _translationManager.AddOrUpdate(translationNoRef, default));
        }

		[Theory, AutoData]
		public async Task GivenOnlyReferenceTranslation_WhenAddOrUpdate_ThenAddTranslation(Translation translation, Expression expression)
		{
			// setup
			var translationRef = translation with { Language = Language.Reference };

			_translations.Find(default!).ReturnsForAnyArgs(Array.Empty<Translation>());
			_translations.Add(translationRef).Returns(translationRef);

			_expressionFactory.Create().Returns(expression);
			_expressions.Add(expression).Returns(expression);

			// act
			await _translationManager.AddOrUpdate(translationRef, default);

			// verify
			await _translations.Received(1).Add(translationRef with { ExpressionId = expression.Id });
		}

		[Theory, AutoData]
		public async Task GivenTranslationAndReference_WhenAddOrUpdate_ThenAddTranslation(Translation translation, Expression expression)
		{
			// setup
			var translationRef = translation with { Language = Language.Reference };
			var translationNoRef = translation with { Language = Language.Bengali };

			_translations.Find(default!).ReturnsForAnyArgs(Array.Empty<Translation>());
			_translations.Add(translationRef with { ExpressionId = expression.Id }).Returns(translationRef);
			_translations.Add(translationNoRef).Returns(translationNoRef);

			_expressionFactory.Create().Returns(expression);
			_expressions.Add(expression).Returns(expression);

			// act
			await _translationManager.AddOrUpdate(translationNoRef, translationRef);

			// verify
			await _translations.Received(1).Add(translationNoRef with { ExpressionId = translationRef.ExpressionId });
		}
	}
}

