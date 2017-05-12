using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using BoxingApp.Helpers;
using BoxingApp.Models;

namespace BoxingApp
{
    public class ProfileSettingsFragment : NavigationFragment
    {
        public List<string> ProfileNames { get; set; }

        private Button _btnAddProfile;
        private Button _btnDeleteProfile;
        private Button _btnComboSounds;
        private Button _btnSaveProfile;
        private Button _btnCancelProfile;
        private ToggleButton _tglBtnScreenAlwaysOn;
        private Spinner _spinnerProfiles;
        private EditText _editTxtRoundTime;
        private EditText _editTxtPrepTime;
        private EditText _editTxtNumOfRounds;
        private EditText _editTxtRestTime;
        private EditText _editTxtMaxCombo;

        public override string FragmentTag => "ProfileSettingsFragment";

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.profile_settings_fragment, container, false);

            GetUiWidgets(v);
            SetupSpinner();
            SetUIEventHandlers();

            return v;
        }

        private void SetUIEventHandlers()
        {
            _spinnerProfiles.ItemSelected += _spinnerProfiles_ItemSelected;

            _btnCancelProfile.Click += Cancel;
            _btnAddProfile.Click += AddProfile;
            _btnSaveProfile.Click += SaveProfile;
            _btnComboSounds.Click += DisplayComboSoundsFragment;
            _btnDeleteProfile.Click += DeleteProfile;
        }

        private void DeleteProfile(object sender, EventArgs e)
        {
            var alert = new AlertDialog.Builder(Activity);
            alert.SetCancelable(true);

            alert.SetTitle("Delete Profile");
            alert.SetMessage($"Are you sure you would like to delete profile: {_spinnerProfiles.SelectedItem}?");

            alert.SetPositiveButton("Yes", (senderAlert, args) => SettingsHelper.DeleteProfile(_spinnerProfiles.SelectedItem.ToString()));

            Activity.RunOnUiThread(() =>
            {
                alert.Show();
            });
            
        }

        private void DisplayComboSoundsFragment(object sender, System.EventArgs e)
        {
            var soundsFragment = new SoundsFragment();

            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.mainFragmentContainer, soundsFragment, FragmentTag);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

        private void SaveProfile(object sender, System.EventArgs e)
        {
            var round = int.Parse(_editTxtRoundTime.Text);
            var prep = int.Parse(_editTxtPrepTime.Text);
           var numRounds = int.Parse(_editTxtNumOfRounds.Text);
            var rest = int.Parse(_editTxtRestTime.Text);
            var maxCombo = int.Parse(_editTxtMaxCombo.Text);
            var screenOn = _tglBtnScreenAlwaysOn.Checked;

            var profileToSave = new Profile(_spinnerProfiles.SelectedItem.ToString(),
                new Settings(prep, round, rest, numRounds, maxCombo, screenOn, new List<string>()));

            SettingsHelper.SaveProfileSettings(profileToSave);
        }

        private void AddProfile(object sender = null, System.EventArgs e = null)
        {
            var layoutInflater = Activity.LayoutInflater;
            var promptView = layoutInflater.Inflate(Resource.Layout.input_dialog, null);
            var alertDialogBuilder = new AlertDialog.Builder(Activity);
            alertDialogBuilder.SetView(promptView);

            var editText = promptView.FindViewById<EditText>(Resource.Id.editTxtProfileName);
            // setup a dialog window
            alertDialogBuilder.SetCancelable(false);

            alertDialogBuilder.SetPositiveButton("OK", (s, a) =>
            {
                var profileName = editText.Text;

                if (string.IsNullOrWhiteSpace(profileName))
                {
                    DialogHelper.DisplayMessage(Activity, "Profile name may not be empty.");
                    return;
                }

                var newProfile = Constants.DefaultProfile;
                newProfile.Name = profileName;

                SettingsHelper.SaveProfileSettings(newProfile);

                var prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
                var editor = prefs.Edit();
                editor.PutString("SelectedProfile", _spinnerProfiles.SelectedItem.ToString());
                editor.Apply();

                SetupSpinner();
            });

            // create an alert dialog
            var alert = alertDialogBuilder.Create();
            alert.Show();
        }

        private void Cancel(object sender = null, System.EventArgs e = null)
        {
            FragmentManager.PopBackStack();
        }

        private void _spinnerProfiles_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            LoadSelectedProfileSettings();
        }

        private void LoadSelectedProfileSettings()
        {
            Globals.SelectedProfile = SettingsHelper.GetProfileSettings(_spinnerProfiles.SelectedItem.ToString());

            _editTxtRoundTime.Text = Globals.SelectedProfile.Settings.RoundTime.ToString();
            _editTxtPrepTime.Text = Globals.SelectedProfile.Settings.PreparationTime.ToString();
            _editTxtNumOfRounds.Text = Globals.SelectedProfile.Settings.NumberOfRounds.ToString();
            _editTxtRestTime.Text = Globals.SelectedProfile.Settings.RestTime.ToString();
            _editTxtMaxCombo.Text = Globals.SelectedProfile.Settings.MaxCombo.ToString();

            _tglBtnScreenAlwaysOn.Checked = Globals.SelectedProfile.Settings.KeepScreenOn;
        }

        private void SetupSpinner()
        {
            ProfileNames = SettingsHelper.GetProfileNames();

            var profileAdapter = new ArrayAdapter<string>(Activity,
                Android.Resource.Layout.SimpleSpinnerItem, ProfileNames);

            profileAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _spinnerProfiles.Adapter = profileAdapter;
        }


        private void GetUiWidgets(View v)
        {
            _btnAddProfile = v.FindViewById<Button>(Resource.Id.btnAddProfile);
            _btnDeleteProfile = v.FindViewById<Button>(Resource.Id.btnDeleteProfile);
            _btnCancelProfile = v.FindViewById<Button>(Resource.Id.btnCancelProfile);
            _btnSaveProfile = v.FindViewById<Button>(Resource.Id.btnSaveProfile);
            _btnComboSounds= v.FindViewById<Button>(Resource.Id.btnComboSounds);

            _tglBtnScreenAlwaysOn = v.FindViewById<ToggleButton>(Resource.Id.toggleButtonScreenAlwaysOn);

            _spinnerProfiles = v.FindViewById<Spinner>(Resource.Id.spinnerSettingsProfiles);

            _editTxtPrepTime = v.FindViewById<EditText>(Resource.Id.editTxtPreparationTime);
            _editTxtRoundTime = v.FindViewById<EditText>(Resource.Id.editTxtRoundTime);
            _editTxtRestTime = v.FindViewById<EditText>(Resource.Id.editTxtRestTime);
            _editTxtNumOfRounds = v.FindViewById<EditText>(Resource.Id.editTxtNumberOfRounds);
            _editTxtMaxCombo = v.FindViewById<EditText>(Resource.Id.editTxtMaxCombo);
        }
    }
}