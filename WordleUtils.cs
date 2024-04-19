using System;

public static class WordleUtils
{
    public const int LettersPerWord = 5;
    public const char GreenCode = 'G';
    public const char YellowCode = 'Y';
    public const char GreyCode = 'X';

    public static bool IsResponseValid(string response)
    {
        if (response.Length != LettersPerWord)
            return false;

        foreach (char letter in response)
        {
            if (letter != GreenCode && letter != YellowCode && letter != GreyCode)
                return false;
        }
        return true;
    }

    public static string CalculateWordleResponse(string actualWord, string guessWord)
    {
        if (actualWord.Length != LettersPerWord || guessWord.Length != LettersPerWord)
            throw new ArgumentException("Words must be of length " + LettersPerWord);

        string response = new string('?', LettersPerWord);
        char[] actualWordWithGreenRemoved = actualWord.ToCharArray();

        // First check for green letters
        for (int index = 0; index < LettersPerWord; index++)
        {
            if (guessWord[index] == actualWord[index])
            {
                response = response.Remove(index, 1).Insert(index, GreenCode.ToString());
                actualWordWithGreenRemoved[index] = ' ';  // Mark this character as used
            }
        }

        // Now check for yellow letters
        for (int index = 0; index < LettersPerWord; index++)
        {
            if (response[index] != GreenCode)
            {
                if (Array.IndexOf(actualWordWithGreenRemoved, guessWord[index]) >= 0)
                {
                    response = response.Remove(index, 1).Insert(index, YellowCode.ToString());
                    // Remove the first occurrence of the letter from the actual word
                    int firstIndex = Array.IndexOf(actualWordWithGreenRemoved, guessWord[index]);
                    actualWordWithGreenRemoved[firstIndex] = ' ';  // Mark this character as used
                }
            }
        }

        // Now check for grey letters
        for (int index = 0; index < LettersPerWord; index++)
        {
            if (response[index] == '?')
            {
                response = response.Remove(index, 1).Insert(index, GreyCode.ToString());
            }
        }

        return response;
    }
}
