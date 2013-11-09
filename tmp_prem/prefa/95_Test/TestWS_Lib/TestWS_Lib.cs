using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml;

namespace TestWS_Lib {
    public class TestWS_Lib {

        private List<Thread> threadList;
        private EFA_WS.Service service;
        private String result;
        private int progressValue;
        private XmlDocument resultDocument;

        public TestWS_Lib() {
            service = new EFA_WS.Service();
        }

        /// <summary>
        /// A string containing results from the WS.
        /// </summary>
        public String Result {
            get { return result; }
        }

        /// <summary>
        /// 1/10 of currently done percentage.
        /// </summary>
        public int ProgressValue {
            get { return progressValue; }
        }

        /// <summary>
        /// XmlDocument, containing results from the WS.
        /// </summary>
        public XmlDocument ResultDocument {
            get { return resultDocument; }
        }

        public void startTesting(List<ThreadData> listThreadData) {

            result = "";
            progressValue = 0;

            //create the xml document for storing results
            resultDocument = new XmlDocument();
            XmlNode node = resultDocument.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            resultDocument.AppendChild(node);
            XmlNode report = resultDocument.CreateElement("Report");
            resultDocument.AppendChild(report);

            int numThreads = listThreadData.Count;

            //start all threads
            threadList = new List<Thread>();
            for (int i=0; i<numThreads; i++) {
                ThreadData td = listThreadData[i];
                int j = i;
                ThreadStart starter = delegate { threadWork(td.Packages, td.NumSendPackages, td.TimeLag, numThreads, j); };
                Thread t = new Thread(starter);
                threadList.Add(t);
                t.Start();
            }
        }

        private void threadWork(List<Package> selectedPackages, int numSendPackages, int timeLag, int numThreads, int internalThreadNum) {

            String output = "";
            String report = "";
            int kolicnik = numSendPackages * numThreads * selectedPackages.Count;
            Stopwatch sw = new Stopwatch();

            //create InternalThread element
            XmlElement xmlThread = resultDocument.CreateElement("InternalThread");
            xmlThread.SetAttribute("id",internalThreadNum.ToString());
            resultDocument.ChildNodes[1].AppendChild(xmlThread);

            //sends each package 'numSendPackages' times, with a 'timeLag' between each send
            for (int i = 0; i < numSendPackages; i++) {
                foreach (Package sp in selectedPackages) {
                    sw.Reset();
                    String paketStatus = "";

                    //create package element
                    XmlElement xmlPackage = resultDocument.CreateElement("Package");
                    xmlThread.AppendChild(xmlPackage);
                    XmlText xmlText;

                    try {
                        sw.Start();
                        paketStatus = service.PosljiPaket(sp.PackageContent);
                        sw.Stop();
                        output = paketStatus.Substring(paketStatus.IndexOf("<PaketStatusOpis>") + 17);
                        output = output.Remove(output.IndexOf("</PaketStatusOpis>"));

                        //create status element
                        XmlElement xmlPackageStatus = resultDocument.CreateElement("Status");
                        xmlText = resultDocument.CreateTextNode(output);
                        xmlPackageStatus.AppendChild(xmlText);
                        xmlPackage.AppendChild(xmlPackageStatus);

                        int indZ;
                        while ((indZ = paketStatus.IndexOf("<SeznamNapak>")) != -1) {
                            int indK = paketStatus.IndexOf("</SeznamNapak>") + "</SeznamNapak>".Length;
                            string napaka = paketStatus.Substring(indZ + "<SeznamNapak>".Length, (indK - "</SeznamNapak>".Length) - (indZ + "<SeznamNapak>".Length));

                            //create ListOfErrors element
                            XmlElement xmlPackageErrorList = resultDocument.CreateElement("ErrorList");
                            napaka = napaka.Replace("'", "");
                            xmlText = resultDocument.CreateTextNode(napaka);
                            xmlPackageErrorList.AppendChild(xmlText);
                            xmlPackage.AppendChild(xmlPackageErrorList);

                            paketStatus = paketStatus.Remove(indZ, indK - indZ);
                        }

                    } catch (Exception e) {
                        output = "\r\nPrišlo je do napake, mogoče je paket prevelik. Za več informacij" +
                            " preberi še sledeče sporočilo.\r\n\r\n" + e.ToString() + "\r\n" +  paketStatus;

                        //create exception element
                        XmlElement xmlPackageException = resultDocument.CreateElement("Exception");
                        xmlText = resultDocument.CreateTextNode(e.ToString());
                        xmlPackageException.AppendChild(xmlText);
                        xmlPackage.AppendChild(xmlPackageException);
                    }
                    report = "Thread id: " + Thread.CurrentThread.ManagedThreadId +
                            "\t elapsed time:" + (int)sw.Elapsed.TotalSeconds + "s " +
                            sw.Elapsed.Milliseconds + "ms"+
                            "\t package name: " + sp.Name +
                            "\t package status: " + output + "\r\n";

                    //create thread id element
                    XmlElement xmlPackageThreadId = resultDocument.CreateElement("ThreadId");
                    xmlText = resultDocument.CreateTextNode(Thread.CurrentThread.ManagedThreadId.ToString());
                    xmlPackageThreadId.AppendChild(xmlText);
                    xmlPackage.AppendChild(xmlPackageThreadId);

                    //create elapsed time element
                    XmlElement xmlPackageElapsedTime = resultDocument.CreateElement("ElapsedTimeMilliseconds");
                    xmlText = resultDocument.CreateTextNode(sw.Elapsed.TotalMilliseconds.ToString());
                    xmlPackageElapsedTime.AppendChild(xmlText);
                    xmlPackage.AppendChild(xmlPackageElapsedTime);

                    //create name element
                    XmlElement xmlPackageName = resultDocument.CreateElement("Name");
                    xmlText = resultDocument.CreateTextNode(sp.Name);
                    xmlPackageName.AppendChild(xmlText);
                    xmlPackage.AppendChild(xmlPackageName);

                    this.SetText(report);

                    Thread.Sleep(timeLag);
                    this.SetProgress((int)1000 / (kolicnik));
                }
                //if there was an error when reading xml, update progress anyway
                if (selectedPackages.Count == 0)
                    this.SetProgress((int)1000 / (kolicnik));
            }
        }

