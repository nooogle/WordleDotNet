using System.Collections.Immutable;

namespace DotNetTests;

class Program
{
    static void Main(string[] args)
    {
        ImmutableArray<string> laWords = File.ReadAllLines(@"./Words/la.txt").ToImmutableArray();
        ImmutableArray<string> taWords = File.ReadAllLines(@"./Words/ta.txt").ToImmutableArray();
        Console.WriteLine($"Read {laWords.Length} words from la.txt");
        Console.WriteLine($"Read {taWords.Length} words from ta.txt");

        // var guessReductionSeeker = new GuessReductionSeeker(laWords, taWords);
        // var guessWordReduction = guessReductionSeeker.Run();

        // // save guess word reduction to file
        // using (StreamWriter file = new StreamWriter(@"./Words/guessWordReduction.csv"))
        // {
        //     foreach (var item in guessWordReduction)
        //     {
        //         file.WriteLine($"{item.Key},{item.Value}");
        //     }
        // }


        Dictionary dictionary = new Dictionary(laWords.Add("roate"));
        IGuesser guesser = new ReductionSeekerGuesser();
        //Evaluater.Run(guesser, dictionary);
        Interactive.RunInteractiveGuessLoop(guesser, dictionary);
        //AutomaticGuesser.RunAutomaticGuessLoop(guesser, dictionary, "shank", true);
    }
}
