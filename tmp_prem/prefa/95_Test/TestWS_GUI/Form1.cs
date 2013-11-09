using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using TestWS_Lib;

namespace TestWS_GUI {
    public partial class Form1 : Form {

        private String dirPath;
        private String generatePath;
        private String sendPath;
        private XmlWriter xw;
        private StreamReader sr;
        private int st;
        private Random random;
        delegate void SetTextCallback(string text);
        delegate void SetProgressCallback(int value);
        private int threadCount;
        private TestWS_Lib.TestWS_Lib lib;

        public Form1() {
            InitializeComponent();
            dirPath = System.IO.Directory.GetCurrentDirectory();
            dirPath = dirPath.Remove(dirPath.IndexOf("bin\\Debug"));
            dirPath += "TestniPaketi";
            generatePath = dirPath;
            sendPath = generatePath + "\\generated_packages";
            random = new Random();
            threadCount = 2;
            lib = new TestWS_Lib.TestWS_Lib();
        }

        private void Form1_Load(object sender, EventArgs e) {
            fillListBoxPackages();
            fillListBoxSendPackages();
            progressBar1.Value = 0;
            st = 0;
        }
        
        #region //Create packages

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Orodje za testiranje EFA WebService.", "O programu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonFolder1_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                generatePath = folderBrowserDialog1.SelectedPath;
                sendPath = generatePath + "\\generated_packages";
            }
            fillListBoxPackages();
        }

        private void tabControl1_Click(object sender, EventArgs e) {
            fillListBoxPackages();
            fillListBoxSendPackages();
        }

        /*
         * fills the list box with the .xmls from the specified folder
         */
        private void fillListBoxPackages() {
            listBoxPackages.Items.Clear();
            String s = "";
            foreach (Object item in Directory.GetFiles(generatePath)) {
                s = item.ToString();
                if(s.EndsWith(".xml")) {                
                    s = s.Substring(s.LastIndexOf("\\")+1);
                    listBoxPackages.Items.Add(s);
                }
            }
            listBoxPackages.SelectedIndex = 0;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            String link = "file:///" + generatePath.Replace("\\", "/") + "/generated_packages";
            System.Diagnostics.Process.Start(link);
        }

        private void buttonGenerate_Click(object sender, EventArgs e) {
            //read selected xml into actual XmlDocument
            String packagePath = generatePath + "\\" + listBoxPackages.SelectedItem.ToString();
            String packageString = null;

            sr = new StreamReader(packagePath);
            packageString = sr.ReadToEnd();

            XmlDocument packageXml = new XmlDocument();
            try {
                packageXml.LoadXml(packageString);
            } catch {
                MessageBox.Show("XML napaka.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                sr.Close();
            }

            //read other data
            int numPack = 1;
            int size = 1;
            try {
                numPack = Int32.Parse(numPackages.Text);
                size = Int32.Parse(textBoxSize.Text);
            } catch (Exception) {}

            //call generate function
            this.Cursor = Cursors.WaitCursor;
            generateXmlPackets(packageXml, numPack, size);
            this.Cursor = Cursors.Arrow;

        }

        private void generateXmlPackets(XmlDocument packageXml, int numPack, int size) {
            progressBar1.Value = 0;
            //select all EfaGlava nodes
            XmlNodeList list = packageXml.SelectNodes("//EfaGlava");

            String savePath = generatePath + "\\generated_packages\\package_" + size;

            //if directory doesn't alredy exist,  create it
            if (!Directory.Exists(generatePath + "\\generated_packages\\"))
                Directory.CreateDirectory(generatePath + "\\generated_packages\\");

            int sizeMin = 120 * size;
            int sizeMax = 130 * size;
            for (int i=0; i<numPack; i++) {
                int max = random.Next(sizeMin, sizeMax);

                //clone the base xml
                XmlDocument newPackage = new XmlDocument();
                newPackage = (XmlDocument)packageXml.Clone();

                //select the EfaPaket node
                XmlNode node = newPackage.SelectSingleNode("//EfaPaket");
                for (int j=0; j<max; j++) {
                    //fill the package with a random node from all EfaGlava nodes until it is large enough
                    XmlNode importNode = list.Item(random.Next(list.Count));
                    node.AppendChild(newPackage.ImportNode(importNode, true));
                }
                
                //change IzvorGlavaId so it stays unique (database unique constraint)
                XmlNodeList nodeList = node.SelectNodes("//IzvorGlavaId");
                foreach (XmlNode item in nodeList) {
                    item.FirstChild.Value = st++.ToString();
                }

                //save the xml on the disk
                try {
                    xw = new XmlTextWriter(savePath + "." + generateId() + ".xml", null);
                    newPackage.Save(xw);
                } catch (Exception) {
                    MessageBox.Show("Napaka pri shranjevanju paketa.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } finally {
                    xw.Close();
                }
                progressBar1.Value += (int)1000/numPack;
                st = 0;
            }
            progressBar1.Value = 1000;
        }

        //generates unique strings
        private string generateId() {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray()) {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        #endregion

        #region //Send packages

        private void checkBoxCheckAll_CheckedChanged(object sender, EventArgs e) {
            changeCheckBox(sender);            
        }

        private void changeCheckBox(object sender) {
            CheckBox cb = (CheckBox)sender;
            if (cb.Name == "checkBoxCheckAll") {
                for (int i = 0; i < listBoxSendPackages.Items.Count; i++) {
                    listBoxSendPackages.SetSelected(i, cb.Checked);
                }
            }
        }

        /*
         * fills the listbox with the packages in the /generated/ or the specified folder
         */
        private void fillListBoxSendPackages() {
            listBoxSendPackages.Items.Clear();
            String s = "";

            if (!Directory.Exists(sendPath))
                sendPath = generatePath;

            foreach (Object item in Directory.GetFiles(sendPath)) {
                s = item.ToString();
                if (s.EndsWith(".xml")) {
                    s = s.Substring(s.LastIndexOf("\\") + 1);
                    Package p = new Package(s, null, sendPath + "\\");
                    listBoxSendPackages.Items.Add(p);
                }
            }

        }

        private void buttonSend_Click(object sender, EventArgs e) {

            textBoxResults.Clear();
            progressBar1.Value = 0;

            List<ThreadData> listThreadData = getListThreadDataFromForm();

            this.Cursor = Cursors.WaitCursor;
            
            //start all threads
            lib.startTesting(listThreadData);
            backgroundWorker1.RunWorkerAsync();

        }

        /*
         * gets the list, containing all the data on the form
         */
        private List<ThreadData> getListThreadDataFromForm() {
            //if there is a thread without any packages, throw exception
            foreach (TabPage item in tabControlThreads.TabPages) {
                ListBox lb = (ListBox)item.Controls[0];
                if (lb.Items.Count <= 0) {
                    MessageBox.Show("Nekateri threadi so brez paketov.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            int numThreads = tabControlThreads.TabCount;

            //get all the selected packages and other data
            List<ThreadData> listThreadData = new List<ThreadData>(numThreads);

            //for each tab in tabbed pane
            foreach (TabPage page in tabControlThreads.TabPages) {
                ListBox lb = (ListBox)page.Controls[0];

                List<Package> selectedPackages = new List<Package>();
                int numSendPackages = 1;
                int timeLag = 100;

                String packagePath = "";
                try {
                    //for each package in the tab
                    foreach (Package item in lb.Items) {
                        //if not already filled, fill it
                        if (item.PackageContent == "" || item.PackageContent == null) {
                            packagePath = item.Path + item.Name;
                            String packageString = null;
                            sr = new StreamReader(packagePath);
                            packageString = sr.ReadToEnd();
                            item.PackageContent = packageString; 
                        }
                        selectedPackages.Add(item);

                    }
                } catch (Exception e1) {
                    MessageBox.Show("Napaka pri branju paketov.\r\n" + e1.ToString(), "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } finally {
                    sr.Close();
                }

                TextBox tbNum = (TextBox)page.Controls[2];
                TextBox tbLag = (TextBox)page.Controls[4];
                try {
                    if (tbNum.Text != "")
                        numSendPackages = Int32.Parse(tbNum.Text);
                    if (tbLag.Text != "")
                        timeLag = Int32.Parse(tbLag.Text);
                } catch {
                    MessageBox.Show("Napaka pri branju številk.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                ThreadData td = new ThreadData(selectedPackages, numSendPackages, timeLag);
                listThreadData.Add(td);

            }
            return listThreadData;
        }

        /*
         * move the selected packages to the list for each thread
         */
        private void buttonAdd_Click(object sender, EventArgs e) {
            foreach (Object item in listBoxSendPackages.SelectedItems) {
                ListBox lb = (ListBox)tabControlThreads.SelectedTab.Controls[0];
                lb.Items.Add(item);
            }
            listBoxSendPackages.ClearSelected();            
        }

        /*
         * move the selection in the listbox of selected packages up
         */
        private void buttonUp_Click(object sender, EventArgs e) {
            ListBox lb = (ListBox)tabControlThreads.SelectedTab.Controls[0];
            int selectedIndex = lb.SelectedIndex;
            if (selectedIndex > 0) {
                Object selectedItem = lb.SelectedItem;            
                lb.Items.Remove(selectedItem);
                lb.Items.Insert(selectedIndex - 1, selectedItem);
                lb.SelectedIndex = selectedIndex - 1;
            }
        }

        /*
         * move the selection in the listbox of selected packages down
         */
        private void buttonDown_Click(object sender, EventArgs e) {
            ListBox lb = (ListBox)tabControlThreads.SelectedTab.Controls[0];
            int selectedIndex = lb.SelectedIndex;
            if (selectedIndex < lb.Items.Count - 1 && selectedIndex >= 0) {
                Object selectedItem = lb.SelectedItem;
                lb.Items.Remove(selectedItem);
                lb.Items.Insert(selectedIndex + 1, selectedItem);
                lb.SelectedIndex = selectedIndex + 1;
            }
        }

        /*
         * removes the selected packages
         */
        private void buttonRemove_Click(object sender, EventArgs e) {
            ListBox lb = (ListBox)tabControlThreads.SelectedTab.Controls[0];
            int selectedIndex = lb.SelectedIndex;
            if (selectedIndex >= 0) {                
                lb.Items.RemoveAt(selectedIndex);
            }
        }

        /*
         * removes all packages
         */
        private void buttonRemoveAll_Click(object sender, EventArgs e) {
            ListBox lb = (ListBox)tabControlThreads.SelectedTab.Controls[0];
            lb.Items.Clear();
        }

        /*
         * background thread for updating mouse cursor
         */
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            while (lib.threadsRunning()) {
                this.SetProgress(lib.ProgressValue);
                this.SetText(lib.Result);
                Thread.Sleep(50);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            progressBar1.Value = 1000;
            this.textBoxResults.Text = lib.Result;
            this.Cursor = Cursors.Arrow;
        }

        /*
         * for synchonized access to the text box with results in it
         */
        private void SetText(string text) {
            if (this.textBoxResults.InvokeRequired) {
                SetTextCallback callback = new SetTextCallback(SetText);
                this.Invoke(callback, new object[] { text });
            }
            else {
                this.textBoxResults.Text = text;
            }
        }

        /*
         * for synchonized access to the progress bar
         */
        private void SetProgress(int value) {
            if (this.progressBar1.InvokeRequired) {
                SetProgressCallback callback = new SetProgressCallback(SetProgress);
                this.Invoke(callback, new object[] { value });
            }
            else {
                this.progressBar1.Value = value;
            }
        }

        private void buttonFolder2_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                sendPath = folderBrowserDialog1.SelectedPath + "\\";
            }
            fillListBoxSendPackages();
        }

        /*
         * aborts all threads that are sending packets to the WS
         */
        private void buttonCancel_Click(object sender, EventArgs e) {
            lib.cancel();
            backgroundWorker1_RunWorkerCompleted(null, null);
        }

        /*
         * adds a new tab with all the necessary components on it
         */
        private void buttonAddThread_Click(object sender, EventArgs e) {
            TabPage tab = createTab(null, -1, -1);

            tabControlThreads.Controls.Add(tab);
            tab.UseVisualStyleBackColor = true;
            tabControlThreads.SelectedTab = tab;
        }

        private TabPage createTab(List<Package> packages, int numSend, int timeLag) {
            TabPage tab = new TabPage("thread " + threadCount.ToString());
            threadCount++;

            ListBox listbox = new ListBox();
            listbox.Bounds = new Rectangle(0, 0, 208, 147);
            if (packages != null) {
                foreach (Package package in packages) {
                    listbox.Items.Add(package);
                }
            }
            tab.Controls.Add(listbox);

            Label label = new Label();
            label.Text = "Vpiši kolikokrat se pošlje cel seznam (1)";
            label.Bounds = new Rectangle(6, 163, 193, 13);
            tab.Controls.Add(label);

            TextBox textbox = new TextBox();
            textbox.Bounds = new Rectangle(9, 179, 100, 20);
            if (numSend != -1)
                textbox.Text = numSend.ToString();
            tab.Controls.Add(textbox);

            Label label1 = new Label();
            label1.Text = "Vpiši časovni zamik v [ms] (100)";
            label1.Bounds = new Rectangle(6, 211, 157, 13);
            tab.Controls.Add(label1);

            TextBox textbox1 = new TextBox();
            textbox1.Bounds = new Rectangle(9, 227, 100, 20);
            if (timeLag != -1)
                textbox1.Text = timeLag.ToString();
            tab.Controls.Add(textbox1);

            return tab;
        }

        /*
         * removes the currently selected tab
         */
        private void buttonRemoveThread_Click(object sender, EventArgs e) {
            int index = tabControlThreads.SelectedIndex;
            if (tabControlThreads.Controls.Count > 1) {
                tabControlThreads.Controls.RemoveAt(index);
                if(index != 0)
                    tabControlThreads.SelectedIndex = index - 1;
            }
        }

        #endregion

        /*
         * saves the data on the form into a scenario.xml on the harddisc
         */
        private void buttonSaveScenario_Click(object sender, EventArgs e) {

            Stream stream = null;

            //open file dialog
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                stream = saveFileDialog1.OpenFile();
            }

            List<ThreadData> list = getListThreadDataFromForm();

            //create first 2 nodes: declaration and root
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDoc.AppendChild(node);

            XmlNode scenario = xmlDoc.CreateElement("Scenario");
            xmlDoc.AppendChild(scenario);

            foreach (ThreadData threadData in list) {
                XmlElement xmlThreadData = xmlDoc.CreateElement("ThreadData");
                xmlDoc.ChildNodes[1].AppendChild(xmlThreadData);

                XmlText text;

                //read all packages and append them
                foreach (Package package in threadData.Packages) {
                    XmlElement xmlPackage = xmlDoc.CreateElement("Package");

                    XmlElement xmlName = xmlDoc.CreateElement("Name");
                    text = xmlDoc.CreateTextNode(package.Name);
                    xmlName.AppendChild(text);
                    xmlPackage.AppendChild(xmlName);

                    XmlElement xmlPath = xmlDoc.CreateElement("Path");
                    text = xmlDoc.CreateTextNode(package.Path);
                    xmlPath.AppendChild(text);
                    xmlPackage.AppendChild(xmlPath);

                    xmlThreadData.AppendChild(xmlPackage);                    
                }

                //read numSend and append it
                XmlElement xmlNumSend = xmlDoc.CreateElement("NumSend");
                text = xmlDoc.CreateTextNode(threadData.NumSendPackages.ToString());
                xmlNumSend.AppendChild(text);
                xmlThreadData.AppendChild(xmlNumSend);

                //read timeLag and append it
                XmlElement xmlTimeLag = xmlDoc.CreateElement("TimeLag");
                text = xmlDoc.CreateTextNode(threadData.TimeLag.ToString());
                xmlTimeLag.AppendChild(text);
                xmlThreadData.AppendChild(xmlTimeLag);               

            }


            //write xml to disc
            XmlTextWriter xtw = null;
            try {
                xtw = new XmlTextWriter(stream, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;
                xmlDoc.Save(xtw);

            } catch (Exception exc) {
                MessageBox.Show("Napaka pri shranjevanju scenarija.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                xtw.Close();
            }
        }

        /*
         * loads previously saved scenario
         */
        private void buttonLoadScenario_Click(object sender, EventArgs e) {

            String filePath = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                filePath = openFileDialog1.FileName;
            }

            //get data from scenario
            List<ThreadData> listThreadData = xmlToThreadData(filePath);
            if (listThreadData == null)
                return;

            //remove all tabs
            tabControlThreads.Controls.Clear();

            foreach (ThreadData threadData in listThreadData) {
                TabPage tab = createTab(threadData.Packages, threadData.NumSendPackages, threadData.TimeLag);

                tabControlThreads.Controls.Add(tab);
                tab.UseVisualStyleBackColor = true;
                tabControlThreads.SelectedTab = tab;
            }

        }


        /*
         * gets the xml path, reads the xml and returns a thread data list
         */
        private List<ThreadData> xmlToThreadData(String xmlPath) {

            List<ThreadData> listThreadData = new List<ThreadData>();
            XmlDocument packageXml = new XmlDocument();
            sr = new StreamReader(xmlPath);
            try {
                String packageString = sr.ReadToEnd();
                packageXml.LoadXml(packageString);
            } catch (Exception e) {
                MessageBox.Show("Napaka pri branju scenarija.\r\n" + e.Message, "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            } finally {
                sr.Close();
            }

            XmlNodeList listXmlThread = packageXml.SelectNodes("//ThreadData");

            //read data from xml for each thread
            foreach (XmlNode thread in listXmlThread) {

                //read all packages
                XmlNodeList listXmlPackages = thread.SelectNodes("Package");
                List<Package> listPackages = new List<Package>(listXmlPackages.Count);
                foreach (XmlNode package in listXmlPackages) {
                    String packageName = package.SelectSingleNode("Name").InnerText;
                    String packagePath = package.SelectSingleNode("Path").InnerText;
                    String packageContent = "";
                    if (packagePath == "")
                        packagePath = dirPath;

                    try {
                        //read the actual package
                        String s = packagePath + packageName;
                        sr = new StreamReader(s);
                        packageContent = sr.ReadToEnd();
                    } catch (Exception e) {
                        MessageBox.Show("Napaka pri branju vrednosti paketa.\r\n" + e.Message, "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    } finally {
                        sr.Close();
                    }

                    Package p = new Package(packageName, packageContent, packagePath);
                    listPackages.Add(p);
                }

                //read numSend and timeLag
                XmlNode xmlNumSend = thread.SelectSingleNode("NumSend");
                XmlNode xmlTimeLag = thread.SelectSingleNode("TimeLag");

                int numSend = 1;
                int timeLag = 100;
                try {
                    if (xmlNumSend.InnerText != "")
                        numSend = Int32.Parse(xmlNumSend.InnerText);
                    if (xmlTimeLag.InnerText != "")
                        timeLag = Int32.Parse(xmlTimeLag.InnerText);
                } catch (Exception e) {
                    MessageBox.Show("Napaka pri branju numSend in timeLag v scenariju.\r\n" + e.Message, "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                //add this thread data to the list
                ThreadData td = new ThreadData(listPackages, numSend, timeLag);
                listThreadData.Add(td);
            }
            return listThreadData;
        }


    }
}