        /*
         * for synchonized access to the result string
         */
        private void SetText(String text) {
            lock(result) {
                result += text;
            }
        }

        /*
         * for synchonized access to the progress bar
         */
        private void SetProgress(int value) {
            lock ((Object)progressValue) {
                progressValue += value;
            }
        }

        public void cancel() {
            foreach (Thread item in threadList) {
                item.Abort();
            }
        }

        public bool threadsRunning() {
            foreach (Thread item in threadList) {
                if (item.IsAlive)
                    return true;
            }
            return false;
        }

    }

    /// <summary>
    /// A class that contains all data for each package.
    /// </summary>
    public class Package {

        private String name;
        private String path;
        private String packageContent;

        public Package() { }

        public Package(String name, String package, String path) {
            this.name = name;
            this.packageContent = package;
            this.path = path;
        }

        public Package(String name, String package) {
            this.name = name;
            this.packageContent = package;
            this.path = "";
        }

        /// <summary>
        /// Name of the package.
        /// </summary>
        public String Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The actual .xml content.
        /// </summary>
        public String PackageContent {
            get { return this.packageContent; }
            set { this.packageContent = value; }
        }

        /// <summary>
        /// Path to the package.
        /// </summary>
        public String Path {
            get { return path; }
            set { path = value; }
        }

        public override String ToString() {
            return this.name;
        }
    }

    /// <summary>
    /// A class that contains all the data for each thread, that sends packages to the WS.
    /// </summary>
    public class ThreadData {

        private List<Package> packages;
        private int numSendPackages;
        private int timeLag;

        /// <summary>
        /// List, containing all the packages in this thread.
        /// </summary>
        public List<Package> Packages {
            get { return packages; }
            set { packages = value; }
        }

        /// <summary>
        /// How many times the list is sent.
        /// </summary>
        public int NumSendPackages {
            get { return numSendPackages; }
            set { numSendPackages = value; }
        }

        /// <summary>
        /// Time lag between each package, that we send.
        /// </summary>
        public int TimeLag {
            get { return timeLag; }
            set { timeLag = value; }
        }

        public ThreadData(List<Package> packages, int numSendPackages, int timeLag) {
            this.packages = packages;
            this.numSendPackages = numSendPackages;
            this.timeLag = timeLag;
        }
    
    }
}
