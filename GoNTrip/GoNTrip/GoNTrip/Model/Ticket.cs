using System.Text;
using System.Security.Cryptography;
using System;

namespace GoNTrip.Model
{
    public class Ticket : ModelElement
    {
        public long UserId { get; private set; }
        public long TourId { get; private set; }
        public string Hash { get; private set; }

        public Ticket(User user, Tour tour, Participating participating, string key, HashAlgorithm hashAlgorithm)
        {
            UserId = user.id;
            TourId = tour.id;

            string hashString = $"{key}_{user.id}_{tour.id}_{participating.orderId}_{key}";
            Hash = BitConverter.ToString(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(hashString))).Replace("-", "").ToUpper();
        }

        public string Data => $"{UserId};{TourId};{Hash}";
    }
}
