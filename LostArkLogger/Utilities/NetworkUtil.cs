using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

// https://stackoverflow.com/questions/10789898/determine-which-network-adapter-a-process-is-using
namespace LostArkLogger.Utilities
{
    class NetworkUtil
    {
        public enum ReqType
        {
            NICName,
            ProcessName
        }
        public static NetworkInterface GetAdapter(string name, ReqType type)
        {
            switch (type)
            {
                case ReqType.ProcessName:
                    Process[] candidates = Process.GetProcessesByName(name);
                    if (candidates.Length != 0)
                    {
                        IPAddress localAddr = null;
                        using (Process p = candidates[0])
                        {
                            TcpTable table = ManagedIpHelper.GetExtendedTcpTable(true);
                            foreach (TcpRow r in table)
                                if (r.ProcessId == p.Id)
                                {
                                    localAddr = r.LocalEndPoint.Address;
                                    break;
                                }
                        }

                        if (localAddr != null)
                        {
                            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                            {
                                IPInterfaceProperties ipProps = nic.GetIPProperties();
                                if (ipProps.UnicastAddresses.Any(new Func<UnicastIPAddressInformation, bool>((u) => { return u.Address.ToString() == localAddr.ToString(); })))
                                    return nic;
                            }
                        }
                    }
                    break;
                case ReqType.NICName:
                    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (nic.Name == name)
                        {
                            return nic;
                        }
                    }
                    break;
            }

            return null;
        }
    }
}
