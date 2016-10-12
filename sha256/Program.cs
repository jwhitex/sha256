using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace sha256
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string path;
            var curdir = Directory.GetCurrentDirectory();
            var fbd = new OpenFileDialog { InitialDirectory = curdir };

            var dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                path = fbd.FileName;
            }
            else
            {
                Console.WriteLine("No directory selected.");
                return;
            }

            try
            {
                var file = new FileInfo(path);
                if (!file.Exists)
                {
                    Console.WriteLine("Could not find file specified.");
                    return;
                }
                var parentDirectory = file.Directory;
                if (parentDirectory == null)
                {
                    Console.WriteLine("Could not find the files parent directory");
                    return;
                }

                if (args.Length > 0 && args[0] == "-md5")
                {
                    var md5 = MD5.Create();


                    using (var fileStream = file.Open(FileMode.Open))
                    {
                        // Compute and print the hash values for each file in directory.
                        fileStream.Position = 0;
                        var hashValue = md5.ComputeHash(fileStream);
                        var byteString = GetByteString(hashValue);
                        var byteStringLower = byteString.ToLower().Replace(" ", "");
                        var indexOfExtention = file.Name.LastIndexOf(".", StringComparison.Ordinal);
                        var fileNameNoExt = file.Name.Substring(0, indexOfExtention);

                        using (var fileStream2 = new FileStream(parentDirectory.FullName + $"/SHA256_{fileNameNoExt}.txt", FileMode.Create))
                        {
                            var streamWriter = new StreamWriter(fileStream2);
                            streamWriter.WriteLine(byteString);
                            streamWriter.WriteLine(byteStringLower);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                    }

                }
                else
                {
                    // Initialize a SHA256 hash object.
                    var mySha256 = SHA256.Create();

                    // Create a fileStream for the file.
                    using (var fileStream = file.Open(FileMode.Open))
                    {
                        // Compute and print the hash values for each file in directory.
                        fileStream.Position = 0;
                        var hashValue = mySha256.ComputeHash(fileStream);
                        var byteString = GetByteString(hashValue);
                        var byteStringLower = byteString.ToLower().Replace(" ", "");
                        var indexOfExtention = file.Name.LastIndexOf(".", StringComparison.Ordinal);
                        var fileNameNoExt = file.Name.Substring(0, indexOfExtention);

                        using (var fileStream2 = new FileStream(parentDirectory.FullName + $"/SHA256_{fileNameNoExt}.txt", FileMode.Create))
                        {
                            var streamWriter = new StreamWriter(fileStream2);
                            streamWriter.WriteLine(byteString);
                            streamWriter.WriteLine(byteStringLower);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                    }
                }              
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Error: The directory specified could not be found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error: A file in the directory could not be accessed.");
            }
        }

        // Print the byte array in a readable format.
        public static string GetByteString(byte[] array)
        {
            var byteString = string.Empty;
            for (var i = 0; i < array.Length; i++)
            {
                byteString += $"{array[i]:X2}";
                if ((i % 4) == 3) byteString += " ";
            }
            return byteString;
        }
    }
}
