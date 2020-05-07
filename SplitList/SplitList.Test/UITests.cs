using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SplitList.Test
{
    [TestFixture(Platform.Android)]
    public class UITests
    {
        IApp app;
        Platform platform;

        public UITests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void IsDisplayed_LoginBtn()
        {
            app.Repl();
            AppResult[] results = app.Query(view => view.Marked("LoginBtn"));
            Assert.IsTrue(results.Any());
        }
    }
}
