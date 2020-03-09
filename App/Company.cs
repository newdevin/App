namespace App
{
    public class Company
    {
        // TODO: remove hard coded magic strings
        public int Id { get; set; }

        public string Name { get; set; }

        public Classification Classification { get; set; }

        public bool IsVeryImportantClient()
        {
            if (Name == "VeryImportantClient")
                return true;
            else
                return false;
        }

        public bool IsImportantClient()
        {
            if (Name == "ImportantClient")
                return true;
            else
                return false;
        }

    }
}