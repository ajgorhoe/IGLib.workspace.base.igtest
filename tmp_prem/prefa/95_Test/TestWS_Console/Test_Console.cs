using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestWS_Lib;
using System.Xml;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TestWS_Console {

    /*
     * Pozor:
     * Vsak scenarij mora imeti napisano pot do pricakovanega (pravilnega) rezultata.
     * Ce se bo ta xml razlikoval od trenutno dobljenega, se bo pojavila izjema.
     */

    class Test_Console {

        private static StreamReader sr;
        private static TestWS_Lib.TestWS_Lib lib;
        private static String expectedResult;
        private static bool resultOK;
        private static String resultMessage;

        static void Main(string[] args) {

            if (args.Length != 2) {
                Console.WriteLine("Uporaba:\targs[0] = 1 (posamezen scenarij) ali 2 (seznam scenarijev)");
                Console.WriteLine("\t\targs[1] = pot do scenarija oz. seznama");
                Console.ReadLine();
                return;
            }

            run(args);

        }

        private static bool run(string[] args) {

            resultOK = true;
            resultMessage = "";

            String dirPath = args[1].Remove(args[1].IndexOf(@"\Scenarios"));
            String logPath = "";
            String result = "";
            String[] listScenario = null;

            switch (args[0]) {
                case "1":
                    //only one scenario
                    listScenario = new String[1];
                    listScenario[0] = args[1];
                    break;

                case "2":
                    //scenario list
                    sr = new StreamReader(args[1]);
                    listScenario = Regex.Split(sr.ReadToEnd(), "\r\n");
                    break;

                default:
                    throw new Exception("Napaka pri zacetnih vrednostih.");
            }

            lib = new TestWS_Lib.TestWS_Lib();

            //execute every scenario in the list
            foreach (String scenario in listScenario) {

                result = "";

                Console.WriteLine("\tScenarij: " + scenario);
                result += "\tScenarij: " + scenario + "\r\n";

                //create path for writing xmls and logs
                String date = DateTime.Now.ToString().Replace(":", ".");
                logPath = scenario.Remove(scenario.IndexOf(".xml")) + "_" + date + ".log";

                //if everything is ok with the input xml
                try {
                    List<ThreadData> listThreadData = xmlToThreadData(dirPath, scenario);

                    lib.startTesting(listThreadData);
                    result += displayResults(lib);

                    //write xml to disc
                    writeXml(logPath);

                    //compare this xml with the expected result
                    compareXmls(scenario);

                } catch (Exception e) {
                    Console.Error.WriteLine("NAPAKA: " + e.Message + "\r\n");
                    result += e.ToString() + "\r\n";
                }

                //write a log file to hard disc
                writeLog(logPath, result);

            }


            //Console.WriteLine("Press enter...");
            //Console.ReadLine();

            return resultOK;

        }

        /*
         * compares the xml from the WS with the expected result
         */
        private static void compareXmls(String currentScenario) {

            XmlDocument xmlActualResult = lib.ResultDocument;

            //read expected result from harddisc
            XmlDocument xmlExpectedResult = new XmlDocument();
            try {
                sr = new StreamReader(expectedResult);
                String packageString = sr.ReadToEnd();
                xmlExpectedResult.LoadXml(packageString);
            } catch (Exception e) {
                throw new Exception("Napaka pri branju pricakovanega rezultata.\r\n", e);
            } finally {
                sr.Close();
            }

            //go over all threads
            int i = 0;
            while (true) {
                String xpath = "//InternalThread[@id='" + i + "']/Package";
                XmlNodeList listExpected = xmlExpectedResult.SelectNodes(xpath);

                //if there are no more threads, break
                if (listExpected.Count == 0) {
                    break;
                }

                //for every package in a thread, get his name and status and 
                //check if the actual result also contains it
                foreach (XmlNode package in listExpected) {
                    String name = package.SelectSingleNode("Name").InnerText;
                    String status = package.SelectSingleNode("Status").InnerText;

                    //if status = 'Sprejet', then just check 'Name' and 'Status'
                    if (status == "Sprejet") {
                        String xpath1 = xpath + "[Name='" + name + "' and Status='" + status + "']";

                        XmlNodeList actualResult = xmlActualResult.SelectNodes(xpath1);

                        //if there are none, then xmls are not the same
                        //there can be more than one, because one thread can send one same package many times
                        if (actualResult.Count == 0) {
                            resultOK = false;
                            resultMessage += "V scenariju " + currentScenario + " se xml-ja razlikujeta pri: " + xpath1 + "\r\n";
                            //throw new Exception(resultMessage);
                        }
                    }
                    //else check also 'ErrorList'
                    else {
                        XmlNodeList errorlist = package.SelectNodes("ErrorList");
                        foreach (XmlNode error in errorlist) {
                            String xpath1 = xpath + "[Name='" + name + "' and Status='" + status +
                                            "' and ErrorList='" + error.InnerText + "']";

                            XmlNodeList actualResult = xmlActualResult.SelectNodes(xpath1);

                            if (actualResult.Count == 0) {
                                resultOK = false;
                                resultMessage += "V scenariju " + currentScenario + " se xml-ja razlikujeta pri: " + xpath1 + "\r\n";
                                //throw new Exception(resultMessage);
                            }
                        }
                    }

                }

                i++;
            }

        }

        /*
         * writes the result xml to harddisc
         */
        private static void writeXml(String logPath) {
            XmlTextWriter xtw = null;
            try {
                xtw = new XmlTextWriter(logPath.Remove(logPath.IndexOf(".log")) + ".xml", Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;
                lib.ResultDocument.Save(xtw);
            } catch (Exception exc) {
                throw new Exception("Napaka pri shranjevanju xml result dokumenta.\r\n", exc);
            } finally {
                xtw.Close();
            }
        }

        /*
         * writes the result log to harddisc
         */
        private static void writeLog(String logPath, String result) {
            StreamWriter sw = null;
            try {
                sw = new StreamWriter(logPath);
                sw.Write(result);
            } catch (Exception e) {
                throw new Exception("Napaka pri pisanju .log datoteke.\r\n", e);
            } finally {
                sw.Close();
            }
        }

        // displays the results from WS
        private static String displayResults(TestWS_Lib.TestWS_Lib lib) {
            String s = "";
            int i = 1;
            while (lib.threadsRunning()) {
                if (lib.ProgressValue > i * 100) {
                    Console.Write("*****");
                    i++;
                }
                Thread.Sleep(50);
            }
            s += "\r\n" + lib.Result + "\r\n";
            Console.WriteLine("\r\n");
            Console.WriteLine(lib.Result + "\r\n");
            return s;
        }

        /*
         * reads xml data into a list
         */
        private static List<ThreadData> xmlToThreadData(String dirPath, String xmlPath) {

            List<ThreadData> listThreadData = new List<ThreadData>();
            XmlDocument packageXml = new XmlDocument();
            sr = new StreamReader(xmlPath);
            try {
                String packageString = sr.ReadToEnd();
                packageXml.LoadXml(packageString);
            } catch (Exception e) {
                throw new Exception("Napaka pri branju scenarija.\r\n", e);
            } finally {
                sr.Close();
            }

            //get the filename of the expected xml
            XmlNode node = packageXml.SelectSingleNode("//ExpectedResult");
            try {
                expectedResult = node.InnerText;
            } catch (Exception e) {
                throw new Exception("V scenariju ni napisanega predvidenega rezultata.\r\n", e);
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
                        packagePath = dirPath.Remove(dirPath.IndexOf("TestWS_Console")) + "TestWS_GUI\\TestniPaketi\\";

                    try {
                        //read the actual package
                        String s = packagePath + packageName;
                        sr = new StreamReader(s);
                        packageContent = sr.ReadToEnd();
                    } catch (Exception e) {
                        throw new Exception("Napaka pri branju vrednosti paketa.\r\n", e);
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
                    throw new Exception("Napaka pri branju vrednosti numSend in timeLag v scenariju.\r\n", e);
                }
                //add this thread data to the list
                ThreadData td = new ThreadData(listPackages, numSend, timeLag);
                listThreadData.Add(td);
            }
            return listThreadData;
        }
    }
}
