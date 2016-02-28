using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace OpenRCT2_Autostarter
{
    class ExecuteConsole
    {
        Boolean isRunning = true;
        String installPath,savesPath;
        Process x;

        public event ServerStateChanges OnServerStateChanges;

        public void setInstallPath(String installPath)
        {
            if((installPath[installPath.Length-1] != '\\') && (installPath[installPath.Length - 1] != '/'))
            {
                installPath += @"\";

            }
            this.installPath = installPath;
        }

        public void setSavesPath(String savesPath)
        {
            if ((savesPath[savesPath.Length - 1] != '\\') && (savesPath[savesPath.Length - 1] != '/'))
            {
                savesPath += @"\";

            }
            this.savesPath = savesPath;
        }
        public void startServer()
        {
            Thread mainThread = new Thread(new ThreadStart(main));
            mainThread.Start();
        }

        private void main()
        {
            try
            {
                while (isRunning)
                {

                    
                    String saveFile = getNewestFile();
                    System.Threading.Thread.Sleep(1000);
                    //Process x = Process.Start()
                    x = Process.Start(installPath + "openrct2", "host \"" + saveFile + "\" --port 11753");

                    ServerRunningEventArgs e1 = new ServerRunningEventArgs();
                    e1.running = 1;
                    OnServerStateChanges(this, e1);

                    System.Threading.Thread.Sleep(1000);
                    while (x.Responding)
                    {
                        ;
                    }
                    x.Kill();

                    ServerRunningEventArgs e2 = new ServerRunningEventArgs();
                    e2.running = 0;
                    OnServerStateChanges(this, e2);

                    System.Threading.Thread.Sleep(1000);

                }
            }
            catch(Exception ex)
            {
                ServerRunningEventArgs e2 = new ServerRunningEventArgs();
                e2.running = 2;
                OnServerStateChanges(this, e2);
            }
        }
        
        public string getNewestFile()
        {
            String folder = savesPath;
            String[] files = System.IO.Directory.GetFiles(folder);
            String newestFile = "";
            
            foreach (String file in files)
            {

                if (newestFile != "")
                {

                    if ((new FileInfo(file).CreationTime > new FileInfo(newestFile).CreationTime))
                    {
                        newestFile = file;
                    }
                }
                else
                {
                    newestFile = file;
                }
            }

            Console.WriteLine(newestFile);

            return newestFile;
        }

        public void killProcesses()
        {
            isRunning = false;
            x.Kill();
        }
        public delegate void ServerStateChanges(object sender, ServerRunningEventArgs e);
    }



    public class ServerRunningEventArgs : EventArgs
    {
        public int running;
    }
}
