using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SplitList.Test
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.ApkFile("C:\\Users\\tcars\\source\\repos\\4SemesterProjekt\\Repository\\SplitList\\SplitList.Test\\com.companyname.splitlist.apk").StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}