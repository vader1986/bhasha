using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using Bhasha.Web.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
	public class ExpressionManagerTests
	{
		private IRepository<Expression> _repository = default!;
		private ExpressionManager _manager = default!;

		[SetUp]
		public void Before()
		{
			_repository = A.Fake<IRepository<Expression>>();
			_manager = new ExpressionManager(_repository);
		}

		[Test, AutoData]
		public async Task GivenNewExpression_WhenAddOrUpdate_ThenAddedToRepository(Expression expression)
		{
			// setup
			A.CallTo(() => _repository
				.Find(A<System.Linq.Expressions.Expression<Func<Expression, bool>>>.Ignored))
				.Returns(Array.Empty<Expression>());

			var validExpression = expression.WithReference();

			// act
			await _manager.AddOrUpdate(validExpression);

			// verify
			A.CallTo(() => _repository.Add(validExpression)).MustHaveHappenedOnceExactly();
		}

		[Test, AutoData]
		public void GivenInvalidExpression_WhenAddOrUpdate_ThenThrow(Expression expression)
        {
			// setup
			var invalidExpression = expression.WithoutReference();

			// act & verify
			Assert.ThrowsAsync<ArgumentException>(async () => await _manager.AddOrUpdate(invalidExpression));
		}

		[Test, AutoData]
		public async Task GivenUpdatedExpression_WhenAddOrUpdate_ThenUpdatedRepository(Expression expression, Expression existingExpression)
		{
			// setup
			A.CallTo(() => _repository
				.Find(A<System.Linq.Expressions.Expression<Func<Expression, bool>>>.Ignored))
                .Returns(new[] { existingExpression });

			var validExpression = expression.WithReference();
			
			// act
			await _manager.AddOrUpdate(validExpression);

			// verify
			A.CallTo(() => _repository.Update(existingExpression.Id, A<Expression>.Ignored))
				.MustHaveHappenedOnceExactly();
		}
	}
}

