using System;
using System.Collections.Generic;
using System.Timers;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BoxingApp.Helpers;
using BoxingApp.Models;
using Settings = BoxingApp.Models.Settings;
using Android.Speech.Tts;
using BoxingApp.Enums;
using Java.Util;
using Timer = System.Timers.Timer;

namespace BoxingApp
{
    public class MainFragment : NavigationFragment, TextToSpeech.IOnInitListener
    {
        #region Clock Shit
        private string _clock;
        public string Clock
        {
            get { return _clock; }
            set
            {
                _clock = value;
                if (_textClock != null)
                {
                    _textClock.Text = _clock;
                }
            }
        }

        private int _currentRound;
        public int CurrentRound
        {
            get { return _currentRound; }
            set
            {
                _currentRound = value;
                if (_textRound != null)
                {
                    _textRound.Text = $"Round {_currentRound}";
                }
            }
        }

        private TimerStatus _currentTimerStatus;
        public TimerStatus CurrentTimerStatus
        {
            get { return _currentTimerStatus; }
            set
            {
                _currentTimerStatus = value;

                if (_textStatus != null)
                {
                    _textStatus.Text = _currentTimerStatus.ToString();
                }
            }
        }

        public Timer Timer;

        private int _counter;

        private int _rounds = Globals.SelectedProfile.Settings.NumberOfRounds;
        private int _prepare = Globals.SelectedProfile.Settings.PreparationTime;
        private int _work = Globals.SelectedProfile.Settings.RoundTime;
        private int _rest = Globals.SelectedProfile.Settings.RestTime;



        #endregion

        private TextToSpeech _tts;

        private List<string> ProfileNames { get; set; }

        private bool IsSoundEnabled => _toggleButtonSound != null && _toggleButtonSound.Checked;

        private Settings Settings => Globals.SelectedProfile.Settings;

        private View _btnStart;
        private ToggleButton _toggleButtonSound;
        private Button _buttonEditProfiles;
        private TextView _textClock;
        private Spinner _spinnerProfiles;
        private TextView _textRound;
        private RelativeLayout _layoutClockAndControls;
        private Button _btnPauseResume;
        private Button _btnStop;
        private TextView _textStatus;
        public override string FragmentTag => "MainFragment";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            DisableScreenTimeout();

            _tts = new TextToSpeech(Activity, this);
            CurrentTimerStatus = TimerStatus.Stopped;
            Timer = new Timer(1000);
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Activity.RunOnUiThread(() =>
            {
                Clock = _counter.ToString();
                _counter--;

                if (!_isPlayingCombo && CurrentTimerStatus == TimerStatus.Working && _counter % 2 == 0 && !_isPlayingCombo)
                {
                    PlayCombo();
                }

                if (_counter != -1) return;

                switch (CurrentTimerStatus)
                {
                    case TimerStatus.Preparing:
                        _counter = _work;
                        CurrentTimerStatus = TimerStatus.Working;
                        break;
                    case TimerStatus.Working:
                        _counter = _rest;
                        CurrentTimerStatus = TimerStatus.Resting;
                        if (CurrentRound == _rounds)
                        {
                            Stop();
                            return;
                        }
                        CurrentRound++;
                        break;
                    case TimerStatus.Resting:
                        _counter = _work;
                        CurrentTimerStatus = TimerStatus.Working;
                        break;
                }

                Speak(CurrentTimerStatus.ToString());
            });
        }

        private void DisableScreenTimeout()
        {
            Activity.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
        }

        private void Speak(string words)
        {
            if (IsSoundEnabled)
            {
                _tts.Speak(words, QueueMode.Flush, null);
            }
        }

        private void PlayCombo()
        {
            _isPlayingCombo = true;

            Speak("jab, hook");

            _isPlayingCombo = false;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.main_fragment, container, false);

            GetUiWidgets(v);
            SetUIEventHandlers();
            SetupSpinner();

            LoadSelectedProfileSettings();

            return v;
        }

        private void SetupSpinner()
        {
            ProfileNames = SettingsHelper.GetProfileNames();

            var profileAdapter = new ArrayAdapter<string>(Activity,
                Android.Resource.Layout.SimpleSpinnerItem, ProfileNames);

            profileAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _spinnerProfiles.Adapter = profileAdapter;
        }


        private void LoadSelectedProfileSettings()
        {
            Globals.SelectedProfile = SettingsHelper.GetProfileSettings(_spinnerProfiles.SelectedItem.ToString());
            Stop();
        }

        private void SetUIEventHandlers()
        {
            _btnStart.Click += Start;
            _btnPauseResume.Click += PauseResume;
            _btnStop.Click += Stop;
            _buttonEditProfiles.Click += _buttonEditProfiles_Click;
            _spinnerProfiles.ItemSelected += _spinnerProfiles_ItemSelected;
        }

        private void Stop(object sender = null, EventArgs e = null)
        {
            _btnStart.Visibility = ViewStates.Visible;
            _btnStop.Visibility = ViewStates.Gone;
            _btnPauseResume.Visibility = ViewStates.Gone;

            Clock = "Done";
            CurrentRound = 1;
            Timer.Stop();
            CurrentTimerStatus = TimerStatus.Stopped;
            _btnPauseResume.Text = "Pause";
        }

        private TimerStatus _timerStatusBeforePause;
        private bool _isPlayingCombo = false;

        private void PauseResume(object sender = null, EventArgs e = null)
        {
            if (CurrentTimerStatus != TimerStatus.Paused)
            {
                Timer.Stop();
                _timerStatusBeforePause = CurrentTimerStatus;
                CurrentTimerStatus = TimerStatus.Paused;
                _btnPauseResume.Text = "Resume";
            }
            else
            {
                CurrentTimerStatus = _timerStatusBeforePause;
                Timer.Start();
                _btnPauseResume.Text = "Pause";
            }

        }

        private void _spinnerProfiles_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            LoadSelectedProfileSettings();
        }

        private void _buttonEditProfiles_Click(object sender, EventArgs e)
        {
            var editProfilesFragment = new ProfileSettingsFragment();

            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.mainFragmentContainer, editProfilesFragment, FragmentTag);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

        private void Start(object sender = null, EventArgs e = null)
        {
            _btnStart.Visibility = ViewStates.Gone;
            _btnPauseResume.Visibility = ViewStates.Visible;
            _btnStop.Visibility = ViewStates.Visible;

            _counter = _prepare;
            CurrentRound = 1;
            CurrentTimerStatus = TimerStatus.Preparing;
            Timer.Start();
        }

        private void GetUiWidgets(View v)
        {
            _btnStart = v.FindViewById<Button>(Resource.Id.btnStart);
            _btnPauseResume = v.FindViewById<Button>(Resource.Id.btnPauseResume);
            _btnStop = v.FindViewById<Button>(Resource.Id.btnStop);
            _toggleButtonSound = v.FindViewById<ToggleButton>(Resource.Id.toggleButtonSound);
            _buttonEditProfiles = v.FindViewById<Button>(Resource.Id.buttonEditProfiles);
            _textClock = v.FindViewById<TextView>(Resource.Id.textClock);
            _textRound = v.FindViewById<TextView>(Resource.Id.textRound);
            _textStatus = v.FindViewById<TextView>(Resource.Id.textStatus);
            _spinnerProfiles = v.FindViewById<Spinner>(Resource.Id.spinnerProfiles);
            _layoutClockAndControls = v.FindViewById<RelativeLayout>(Resource.Id.layoutClockAndControls);

            _toggleButtonSound.Checked = true;
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if (status == OperationResult.Success)
            {
                _tts.SetLanguage(Locale.Us);
            }
        }
    }
}