using Kasp.EF.Data;
using Kasp.EF.Localization.Tests.Models;

namespace Kasp.EF.Localization.Tests.Data {
	public class PostRepository : EFBaseRepository<LocalizationDbContext, Post> {
		public PostRepository(LocalizationDbContext db) : base(db) {
		}
	}
}