using System;
using System.Text;
using System.Security.Cryptography;

using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.Model
{
    public class TicketChecker : ModelElement
    {
        public const char QR_DATA_SEPARATOR = ';';

        [CheckTicketField("userId")]
        public long UserId { get; private set; }

        [CheckTicketField("tourId")]
        public long TourId { get; private set; }

        [CheckTicketField("guideId")]
        public long GuideId { get; private set; }

        [CheckTicketField("hash")]
        public string Hash { get; private set; }

        public TicketChecker(string qrData, Guide guide, Tour tour, HashAlgorithm hashAlgorithm)
        {
            string[] datas = (qrData).Split(QR_DATA_SEPARATOR);

            if (tour.id != Convert.ToInt64(datas[1]))
                throw new Exception();

            UserId = Convert.ToInt64(datas[0]);
            TourId = Convert.ToInt64(datas[1]);
            GuideId = guide.id;

            Hash = BitConverter.ToString(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes($"{guide.id}_{datas[2]}"))).Replace("-", "").ToUpper();
        }
    }
}
