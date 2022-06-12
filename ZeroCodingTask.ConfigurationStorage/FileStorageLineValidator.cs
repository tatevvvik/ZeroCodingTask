namespace ZeroCodingTask.ConfigurationStorage
{
    public static class FileStorageLineValidator
    {
        public static bool Validate(string line)
        {
            var splitedValues = line.Trim().Split(':', 2);
            var firstSplitedValue = splitedValues[0];

            if (firstSplitedValue == "FILE") // file : node : path format
            {
                var reSplit = splitedValues[1].Split(':', 2);

                if (reSplit.Length != 2)
                {
                    return false;
                }
            }
            else if (splitedValues.Length != 2) // key : value format
            {
                return false;
            }

            return true;
        }
    }
}
