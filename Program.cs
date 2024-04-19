using System.Collections.Immutable;

namespace DotNetTests;

class Program
{
    static void Main(string[] args)
    {
        ImmutableArray<string> laWords = File.ReadAllLines(@".\Words\la.txt").ToImmutableArray();
        Console.WriteLine($"Read {laWords.Length} words from la.txt");

        Dictionary dictionary = new Dictionary(laWords);

        IGuesser guesser = new GuessReductionSeeker();
        //guesser.GetGuess(dictionary);

        Interactive.RunInteractiveGuessLoop(guesser, dictionary);
        //AutomaticGuesser.RunAutomaticGuessLoop(guesser, dictionary, "shank", true);
    }
}
