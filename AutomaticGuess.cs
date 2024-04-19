class AutomaticGuesser
{
    public static int RunAutomaticGuessLoop(
        IGuesser guesser,
        Dictionary dictionary,
        string actualWord,bool showDiagnostics)
    {
        int attempt = 1;
        bool foundCorrectWord = false;

        if (showDiagnostics)
        {
            Console.WriteLine($"Target word is {actualWord}");
        }

        while (!foundCorrectWord)
        {
            foundCorrectWord = RunOneAutomaticIteration(guesser, dictionary, attempt, actualWord, showDiagnostics);
            if (!foundCorrectWord)
            {
                attempt++;
            }
        }

        return attempt;
    }

    private static bool RunOneAutomaticIteration(
        IGuesser guesser,
        Dictionary dictionary,
        int attempt,
        string actualWord,
        bool showDiagnostics)
    {
        bool didGuessCorrectly = false;
        string guessWord = guesser.GetGuess(dictionary);

        string wordleResponse = WordleUtils.CalculateWordleResponse(actualWord, guessWord);

        if (showDiagnostics)
        {
            Console.WriteLine($"There are {dictionary.LiveWords.Count} words remaining, guess {attempt} is {guessWord}, response is {wordleResponse}");
        }

        dictionary.RemoveWordsFromDictionary(guessWord, wordleResponse);

        // Check if we have found the correct word
        if (dictionary.LiveWords.Count == 0)
        {
            throw new Exception("There are no words left in the list - something went wrong!");
        }
        else if (dictionary.LiveWords.Count == 1 && wordleResponse == "GGGGG")
        {
            didGuessCorrectly = true;
            if (showDiagnostics)
            {
                Console.WriteLine($"Hooray - we did it on attempt {attempt}! The word is {guessWord}");
            }
        }

        return didGuessCorrectly;
    }
}
