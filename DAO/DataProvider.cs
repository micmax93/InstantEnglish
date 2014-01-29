using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using ElFartas.InstantEnglish.Interfaces;

namespace ElFartas.InstantEnglish.DAO
{
    static public class DataProvider
    {
        public static IDataAccess GetDataAccess()
        {
            var dataAccessName = ConfigurationManager.AppSettings["DataAccess"];
            var dllPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + '\\';

            Assembly assembly = Assembly.LoadFile(dllPath + dataAccessName + ".dll");
            Type da = assembly.GetType("ElFartas.InstantEnglish." + dataAccessName + ".DataAccess");
            return (IDataAccess) da.GetConstructor(new Type[] {}).Invoke(new object[] {});
        }
    }
}
