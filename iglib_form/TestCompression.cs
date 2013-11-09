using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace IG.test
{

    public class GZipTest  // CAN BE DELETED! - Testiranje kompresije in dekompresije
    {
        private const int buffer_size = 40;


        private static void ReportError(Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }


        public static void ReadFileToMemoryStream(string sourcefile, MemoryStream ms)
        // Reads a file and writes its contents to a memory stream.
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(sourcefile, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buffer = new byte[buffer_size];
                int count = 0;
                do
                {
                    count = fs.Read(buffer, 0, buffer_size);
                    if (count > 0)
                        ms.Write(buffer, 0, count);
                } while (count > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\nError: " + ex.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        public static int ReadAllBytesFromStream(Stream stream, byte[] buffer)
        {
            // Use this method is used to read all bytes from a stream.
            int offset = 0;
            int totalCount = 0;
            while (true)
            {
                int bytesRead = stream.Read(buffer, offset, buffer_size);
                if (bytesRead == 0)
                {
                    break;
                }
                offset += bytesRead;
                totalCount += bytesRead;
            }
            return totalCount;
        }


        /// <summary>Compares data from two defferent buffers.</summary>
        /// <returns>true if buffers contain the same data, false otherwise.</returns>
        public static bool CompareData(byte[] buf1, int len1, byte[] buf2, int len2)
        {
            // Use this method to compare data from two different buffers.
            if (len1 != len2)
            {
                Console.WriteLine("Number of bytes in two buffer are different {0}:{1}", len1, len2);
                return false;
            }

            for (int i = 0; i < len1; i++)
            {
                if (buf1[i] != buf2[i])
                {
                    Console.WriteLine("byte {0} is different {1}|{2}", i, buf1[i], buf2[i]);
                    return false;
                }
            }
            Console.WriteLine("All bytes compare.");
            return true;
        }

        // HINTS: dealing with encoding
        // System.Text.ASCIIEncoding.ASCII.Get
        // sizeof(char);

        //System.Text.Encoding.Default.GetString(
        //System.Text.Encoding.Default.GetBytes("ÅåÄäÖö"));
        //Convert.ToBase64CharArray(inArray,offsetIn,length,outArray,offsetOut);
        //Convert.FromBase64CharArray(inArray,offset,length);
        //System.Text.Encoding.Convert(...)
        //System.Text.Encoding.GetEncoding(int encoding)  0 - default encoding (=code table) 

        public static byte[] CompressBytes(byte[] Original)
        // Compresses the byte array Original by using the GZipStream and returns the compressed array.
        // $A Igor sep08;
        {
            byte[] zippedArray = null;
            MemoryStream ms = null;
            GZipStream compressedzipStream = null;
            try
            {
                // Create a memory stream for compressed data:
                ms = new MemoryStream();
                compressedzipStream = new GZipStream(ms, CompressionMode.Compress);
                compressedzipStream.Write(Original, 0, Original.Length);
                // WARNING: The GZipStream MUST be closed before the data that is written is read:
                compressedzipStream.Close();
                compressedzipStream = null;
                // Warning: after closing compressedzipStream, ms.Position may not be set!
                //ms.Flush();
                //ms.Position = 0;
                zippedArray = ms.ToArray();
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }
            finally
            {
                try
                {
                    //Close the streams:
                    if (compressedzipStream != null)
                        compressedzipStream.Close();
                    if (ms != null)
                        ms.Close();
                }
                catch { }
            }
            return zippedArray;
        }


        public static byte[] UncompressBytes(byte[] zippedArray)
        // Uncompresses the byte array Original by using the GZipStream and returns the compressed array.
        // $A Igor sep08;
        {
            int BufferSize = 40;
            byte[] unzippedArray = null;
            MemoryStream msIn = null, msOut = null;
            GZipStream zipStream = null;
            try
            {
                bool InstantiateFromArray = true;
                if (InstantiateFromArray)
                {
                    // Instantiate a memory stream on an array:
                    msIn = new MemoryStream(zippedArray);
                }
                else
                {
                    // Instantiate an empty memory stream and write the array into it:
                    msIn = new MemoryStream();
                    msIn.Write(zippedArray, 0, zippedArray.Length);
                }
                // Reset the memory stream position to begin decompression.
                msIn.Position = 0;
                zipStream = new GZipStream(msIn, CompressionMode.Decompress);
                msOut = new MemoryStream();
                byte[] Buffer = new byte[BufferSize];
                int totalRead = 0, bytesRead = 0;
                do
                {
                    bytesRead = zipStream.Read(Buffer, 0, BufferSize);
                    if (bytesRead > 0)
                    {
                        totalRead += bytesRead;
                        msOut.Write(Buffer, 0, bytesRead);
                    }
                } while (bytesRead > 0);
                unzippedArray = msOut.ToArray();
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }
            finally
            {
                try
                {
                    //Close the streams:
                    if (zipStream != null)
                        zipStream.Close();
                    if (msIn != null)
                        msIn.Close();
                    if (msOut != null)
                        msOut.Close();
                }
                catch { }
            }
            return unzippedArray;
        }




        public static void GZipCompressDecompress(string filename)
        {
            Console.WriteLine("Test compression and decompression on file {0}", filename);
            FileStream infile;
            try
            {

                // Read the file contents into Original:
                infile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] Original = new byte[infile.Length];
                int count = infile.Read(Original, 0, Original.Length);
                if (count != Original.Length)
                {
                    infile.Close();
                    Console.WriteLine("Test Failed: Unable to read data from file");
                    return;
                }
                infile.Close();


                // Definitions of variables:
                byte[] zippedArray = null;
                byte[] unzippedArray = null;
                // MemoryStream msIn = null;

                int totalCount = 0;



                Console.WriteLine("Compression");
                zippedArray = CompressBytes(Original);
                Console.WriteLine("Original size: {0}, Compressed size: {1}", Original.Length, zippedArray.Length);



                //MemoryStream ms = new MemoryStream();
                //// Use the newly created memory stream for the compressed data.
                //GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
                //Console.WriteLine("Compression");
                //compressedzipStream.Write(Original, 0, Original.Length);
                //// Close the stream.
                //compressedzipStream.Close();
                //Console.WriteLine("Original size: {0}, Compressed size: {1}", Original.Length, ms.Length);

                //bool UseTheSameStream = false;

                //if (UseTheSameStream)
                //{
                //    msIn = ms;
                //} else
                //{
                //    // Instead of using the same stream for uncompression, we convert the old stream to
                //    // a byte array, create a stream from it, and instantiate uncompressing GZipStream
                //    // from this stream.
                //    ms.Position = 0;
                //    zippedArray = ms.ToArray();
                //    ms.Close();
                //    // re-open a memory stream and initialize it with the byte array:
                //    bool InstantiateFromArray = true;
                //    if (InstantiateFromArray)
                //    {
                //        // Instantiate a memory stream on an array:
                //        msIn = new MemoryStream(zippedArray);
                //    }
                //    else
                //    {
                //        // Instantiate an empty memory stream and write the array into it:
                //        msIn = new MemoryStream();
                //        msIn.Write(zippedArray, 0, zippedArray.Length);
                //    }
                //}











                //bool InstantiateFromArray = true;
                //if (InstantiateFromArray)
                //{
                //    // Instantiate a memory stream on an array:
                //    msIn = new MemoryStream(zippedArray);
                //}
                //else
                //{
                //    // Instantiate an empty memory stream and write the array into it:
                //    msIn = new MemoryStream();
                //    msIn.Write(zippedArray, 0, zippedArray.Length);
                //}


                GZipStream zipStream = null;

                //// Reset the memory stream position to begin decompression.
                //msIn.Position = 0;
                //GZipStream zipStream = new GZipStream(msIn, CompressionMode.Decompress);
                //Console.WriteLine("Decompression");


                //int BufferSize = 40;
                //MemoryStream msOut = new MemoryStream();
                //byte[] Buffer = new byte[BufferSize];
                //int totalRead = 0, bytesRead = 0;
                //do
                //{
                //    bytesRead = zipStream.Read(Buffer, 0, BufferSize);
                //    if (bytesRead > 0)
                //    {
                //        totalRead += bytesRead;
                //        msOut.Write(Buffer, 0, bytesRead);
                //    }
                //} while (bytesRead > 0);
                //unzippedArray = msOut.ToArray();

                //totalCount = unzippedArray.Length; 


                unzippedArray = UncompressBytes(zippedArray);
                totalCount = unzippedArray.Length;





                //unzippedArray = new byte[Original.Length + buffer_size];
                //int bytesRead1 = 0;
                //totalCount = 0, bytesRead1 = 0;
                //int offset = 0;
                //do
                //{
                //    bytesRead1 = zipStream.Read(unzippedArray, offset, buffer_size);
                //    if (bytesRead1 != 0)
                //    {
                //        offset += bytesRead1;
                //        totalCount += bytesRead1;
                //    }
                //} while (bytesRead1 > 0);



                // Use the ReadAllBytesFromStream to read the stream.
                // int totalCount = GZipTest.ReadAllBytesFromStream(zipStream, unzippedArray);
                Console.WriteLine("Decompressed {0} bytes", totalCount);

                if (!GZipTest.CompareData(Original, Original.Length, unzippedArray, totalCount))
                {
                    Console.WriteLine("Error. The two buffers did NOT compare.");
                }
                if (zipStream != null)
                    zipStream.Close();
            } // end try
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }





        public static void Main0(string[] args)
        {
            string usageText = "Usage: " + Environment.GetCommandLineArgs()[0] + " <inputfilename> ";
            // 
            string filename = null;
            if (args.Length == 0)
            {
                Console.WriteLine(usageText);
                Console.WriteLine("\nChoose the file to compress and decompress in memory!");

                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Title = "Insert the name of the file to compress/uncompress!";
                openfile.InitialDirectory = Environment.CurrentDirectory;
                openfile.FileName = "readme.txt";
                filename = "..\\..\\readme.txt";
                if (!File.Exists(filename))
                {
                    if (openfile.ShowDialog() == DialogResult.OK)
                    {
                        filename = openfile.FileName;
                    }
                    else
                    {
                        Console.WriteLine("File name: ");
                        filename = Console.ReadLine();
                    }
                }
            }
            else
            {
                filename = args[0];
            }
            if (filename != null)
            {
                if (filename.Length > 0)
                {
                    if (File.Exists(filename))
                        GZipCompressDecompress(filename);
                    else
                        Console.WriteLine("\nFile does not exist: ");
                }
                else
                    Console.WriteLine("\nInvalid file name (zero length).");
            }
            else
                Console.WriteLine("\nInvalid file name (null string).");

        }
    }  // class GZipTest



}
