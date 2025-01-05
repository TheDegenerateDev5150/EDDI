using System.Data.SQLite;
using System.IO;
using Utilities;

namespace EddiDataProviderService
{
    public class SqLiteBaseRepository
    {
        protected static bool unitTesting
        {
            get => _unitTesting;
            set
            {
                if ( _unitTesting != value )
                {
                    ResetTestDatabase();
                    _unitTesting = value;
                }
            }
        }
        private static bool _unitTesting;

        private static void ResetTestDatabase ()
        {
            var testDatabase = new FileInfo( Constants.DATA_DIR + @"\EDDI_TEST.sqlite" );
            if ( testDatabase.Exists ) { testDatabase.Delete(); }
        }

        protected static string DbFile => unitTesting 
            ? Constants.DATA_DIR + @"\EDDI_TEST.sqlite"
            : Constants.DATA_DIR + @"\EDDI.sqlite";

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }
}
