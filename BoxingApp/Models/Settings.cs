using System.Collections.Generic;
using Java.Security;

namespace BoxingApp.Models
{
    public class Settings
    {
        public Settings(int prepTime, int roundTime, int restTime, int numRounds, int maxCombo, bool screenOn, List<string> comboSounds)
        {
            PreparationTime = prepTime;
            RoundTime = roundTime;
            RestTime = restTime;
            NumberOfRounds = numRounds;
            MaxCombo = maxCombo;
            KeepScreenOn = screenOn;
            ComboSounds = comboSounds;

        }

        public int PreparationTime;
        public int RoundTime;
        public int RestTime;
        public int NumberOfRounds;
        public int MaxCombo;
        public bool KeepScreenOn;
        public List<string> ComboSounds;
    }
}