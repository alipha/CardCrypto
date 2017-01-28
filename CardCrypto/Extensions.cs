using System;

namespace CardCrypto
{
    public static class Extensions
    {
        public static Suit GetSuit(this int card)
        {
            return (Suit)((card - 1) / 13);
        }

        public static bool IsBlack(this int card)
        {
            return card <= 26;
        }

        public static bool IsRed(this int card)
        {
            return !card.IsBlack();
        }

        public static int ToBlackCard(this char letter)
        {
            var isLower = (letter >= 'a' && letter <= 'z');
            return letter - (isLower ? 'a' : 'A') + 1;
        }

        public static int ToRedCard(this char letter)
        {
            return letter.ToBlackCard() + 26;
        }

        public static char ToLetter(this int card)
        {
            return (char)('A' + (card - 1) % 26);
        }

        public static string CardName(this int card)
        {
            var faceValue = (card - 1) % 13 + 1;
            char faceSymbol;

            switch(faceValue) 
            {
                case 1:  faceSymbol = 'A'; break;
                case 10: faceSymbol = 'T'; break;
                case 11: faceSymbol = 'J'; break;
                case 12: faceSymbol = 'Q'; break;
                case 13: faceSymbol = 'K'; break;
                default:
                    faceSymbol = (char)(faceValue + '0');
                    break;
            }


            if (card <= 13)
                return faceSymbol + "S";

            if (card <= 26)
                return faceSymbol + "C";

            if (card <= 39)
                return faceSymbol + "H";

            return faceSymbol + "D";
        }

        public static int ToCard(this string cardName)
        {
            int faceValue;

            switch (cardName[0])
            {
                case 'A': faceValue = 1;  break;
                case 'T': faceValue = 10; break;
                case 'J': faceValue = 11; break;
                case 'Q': faceValue = 12; break;
                case 'K': faceValue = 13; break;
                default:
                    faceValue = cardName[0] - '0';
                    break;
            }

            switch (cardName[1])
            {
                case 'S': return faceValue;
                case 'C': return faceValue + 13;
                case 'H': return faceValue + 26;
                case 'D': return faceValue + 39;
            }

            throw new ArgumentOutOfRangeException("cardName", cardName, "The card name does not match the regex [A2-9TJQK][SCHD]");
        }
    }
}
