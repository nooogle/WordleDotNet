using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;


public class Dictionary 
{
    private ImmutableArray<string> originalWords;
    private List<string> liveWords;

    public Dictionary(ImmutableArray<string> words) {
        originalWords = words;
        liveWords = new List<string>(words);
    }

    public List<string> LiveWords {
        get { return liveWords; }
    }

    public ImmutableArray<string> OriginalWords => originalWords;
    

    public void ResetWords() {
        liveWords = new List<string>(originalWords);
    }

    public void RemoveWordsFromDictionary(string guessWord, string wordleResponse) 
    {
        FilterDictionaryOnGreenResponses(guessWord, wordleResponse);
        FilterDictionaryOnYellowResponses(guessWord, wordleResponse);

        var mustHaveDict = GenerateMustHaveDict(guessWord, wordleResponse);
        var mustNotHaveDict = GenerateMustNotHaveDict(guessWord, wordleResponse);

        CleanMustNotHaveDict(mustHaveDict, mustNotHaveDict);

        FilterFromLetterCountDicts(wordleResponse, mustHaveDict, mustNotHaveDict);
    }

    private void FilterFromLetterCountDicts(
        string wordleResponse, 
        Dictionary<char, int> mustHaveDict, 
        Dictionary<char, int> mustNotHaveDict) 
    {
        List<string> wordsToKeep = new List<string>();
        
        foreach (var dictWord in liveWords) 
        {
            var letterCountsDict = CreateNonGreenLetterCountsDict(wordleResponse, dictWord);
            bool keep = ShouldKeepWordBasedOnMustHaveLetters(mustHaveDict, letterCountsDict);
            keep = keep && ShouldKeepWordBasedOnMustNotHaveLetters(mustNotHaveDict, letterCountsDict);

            if (keep) 
            {
                wordsToKeep.Add(dictWord);
            }
        }

        liveWords = wordsToKeep;
    }

    private bool ShouldKeepWordBasedOnMustNotHaveLetters(
        Dictionary<char, int> mustNotHaveDict, 
        Dictionary<char, int> letterCountsDict) 
    {
        foreach (var letter in mustNotHaveDict.Keys) 
        {
            if (letterCountsDict.TryGetValue(letter, out int actualLetterCount) && actualLetterCount > 0) 
            {
                return false;
            }
        }

        return true;
    }

    private bool ShouldKeepWordBasedOnMustHaveLetters(
        Dictionary<char, int> mustHaveDict,
        Dictionary<char, int> letterCountsDict) 
    {
        foreach (var pair in mustHaveDict) 
        {
            if (!letterCountsDict.TryGetValue(pair.Key, out int actualLetterCount) || actualLetterCount < pair.Value) 
            {
                return false;
            }
        }

        return true;
    }

    private Dictionary<char, int> CreateNonGreenLetterCountsDict(string wordleResponse, string dictWord) {
        Dictionary<char, int> letterCountsDict = new Dictionary<char, int>();
        for (int index = 0; index < dictWord.Length; index++) 
        {
            if (wordleResponse[index] != WordleUtils.GreenCode) 
            {
                char letter = dictWord[index];
                if (letterCountsDict.ContainsKey(letter))
                {
                    letterCountsDict[letter]++;
                }
                else
                {
                    letterCountsDict.Add(letter, 1);
                }
            }
        }

        return letterCountsDict;
    }

    private void CleanMustNotHaveDict(Dictionary<char, int> mustHaveDict, Dictionary<char, int> mustNotHaveDict) {
        foreach (var letter in mustHaveDict.Keys.ToList()) 
        {
            if (mustNotHaveDict.ContainsKey(letter)) 
            {
                mustNotHaveDict.Remove(letter);
            }
        }
    }


    private Dictionary<char, int> GenerateMustNotHaveDict(string guessWord, string wordleResponse) {
        Dictionary<char, int> mustNotHaveDict = new Dictionary<char, int>();
        for (int index = 0; index < wordleResponse.Length; index++) 
        {
            if (wordleResponse[index] == WordleUtils.GreyCode) 
            {
                char letter = guessWord[index];
                if (mustNotHaveDict.ContainsKey(letter))
                {
                    mustNotHaveDict[letter]++;
                }
                else
                {
                    mustNotHaveDict.Add(letter, 1);
                }
            }
        }

        return mustNotHaveDict;
    }

    private Dictionary<char, int> GenerateMustHaveDict(string guessWord, string wordleResponse) {
        Dictionary<char, int> mustHaveDict = new Dictionary<char, int>();
        for (int index = 0; index < wordleResponse.Length; index++) 
        {
            if (wordleResponse[index] == WordleUtils.YellowCode) 
            {
                char letter = guessWord[index];
                if (mustHaveDict.ContainsKey(letter))
                {
                    mustHaveDict[letter]++;
                }
                else
                {
                    mustHaveDict.Add(letter, 1);
                }
            }
        }
        return mustHaveDict;
    }

    private void FilterDictionaryOnYellowResponses(string guessWord, string wordleResponse) 
    {
        List<string> wordsToKeep = new List<string>();

        for(int liveWordIndex = 0; liveWordIndex < liveWords.Count; liveWordIndex++)
        {
            string liveWord = liveWords[liveWordIndex];
            var keep = true;

            for (int index = 0; keep && (index < wordleResponse.Length); index++) 
            {
                char wordleResponseLetter = wordleResponse[index];
                if (wordleResponseLetter == WordleUtils.YellowCode) 
                {
                    keep = liveWord[index] != guessWord[index];
                }
            }
            
            if (keep)
            {
                wordsToKeep.Add(liveWord);
            }
        }

        liveWords = wordsToKeep;
    }


    private void FilterDictionaryOnGreenResponses(string guessWord, string wordleResponse) 
    {
        List<string> wordsToKeep = new List<string>();

        for(int liveWordIndex = 0; liveWordIndex < liveWords.Count; liveWordIndex++)
        {
            string liveWord = liveWords[liveWordIndex];
            var keep = true;

            for (int index = 0; keep && (index < wordleResponse.Length); index++) 
            {
                char wordleResponseLetter = wordleResponse[index];
                if (wordleResponseLetter == WordleUtils.GreenCode) 
                {
                    keep = liveWord[index] == guessWord[index];
                }
            }
            
            if(keep)
            {
                wordsToKeep.Add(liveWord);
            }
        }

        liveWords = wordsToKeep;
    }
}
