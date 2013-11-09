using TestWS_Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace EFA_Tests
{
    
    
    /// <summary>
    ///This is a test class for Test_ConsoleTest and is intended
    ///to contain all Test_ConsoleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Test_ConsoleTest {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /*
        POZOR: pri takem testiranju bo prikazana le zadnja napaka, ce se testira seznam scenarijev.
        Za morebitne ostale napake je potrebno pogledati .loge.
        
        POZOR2: tudi ce bo run metoda kje prisla do exceptiona, se tega v testu ne bo videlo.
        (naceloma, ce pride pri obeh xml-jih, ki jih primerjamo, do neke napake, je test se vedno uspel).
        Take napake se vidijo v .logih.
        */

        /// <summary>
        ///A test for run
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TestWS_Console.exe")]
        public void runTest() {
            //Uporaba:  args[0] = 1 (posamezen scenarij) ali 2 (seznam secenarijev)
            //          args[1] = pot do scenarija oz. seznama
            string[] args = {"2", @"C:\DevProjects\eFakturiranje\EFA_Solution\95_Test\TestWS_Console\Scenarios\scenario_list.txt"};
            bool expected = true;
            bool actual;
            actual = Test_Console_Accessor.run(args);
            string message = Test_Console_Accessor.resultMessage;
            Assert.AreEqual(expected, actual, message);
        }
    }
}
