namespace BoxingApp.Models
{
    public class Profile
    {
        public Profile(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }

        public Settings Settings;
    }
}