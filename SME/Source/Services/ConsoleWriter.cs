using System;
using System.Text;

namespace SME.Services
{
    public static class ConsoleWriter
    {
        public static void ConsoleAnException(Exception e){
            var sb = new StringBuilder();
            sb.Append("\n------------------------------------------------------------\n");
            sb.Append(e.Message);
            sb.Append("\n------------------------------------------------------------\n");
            sb.Append(e.StackTrace);
            sb.Append("\n------------------------------------------------------------\n");
            sb.Append(e.InnerException);
            sb.Append("\n------------------------------------------------------------\n");
            Console.WriteLine(sb.ToString());
        }

    }
}