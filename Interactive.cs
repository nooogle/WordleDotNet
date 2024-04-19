
class Interactive
{
    static string GetWordleResponse()
    {
        bool responseOk = false;
        string wordleResponse = string.Empty;

        while (!responseOk)
        {
            Console.Write("Enter WORDLE response > ");
            wordleResponse = Console.ReadLine();

            if (!WordleUtils.IsResponseValid(wordleResponse))
            {
                Console.WriteLine("Invalid response. Please try again.");
            }
            else
            {
                responseOk = true;
            }
        }

        return wordleResponse;
    }


    public static void RunInteractiveGuessLoop(IGuesser guesser, Dictionary dictionary)
    {
        int attempt = 1;
        bool foundCorrectWord = false;

        while (!foundCorrectWord)
        {
            foundCorrectWord = RunOneInteractiveIteration(guesser, dictionary, attempt);
            if (!foundCorrectWord)
            {
                attempt++;
            }
        }

        if (!foundCorrectWord)
        {
            Console.WriteLine($"Sorry, after guess {attempt} we did not find the word.");
        }
    }

    private static bool RunOneInteractiveIteration(IGuesser guesser, Dictionary dictionary, int attempt)
    {
        bool didGuessCorrectly = false;
        string guessWord = guesser.GetGuess(dictionary);

        Console.WriteLine($"There are {dictionary.LiveWords.Count} words remaining in the list. Guess {attempt} is {guessWord}");
        string wordleResponse = GetWordleResponse();

        dictionary.RemoveWordsFromDictionary(guessWord, wordleResponse);

        // Check if we have found the correct word
        if (dictionary.LiveWords.Count == 1)
        {
            if (wordleResponse != "GGGGG")
            {
                Console.WriteLine("There is only one word left in the list. The response must be GGGGG - something went wrong!");
            }
            else
            {
                didGuessCorrectly = true;
                Console.WriteLine($"Hooray - we did it on attempt {attempt}! The word is {guessWord}");
            }
        }

        return didGuessCorrectly;
    }
}