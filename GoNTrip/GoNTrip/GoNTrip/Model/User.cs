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
        [UpdateProfileField]
        [GetProfileField]
        [JsonRequired]
        public long id { get; set; }

        [UpdateProfileField]
        [LogInField]
        [SignUpField]
        [JsonRequired]
        public string login { get; set; }

        [UpdateProfileField]
        [LogInField]
        [SignUpField]
        [JsonIgnore]
        public string password { get; set; }

        [UpdateProfileField]
        [SignUpField]
        [JsonRequired]
        public string email { get; set; }

        [UpdateProfileField]
        public string fullName { get; set; }

        [UpdateProfileField]
        public string phone { get; set; }

        [UpdateProfileField]
        public DateTime registrationDatetime { get; set; }

        [UpdateProfileField]
        public bool emailConfirmed { get; set; }

        [UpdateProfileField]
        public string avatarUrl { get; set; }

        [UpdateProfileField]
        public string description { get; set; }

        [UpdateProfileField]
        public Company company { get; set; }

        [UpdateProfileField]
        public Admin administrator { get; set; }

        public User() { }
        public User(long id) { this.id = id; }
        public User(string login, string password, string email = "")
        {
            this.login = login;
            this.password = password;
            this.email = email;
        }

        public void UpdateAvatarUrl(string path) => avatarUrl = path == null ? avatarUrl : path;
    }
}
