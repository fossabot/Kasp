using Kasp.Test;
using Xunit;
using Xunit.Abstractions;

namespace Kasp.FormBuilder.Tests {
    public class UnitTest1 : KClassFixtureWebApp<Startup> {
        public UnitTest1(ITestOutputHelper output, KWebAppFactory<Startup> factory) : base(output, factory) {
        }
        
        [Fact]
        public void Test1() {
        }
    }
}