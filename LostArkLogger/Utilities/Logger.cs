using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostArkLogger.Utilities
{
    public static class Logger
    {
        static string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static string logsPath = Path.Combine(documentsPath, "Lost Ark Logs");

        public static bool debugLog = false;
        static BinaryWriter logger;
        static FileStream logStream;

        private static readonly object LogFileLock = new object();
        private static readonly object DebugFileLock = new object();
        private static readonly object packetDumpLock = new object();
        public static string fileName = logsPath + "\\LostArk_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log";
        public static string fileNameLog = logsPath + "\\LALogs.log";

        static Logger()
        {
            if (!Directory.Exists(logsPath)) Directory.CreateDirectory(logsPath);
        }
        public static void StartNewLogFile()
        {
            fileName = logsPath + "\\LostArk_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log";
        }
        public static event Action<string> onLogAppend;
        //static bool InittedLog = false;
        public static void httpbridgeSender(int id, params string[] elements)
        {
            //write logs for loa-details | only works on console-mode
            var log = id + "|" + DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + "|" + String.Join("|", elements);
            var logHash = string.Concat(System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(log)).Select(x => x.ToString("x2")));

            Task.Run(() =>
            {
                onLogAppend?.Invoke(log + "\n");
                lock (LogFileLock)
                {
                    File.AppendAllText(fileName, log + "|" + logHash + "\n");
                }
            });
        }
        public static void packetDumper(int opcode, byte[] bytes)
        {
            Task.Run(() =>
            {
                string s = "";
                for (int i = 0; i < bytes.Length; i++) s += bytes[i].ToString("X2") +" ";
                var log = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + "\n" + s + "\n";
                lock (packetDumpLock)
                {
                    File.AppendAllText(logsPath + "\\"+opcode.ToString()+"_"+opcode.ToString("X")+".log", log);
                }
            });
        }
        public static void DoDebugLog(byte[] bytes)
        {
            if (debugLog)
            {
                Task.Run(() =>
                {
                    var log = BitConverter.GetBytes(DateTime.Now.ToBinary())
                        .Concat(BitConverter.GetBytes(bytes.Length)).Concat(bytes).ToArray();

                    lock (DebugFileLock)
                    {
                        if (logger == null)
                        {
                            logStream = new FileStream(fileName.Replace(".log", ".bin"), FileMode.Create);
                            logger = new BinaryWriter(logStream);
                        }

                        logger.Write(log);
                    }
                });
            }
        }
        public static void writeLogFile(int id, params string[] elements)
        {
            var log = id + "|" + DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'") + "|" + String.Join("|", elements);
            var logHash = string.Concat(System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(log)).Select(x => x.ToString("x2")));

            Task.Run(() =>
            {
                lock (LogFileLock)
                {
                    File.AppendAllText(fileNameLog, log + "|" + logHash + "\n");
                }
            });
        }
    }
}
