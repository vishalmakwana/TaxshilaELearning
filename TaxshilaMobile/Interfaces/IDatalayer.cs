using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile
{
    public interface IDatalayer
    {
        string GetLocalFilePath(string filename);

        string GetLocalfolderpath();
        //SQLite.SQLiteAsyncConnection GetConnection(string dbPath);
    }
}
