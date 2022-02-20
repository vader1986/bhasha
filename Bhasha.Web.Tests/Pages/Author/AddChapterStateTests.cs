using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Pages.Author;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Pages.Author
{
	public class AddChapterStateTests
	{
		private AddChapterState _state = default!;

		/*
		 * ToDo:
		 * - throw exception leads to error text
		 * - name & description added to translations
		 * - handling of references
		 */

		[SetUp]
		public void Before()
		{
			_state = new AddChapterState();
			_state.ChapterRepository = A.Fake<IRepository<Chapter>>();
			_state.TranslationManager = A.Fake<ITranslationManager>();
			_state.UserId = "Test User 123";
			_state.Name = "Introduction";
			_state.Description = "First steps ...";
			_state.NativeLanguage = Language.English;
			_state.TargetLanguage = Language.Bengali;
			_state.RequiredLevel = 1;
			_state.Pages.Add(new Page(PageType.ClozeChoice, Guid.NewGuid(), Array.Empty<string>()));
			_state.Pages.Add(new Page(PageType.ClozeChoice, Guid.NewGuid(), Array.Empty<string>()));
			_state.Pages.Add(new Page(PageType.ClozeChoice, Guid.NewGuid(), Array.Empty<string>()));
		}

		[Test, AutoData]
		public async Task GivenAddPageState_WhenSubmitPageState_ThenAddPage(AddPageState pageState)
		{
			// setup
			_state.Pages.Clear();

			// act
			await _state.SubmitPageState(pageState);

			// verify
			Assert.That(_state.Pages.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task GivenAllData_WhenSubmit_ThenNoErrorAndAddChapter()
		{
			// act
			await _state.Submit();

			// verify
			Assert.IsNull(_state.Error);
			A.CallTo(() => _state.ChapterRepository.Add(A<Chapter>.Ignored))
				.MustHaveHappenedOnceExactly();
		}

		[Test]
		public async Task GivenMissingUserId_WhenSubmit_ThenSetError()
        {
			// setup
			_state.UserId = null;

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("Unknown user! Make sure you're logged in!", _state.Error);
        }

		[Test]
		public async Task GivenMissingName_WhenSubmit_ThenSetError()
		{
			// setup
			_state.Name = null;

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("NAME must not be empty!", _state.Error);
		}

		[Test]
		public async Task GivenMissingDescription_WhenSubmit_ThenSetError()
		{
			// setup
			_state.Description = null;

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("DESCRIPTION must not be empty!", _state.Error);
		}

		[Test]
		public async Task GivenMissingRequiredLevel_WhenSubmit_ThenSetError()
		{
			// setup
			_state.RequiredLevel = null;

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("Please select a REQUIRED LEVEL!", _state.Error);
		}

		[Test]
		public async Task GivenMissingNativeLanguage_WhenSubmit_ThenSetError()
		{
			// setup
			_state.NativeLanguage = null;

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("Please select a NATIVE LANGUAGE!", _state.Error);
		}

		[Test]
		public async Task GivenMissingTargetLanguage_WhenSubmit_ThenSetError()
		{
			// setup
			_state.TargetLanguage = null;

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("Please select a TARGET LANGUAGE!", _state.Error);
		}

		[Test]
		public async Task GivenTargetLanguageEqualToNativeLanguage_WhenSubmit_ThenSetError()
		{
			// setup
			_state.TargetLanguage = _state.NativeLanguage;

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("TARGET language must be different from NATIVE language!", _state.Error);
		}

		[Test]
		public async Task GivenMissingPages_WhenSubmit_ThenSetError()
		{
			// setup
			_state.Pages.Clear();

			// act
			await _state.Submit();

			// verify
			Assert.AreEqual("A chapter requires at least 3 chapters!", _state.Error);
		}
	}
}

