using System.Collections.Generic;

namespace WpfApp1
{
    public class Character
    {
        public string Name { get; set; }

        public string ProfileImage { get; set; }

        public List<ChatMessage> ChatHistory { get; set; }

        public Character(string name, string profileImage)
        {
            Name = name;
            ProfileImage = profileImage;

            ChatHistory = new List<ChatMessage>();
        }
    }
}