class Evaluater
{
    public static void Run(IGuesser guesser, Dictionary dictionary)
    {
        var guessCount = new List<int>();

        foreach(var targetWord in dictionary.OriginalWords)
        {
            dictionary.ResetWords();

            int numberOfGuesses = AutomaticGuesser.RunAutomaticGuessLoop(
                guesser: guesser,
                dictionary: dictionary,
                actualWord: targetWord,
                showDiagnostics: false);

            guessCount.Add(numberOfGuesses);

            Console.WriteLine($"{targetWord} took {numberOfGuesses} guesses.");
        }

        Console.WriteLine($"Average number of guesses: {guessCount.Average():0.00}");

        // save guess count to a CSV
        File.WriteAllLines("guesses.csv", guessCount.Select(x => x.ToString()));
    }
}
