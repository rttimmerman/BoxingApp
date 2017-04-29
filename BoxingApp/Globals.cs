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
    //just to clarify I fucking hate global variables... but, I need one for this app. - fuck off
    //didn't feel like messing with callbacks and intent extras
    public static class Globals
    {
        private static Profile _selectedProfile;
        public static Profile SelectedProfile
        {
            get { return _selectedProfile ?? Constants.DefaultProfile; }
            set { _selectedProfile = value; }
        }
    }
}