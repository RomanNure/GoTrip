using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.Model
{
    public class Card
    {
        public string CardNum { get; private set; }
        public string MonthExp { get; private set; }
        public string YearExp { get; private set; }
        public string Cvv { get; private set; }

        public Card(string cardNum, string monthExp, string yearExp, string cvv)
        {
            CardNum = cardNum;
            MonthExp = monthExp;
            YearExp = yearExp;
            Cvv = cvv;
        }
    }
}
