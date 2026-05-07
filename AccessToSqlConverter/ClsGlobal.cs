namespace AccessToSqlConverter
{
    public static class GlbData
    {
        public static int svnOffset;

        static public string[] dateTimeStrings =
            {
            "dd-MMM-yy HH:mm:ss",
            "dd-MMM-yy HH:mm",
            "dd-MMM-yy h:mm:ss",
            "dd-MMM-yy h:mm",
            "dd-MMM-yy"
        };
        static public string[] timeStrings = { "HH:mm:ss", "HH:mm" };
        static public string svnVersion;

        public static string _dbugPath;
        public static string SetDbugFileName
        {
            set { _dbugPath = value; }
        }
        public static string GetDbugFileName
        {
            get { return _dbugPath; }
        }
        private static double _commentRungLimit;
        public static double CommentRungLimit
        {
            get { return _commentRungLimit; }
            set { _commentRungLimit = value; }
        }
    }
}
