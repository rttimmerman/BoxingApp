using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace BoxingApp
{
    public class SoundsFragment : NavigationFragment
    {
        private ListView _soundListView;

        public override string FragmentTag => "SoundFragment";

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.main_fragment, container, false);

            GetUiWidgets(v);
            SetUIEventHandlers();

            _soundListView.Adapter = new SoundAdapter(Activity, new List<string>());

            return v;
        }

        private void SetUIEventHandlers()
        {

        }

        private void GetUiWidgets(View v)
        {
            _soundListView = v.FindViewById<ListView>(Resource.Id.listViewSounds);
        }
    }
}