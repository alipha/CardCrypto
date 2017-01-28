
using System;
using System.Text;
namespace CardCrypto
{
    public class Chameleon : ICipher
    {
        //private bool notFirst;

        public Deck Key { get; set; }


        public Chameleon() 
        {
            Key = Deck.NewEmpty();

            var deck = Deck.NewShuffled();
            var blackIndex = -1;
            var redIndex = -1;

            for (var i = 0; i < Key.Cards.Length; i += 2)
            {
                while (deck.Cards[++blackIndex].IsRed()) ;
                while (deck.Cards[++redIndex].IsBlack()) ;

                Key.Cards[i] = deck.Cards[redIndex];
                Key.Cards[i + 1] = deck.Cards[blackIndex];
            }
        }

        public Chameleon(Deck key)
        {
            Key = key;
        }

        public Chameleon(string passphrase)
        {
            Key = Deck.NewOrdered();
            Transform(passphrase, EncryptLetter, isKeying:true);
        }

 
        public string Encrypt(string plaintext)
        {
            return Transform(plaintext, EncryptLetter);
        }

        public string Decrypt(string ciphertext)
        {
            return Transform(ciphertext, DecryptLetter);
        }


        private string Transform(string text, Func<char, bool, char> transform, bool isKeying = false)
        {
            var result = new StringBuilder();

            foreach (var letter in text)
            {
                var isLetter = (letter >= 'a' && letter <= 'z') || (letter >= 'A' && letter <= 'Z');
                result.Append(isLetter ? transform(letter, isKeying) : letter);
            }

            return result.ToString();
        }

        private char EncryptLetter(char letter, bool isKeying)
        {
            var plainBlackCardIndex = Array.IndexOf(Key.Cards, letter.ToBlackCard());
            var t = Key.Cards[plainBlackCardIndex - 1].ToLetter();

            var cipherBlackCardIndex = Array.IndexOf(Key.Cards, t.ToBlackCard());
            var cipherLetter = Key.Cards[cipherBlackCardIndex - 1].ToLetter();

            //if(letter == 'C' && notFirst)
            //    Console.WriteLine(Key);

            Key.SwapCardsByPosition(0, cipherBlackCardIndex - 1);

            //if (letter == 'C' && notFirst)
            //    Console.WriteLine(Key);

            if (isKeying)
                Key.MoveCardsToBottom(2, cipherBlackCardIndex - 1);

            //if (letter == 'C' && notFirst)
            //    Console.WriteLine(Key);

            Key.MoveCardsToBottom(2);

            //if (letter == 'C' && notFirst)
            //    Console.WriteLine(Key);

            //if (letter == 'C')
            //    notFirst = true;

            return cipherLetter;
        }

        private char DecryptLetter(char letter, bool isKeying)
        {
            var cipherRedCardIndex = Array.IndexOf(Key.Cards, letter.ToRedCard());
            var t = Key.Cards[cipherRedCardIndex + 1].ToLetter();

            var plainRedCardIndex = Array.IndexOf(Key.Cards, t.ToRedCard());
            var plainLetter = Key.Cards[plainRedCardIndex + 1].ToLetter();

            Key.SwapCardsByPosition(0, cipherRedCardIndex);
            Key.MoveCardsToBottom(2);

            return plainLetter;
        }
    }
}
