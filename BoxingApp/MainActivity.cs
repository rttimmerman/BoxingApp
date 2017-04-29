using Android.App;
using Android.Content.PM;
using Android.OS;

namespace BoxingApp
{
    [Activity(Label = "Boxing Timer", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected int GetLayoutResId => Resource.Layout.main_activity;
        protected int SingleFragmentContainerId => Resource.Id.mainFragmentContainer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(GetLayoutResId);

            var fm = FragmentManager;
            var fragment = fm.FindFragmentById(SingleFragmentContainerId);

            if (fragment == null)
            {
                fragment = new MainFragment();

                var tag = ((NavigationFragment)fragment).FragmentTag;
                fm.BeginTransaction()
                    .Add(SingleFragmentContainerId, fragment, tag)
                    .Commit();
            }
        }
    }
}

