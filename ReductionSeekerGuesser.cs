using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public class ReductionSeekerGuesser : IGuesser
{
    public ReductionSeekerGuesser() {}

    public string GetGuess(Dictionary dictionary)
    {
        string bestEverWord = "roate";

        if (dictionary.LiveWords.Count == 1)
        {
            return dictionary.LiveWords[0];
        }
        else if(dictionary.LiveWords.Contains(bestEverWord))
        {
            return bestEverWord;
        }
        else
        {
            ImmutableArray<string> availableWords = dictionary.LiveWords.ToImmutableArray();
            return GuessWithBestGuessFinder(availableWords);
        }
    }

    private static int ReduceDictTest(ImmutableArray<string> availableWords, string actualWord, string guessWord)
    {
        Dictionary tempDictionary = new Dictionary(availableWords);
        string wordleResponse = WordleUtils.CalculateWordleResponse(actualWord, guessWord);
        int before = tempDictionary.LiveWords.Count;
        tempDictionary.RemoveWordsFromDictionary(guessWord, wordleResponse);

        int after = tempDictionary.LiveWords.Count;
        int reduction = before - after;
        // Console.WriteLine($"Actual {actualWord}, guess {guessWord}, reduction = {reduction}");

        return reduction;
    }


    private static string GuessWithBestGuessFinder(ImmutableArray<string> availableWords)
    {
        var guessWordReduction = new ConcurrentDictionary<string, int>();

        //var guessWords = new[] { "aback", "puppy", "zonal", "abase" };

        Parallel.ForEach(availableWords, guessWord =>
        {
            guessWordReduction[guessWord] = 0;

            Parallel.ForEach(availableWords, actualWord =>
            {
                if (actualWord != guessWord)
                {
                    int reduction = ReduceDictTest(availableWords, actualWord, guessWord);
                    guessWordReduction[guessWord] += reduction;
                }
            });

            //Console.WriteLine($"Completed {guessWordReduction.Count} reductions of of {availableWords.Length}");
        });

        var sortedGuessWordReduction = guessWordReduction.OrderByDescending(x => x.Value).ToList();
        string bestGuessWord = sortedGuessWordReduction[0].Key;
        return bestGuessWord;
    }
}
