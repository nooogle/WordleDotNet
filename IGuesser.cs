interface IGuesser
{
     /// <summary>
     /// Get the next guess from the dictionary
     /// </summary>
     /// <param name="dictionary">Dictionary of words</param>
     /// <returns>The best guess!</returns>
     string GetGuess(Dictionary dictionary);
}
