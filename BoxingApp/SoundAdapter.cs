using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace BoxingApp
{
    public class SoundAdapter : ArrayAdapter<string>
    {
        private Activity _context;
        private List<string> _sounds;
        //public SoundAdapter(FragmentActivity context, List<string> sounds) :
        public SoundAdapter(Activity context, List<string> sounds) :
            base(context, Resource.Layout.list_item_sound, sounds)
        {
            _sounds = sounds;
            _context = context;
            //Moves = moves.OrderByDescending(m => m.PickedUp).ThenByDescending(m => m.Completed).ThenByDescending(m => m.Priority).ThenBy(m => m.PieceId).ToList();
        }

        public override bool IsEnabled(int position)
        {
            return true;
        }

        public override bool AreAllItemsEnabled()
        {
            return true;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.list_item_sound, null);
            }

            var sound = _sounds[position];

            var txtSoundName = convertView.FindViewById<TextView>(Resource.Id.txtSoundName);
            var tglBtnSoundOnOff = convertView.FindViewById<ToggleButton>(Resource.Id.tglBtnSoundOnOff);

            return convertView;
        }


        public void UpdateSounds(List<string> sounds)
        {
            _sounds = sounds;
            Clear();
            AddAll(_sounds);
            NotifyDataSetChanged();
        }


    }
}