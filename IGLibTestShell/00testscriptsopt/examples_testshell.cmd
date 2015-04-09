
C This script contains some examples of running commands form built in scripts.

C C Get information about the installed commands:
C Internal IG.Script.AppTestShell ?



C ==============================================
C WEB SERVICE applications:

C Internal IG.Script.AppTestShell WebService ?

C ====
C WEB SERVICE CLIENTS:

C C Run the GUI-based test WS clients:
C Internal IG.Script.AppTestShell WebService TestClient

C C Run the custom tests with WS clients enabled through proxy classes:
C C Internal IG.Script.AppTestShell WebService CustomTest <ServiceBaseURL>
C Internal IG.Script.AppTestShell WebService CustomTest
C Internal IG.Script.AppTestShell WebService http://localhost:8080/



C ==============================================
C DEVELOPMENT applications:

C Internal IG.Script.AppTestShell Develop ?

C ====
C DEVELOPMENT - NUMERICS:

C C Run the demo of Newton's method for finding function zeros:
C Internal IG.Script.AppTestShell Develop TestZeroFinderNewton


C ====
C DEVELOPMENT - FORMS:

C C Run the window positioning test:
C Internal IG.Script.AppTestShell FormDemo WindowPositioning



C ==============================================
C DATA STRUCTURES related embedded applications:

C Internal IG.Script.AppTestShell DataStructures ?

C C Run the multidimensional array demo:
C Internal IG.Script.AppTestShell DataStructures TestMultiDimArray

C C Run the CSV demo:
C Internal IG.Script.AppTestShell DataStructures TestCsv

C C Writing data definitions and data in CSV:
C C Internal IG.Script.AppTestShell DataStructures CsvWriteDefinitionAndData 
C C     <definitionFile> <dataFile> <csvResultFile> <sameRow> <indentation>
C Internal IG.Script.AppTestShell DataStructures CsvWriteDefinitionAndData "" "" "" false 2

C C Reading data definitions and data in CSV:
C C Internal IG.Script.AppTestShell DataStructures CsvReadDefinitionAndData 
C C     <inputCsvFile> <outputCsvFile> <sameRow> <indentation>
C Internal IG.Script.AppTestShell DataStructures CsvReadDefinitionAndData "" "" false 0


C ==============================================
C NEURAL NETWORKS related embedded applications (NOT NATIVE):

C Internal IG.Script.AppTestShell NeuralDemo ?

C C Run the ANN approximation demo:
C Internal IG.Script.AppTestShell NeuralDemo TestApp

C C Run the ANN model - based parametric tests:
C Internal IG.Script.AppTestShell NeuralDemo ParametricTests


C C Some OLD Applications and Tests: 

C Run the OLD version of ANN approximation demo:
C Internal IG.Script.AppTestShell NeuralDemo TestAppOld

C C Run the OLD version of 1D ANN approximation demo:
C Internal IG.Script.AppTestShell NeuralDemo TestApp1DOld

C C Run the OLD version of 2D ANN approximation demo:
C Internal IG.Script.AppTestShell NeuralDemo TestApp2DOld



