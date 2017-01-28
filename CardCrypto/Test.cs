
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardCrypto
{
    public class Test
    {
        public static void Main(string[] args)
        {
            for(var i = 0; i < 24; i++)
                SimpleTest(i % 8);
            Console.ReadLine();
        }


        private static void SimpleTest(int phraseIndex)
        {
            if(phraseIndex == 0)
                Console.WriteLine();

            /*
            var phrases = new[]
            {
                "AAAAAAAAAAAAAAAAAAAAAAAAAA",
                "ThisissometestIdonotknowit",
                "Applesbananascarrotsdonuts",
                "AnotherphraseIhavenoideahm",
                "abcdefghijklmnopqrstuvwxyz",
                "thequickbrownfoxjumpsoverz",
                "zyxwvutsrqponmlkjihgfedcba",
                "attackatdawnattackatdawnaa"
            };
            */
            var phrases = new[]
            {
                //"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", //AAAAAAAAAAAAAAAAAA",
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", //AAAAAAAAAAAAAAAAAAAA",
                "AAAAAAAAAAAAAAAAAAAAAAAAAAThisissometestIdonotknowit",
                "ApplesbananascarrotsdonutsThisissometestIdonotknowit",
                "AnotherphraseIhavenoideahmApplesbananascarrotsdonuts",
                "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
                "thequickbrownfoxjumpsoverzthequickbrownfoxjumpsoverz",
                "zyxwvutsrqponmlkjihgfedcbaabcdefghijklmnopqrstuvwxyz",
                "attackatdawnattackatdawnaaApplesbananascarrotsdonuts"
            };

            var cipher = new Chameleon();//"okaycanweputthespadeathebottom"); //"somethingcompletelydifferent");//"letsdoadifferentkey");//"cardciphercardcipher");
            var encrypted = cipher.Encrypt(phrases[phraseIndex]);

            var correctRedCount = 0;
            var correctBlackCount = 0;
            var deckGuess = Deck.NewEmpty();


            //Console.Write(phrases[phraseIndex] + " ");

            var previousRedIndex = -1;

            for (var i = 0; i < encrypted.Length; i++)
            {
                //deckGuess.Cards[i*2 % 52] = encrypted[i].ToRedCard();

                var redCard = encrypted[i].ToRedCard();
                var redCardIndex = Array.IndexOf(deckGuess.Cards, redCard);

                if (redCardIndex > 0)
                {
                    deckGuess.SwapCardsByPosition(0, redCardIndex);

                    /*
                    if (deckGuess.Cards[redCardIndex] != 0 && previousRedIndex > 0)// && i >= 52)
                    {
                        deckGuess.Cards[51] = encrypted[i - 1].ToBlackCard();
                        deckGuess.Cards[previousRedIndex - 1] = deckGuess.Cards[50].ToLetter().ToBlackCard();
                        redCardIndex = -1;
                    }
                     * */
                }
                else
                {
                    deckGuess.Cards[0] = redCard;
                }

                deckGuess.MoveCardsToBottom(2);
                previousRedIndex = redCardIndex;
            }


            for (var i = 0; i < deckGuess.Cards.Length; i += 2) //i++)
            {
                var correct = cipher.Key.Cards[i] == deckGuess.Cards[i];

                if (i % 2 == 0)
                {
                    Console.Write(correct ? '.' : 'X');

                    if (correct)
                        correctRedCount++;
                }
                else
                {
                    Console.Write(deckGuess.Cards[i] == 0 ? '_' : correct ? ',' : 'x');

                    if (correct)
                        correctBlackCount++;
                }
            }

            Console.WriteLine(" " + correctRedCount + "/26 red " + correctBlackCount + "/26 black");
        }


        private static string DeckToString(IEnumerable<int> deck)
        {
            return string.Join(" ", deck.Select(x => x.CardName()));
        }
    }
}
