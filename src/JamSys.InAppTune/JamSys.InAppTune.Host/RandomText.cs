namespace JamSys.InAppTune.Host
{
    public static class RandomText
    {
        public static string GenerateText(int minSize, int maxSize, bool allowWhiteSpace  = true)
        {
            string result = string.Empty;

            while(result.Length <= minSize)
            {
                result += LoremNET.Lorem.Words(1);
                if(allowWhiteSpace)
                    result += " ";
            }

            //one additional word to close the text line
            result += LoremNET.Lorem.Words(1);
            
            if(result.Length > maxSize)
                result = result.Substring(0, maxSize);

            return result;
        }

        public static string InsertRandom(string input, string text)
        {
            string result = input;
            Random rand = new Random();
            int position = rand.Next(0, result.Length - text.Length);
            result = result.Remove(position, text.Length);
            result = result.Insert(position, text);
            return result;
        }
    }
}