namespace WpfApp1
{
    public class Character
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }

        public Character(string name, string profileImage)
        {
            Name = name;
            ProfileImage = profileImage;
        }
    }
}