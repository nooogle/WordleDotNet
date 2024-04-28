using System.Collections.Concurrent;
using System.Collections.Immutable;

public class GuessReductionSeeker
{
    private ImmutableArray<string> laWords;
    private ImmutableArray<string> taWords;

    public GuessReductionSeeker(ImmutableArray<string> laWords, ImmutableArray<string> taWords)
    {
        this.laWords = laWords;
        this.taWords = taWords;
    }

    public ImmutableDictionary<string, int> Run()
    {
        var guessWordReduction = new ConcurrentDictionary<string, int>();
        var combinedWords = laWords.AddRange(taWords);
        string bestGuess = "";
        object diagnosticsLock = new object();

        Parallel.ForEach(combinedWords, guessWord =>
        {
            int reduction = 0;

            Parallel.ForEach(laWords, actualWord =>
            {
                if (actualWord != guessWord)
                {
                    reduction += ReduceDictTest(laWords, actualWord, guessWord);
                }
            });

            lock(diagnosticsLock)
            {
                guessWordReduction[guessWord] = reduction;
                var sortedGuessWordReduction = guessWordReduction.OrderByDescending(x => x.Value).ToList();
                var newBestGuess = sortedGuessWordReduction[0].Key;
                if (newBestGuess != bestGuess)
                {
                    bestGuess = newBestGuess;

                    Console.WriteLine(
                        $"After {guessWordReduction.Count} reductions of {combinedWords.Length} the " + 
                        $"best word is {sortedGuessWordReduction[0].Key} with reduction {sortedGuessWordReduction[0].Value}");
                }
            }
        });

        var sortedGuessWordReduction = guessWordReduction.OrderByDescending(x => x.Value).ToList();
        return sortedGuessWordReduction.ToImmutableDictionary();
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
}
