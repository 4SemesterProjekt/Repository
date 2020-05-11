using System;
using System.Text;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SplitList.Test
{
    public class AppInitializer
    {
        private static string test =
            "C:\\Users\\tcars\\source\\repos\\4SemesterProjekt\\Repository\\SplitList\\SplitList.Test\\com.companyname.splitlist.apk";

        private static string debug =
            "C:\\Users\\tcars\\source\\repos\\4SemesterProjekt\\Repository\\SplitList\\SplitList\\SplitList.Android\\bin\\Debug\\com.companyname.splitlist.apk";
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.ApkFile(test).StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}