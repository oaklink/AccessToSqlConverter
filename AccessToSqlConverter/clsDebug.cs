using System.IO;

namespace AccessToSqlConverter
{
    class ClsDebug
    {

        /*
         * Add message to debug file
         * 
         * Msg Type
         * ! - Fault
         * > - Message
        */
        public void AddToDebug(string message)
        {
            using (StreamWriter w = File.AppendText(GlbData.GetDbugFileName))
            {
                w.WriteLine(message);
                w.WriteLine(" ");
                w.Close();
            }
        }
    }
}
