using Android.App;

namespace BoxingApp
{
    public abstract class NavigationFragment : Fragment
    {
        public abstract string FragmentTag { get; }
    }
}