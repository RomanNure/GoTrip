using System;

using Android.Runtime;

using Newtonsoft.Json;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    [JsonObject]
    [Preserve(AllMembers = true)]
    public class User : ModelElement
    {
        [JsonRequired]
        public long id { get; set; }

        [GetProfileFiled]
        public long userId { get; set; }
        
        [LogInField]
        [SignUpField]
        [JsonRequired]
        public string login { get; set; }

        [LogInField]
        [SignUpField]
        [JsonIgnore]
        public string password { get; set; }

        [SignUpField]
        [JsonRequired]
        public string email { get; set; }

        public string fullName { get; set; }

        public string phone { get; set; }

        public DateTime registrationDatetime { get; set; }

        public bool emailConfirmed { get; set; }

        public string avatarUrl { get; set; }

        public User() { }
        public User(long id) { userId = id; this.id = id; }
        public User(string login, string password, string email = "")
        {
            this.login = login;
            this.password = password;
            this.email = email;
        }
    }
}
