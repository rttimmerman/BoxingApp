using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BoxingApp.Models;

namespace BoxingApp
{
    public static class Constants
    {
        public const string DefaultProfileName = "Default";

        public static Profile DefaultProfile => new Profile(DefaultProfileName,
            new Settings(3, 3, 3, 3, 3, true, new List<string>() {"Jab","Hook","Cross"}));

        public static string SelectedProfile = nameof(SelectedProfile);
    }
}