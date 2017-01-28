using System;
using System.Linq;
using System.Security.Cryptography;


namespace CardCrypto
{
    public class Deck : ICloneable
    {
        private static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        private static byte[] _randByte = new byte[1];

        private int[] _cards;


        public int[] Cards { get { return _cards; } }


        private Deck()
        {
            _cards = new int[52];
        }

        public static Deck NewEmpty()
        {
            return new Deck();
        }

        public static Deck NewOrdered()
        {
            var deck = new Deck();

            //for (var i = 0; i < deck._cards.Length; i++)
            //    deck._cards[i] = i + 1;

            for (var i = 0; i < 13; i++)
            {
                deck._cards[i * 2]     = i + 40;
                deck._cards[i * 2 + 1] = i + 14;
            }

            for (var i = 0; i < 13; i++)
            {
                deck._cards[i * 2 + 26] = i + 27;
                deck._cards[i * 2 + 27] = i + 1;
            }

            return deck;
        }

        public static Deck NewShuffled()
        {
            var deck = NewOrdered();
            deck.Shuffle();
            return deck;
        }


        public Deck Clone()
        {
            var copy = new Deck();

            for (var i = 0; i < copy.Cards.Length; i++)
                copy.Cards[i] = this.Cards[i];

            return copy;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }


        public override string ToString()
        {
            return string.Join(" ", Cards.Select(x => x.CardName()));
        }

        public void Shuffle()
        {
            for (var i = _cards.Length - 1; i > 0; i--)
            {
                var j = GetRandomInt(i + 1);
                SwapCardsByPosition(i, j);
            }
        }

        public void SwapCardsByPosition(int position1, int position2)
        {
            var temp = _cards[position1];
            _cards[position1] = _cards[position2];
            _cards[position2] = temp;
        }

        public void MoveCardsToBottom(int count, int index = 0)
        {
            int j;

            for (var i = index; i < index + count; i++)
            {
                var topCard = _cards[i];

                for (j = i; j < _cards.Length - count; j += count)
                    _cards[j] = _cards[j + count];

                _cards[j] = topCard;
            }
        }


        private static int GetRandomInt(int upperBound)
        {
            int fullSets = 256 / upperBound;
            int max = fullSets * upperBound;

            do
            {
                _rng.GetBytes(_randByte);
            } while (_randByte[0] >= max);

            return _randByte[0] / fullSets;
        }
    }
}
