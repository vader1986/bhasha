using System;
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
				await _translationManager.AddOrUpdate(translation, default));
        }

		/*
		 * ToDo:
		 * - unit tests for AddOrUpdate method
		 */
	}
}

