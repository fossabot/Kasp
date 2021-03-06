using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kasp.Core.Extensions;
using Kasp.Localization.EF.Tests.Data;
using Kasp.Localization.EF.Tests.Models;
using Kasp.Test;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Kasp.Localization.EF.Tests.Tests {
	public class EntityControllerTest : KClassFixtureWebApp<StartupDbLocalization> {
		public EntityControllerTest(ITestOutputHelper output, KWebAppFactory<StartupDbLocalization> factory) : base(output, factory) {
			PostRepository = Factory.Server.Host.Services.GetService<PostRepository>();

			PostRepository.AddAsync(new Post {Title = "سلام", LangId = "fa-IR"}).Preserve();
			PostRepository.AddAsync(new Post {Title = "Hello", LangId = "en-US"}).Preserve();
			PostRepository.SaveAsync().Preserve();
		}

		private PostRepository PostRepository { get; }

		[Theory]
		[InlineData("fa-IR")]
		[InlineData("en-US")]
		public async Task Items(string culture) {
			var response = await Client.GetAsync($"/api/Entity/List?culture={culture}&ui-culture={culture}");
			var items = await response.Content.ReadAsAsync<Post[]>();
			Assert.True(items.All(x => x.LangId == culture));
		}
	}
}