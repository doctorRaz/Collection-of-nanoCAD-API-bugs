using System;

namespace ConsoleApp
{
    internal class Services
    {

        internal static string CallerName(int CountFiles/*, [CallerMemberName] string caller = null*/)
        {
            Version version = new Version(1, 1, 1);

            string appProductName = "app";// System.Windows.Forms.Application.ProductName;

            return $"{appProductName}_{version.Major.ToString()}.{version.Minor.ToString()}_{CountFiles.ToString()}";

        }

    }
}
