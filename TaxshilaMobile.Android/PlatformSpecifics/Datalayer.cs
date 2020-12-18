using TaxshilaMobile.Droid.PlatformSpecifics;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Datalayer))]
namespace TaxshilaMobile.Droid.PlatformSpecifics
{
    class Datalayer : IDatalayer
    {

        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public string GetLocalfolderpath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path);
        }
        //public SQLiteAsyncConnection GetConnection(string dbPath)
        //{
        //    return new SQLiteAsyncConnection(dbPath);
        //}
    }
}