using BoxingApp.Models;

namespace BoxingApp
{
    //just to clarify I hate global variables... but, I need one for this app. 
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