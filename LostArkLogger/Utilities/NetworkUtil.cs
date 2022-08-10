using System.Net.NetworkInformation;
using System.Windows.Forms;

// https://stackoverflow.com/questions/10789898/determine-which-network-adapter-a-process-is-using
namespace LostArkLogger.Utilities
{
    class NetworkUtil
    {
        public static NetworkInterface GetAdapterUsedByProcess(string pName)
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                //MessageBox.Show("P[ " + pName + " ] N[ " + nic.Name + " ]");
                if (nic.Name == pName)
                {
                    //MessageBox.Show(nic.Name);
                    return nic;
                }
            }
            return null;
        }
    }
}
