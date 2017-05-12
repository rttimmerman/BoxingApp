namespace BoxingApp.Models
{
    public class Profile
    {
        public Profile(string name, Settings settings)
        {
            Name = name;
            Settings = settings;
        }

        public string Name;

        public Settings Settings;
    }
}