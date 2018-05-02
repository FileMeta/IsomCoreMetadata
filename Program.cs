using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsomCoreMetadata
{
    /// <summary>
    /// Unit test for IsomCoreMetadata class.
    /// </summary>
    class Program
    {
        const string c_SrcMp4 = "Src.mp4";
        const string c_TestMp4 = "Test.mp4";

        const string c_expectedMajorBrand = "isom";
        const int c_expectedMinorVersion = 512;
        static string[] s_expectedCompatibleBrands = new string[] {"isom", "iso2", "avc1", "mp41"};
        static DateTime s_expectedCreationTime = new DateTime(2018, 5, 2, 6, 0, 0, DateTimeKind.Utc);
        static DateTime s_expectedModificationTime = new DateTime(2018, 5, 2, 13, 0, 0, DateTimeKind.Utc);
        static TimeSpan s_expectedDuration = new TimeSpan(2010000);

        static void Main(string[] args)
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;
                while (!File.Exists(Path.Combine(workingDirectory, c_SrcMp4)))
                {
                    workingDirectory = Path.GetDirectoryName(workingDirectory);
                    if (string.IsNullOrEmpty(workingDirectory))
                    {
                        throw new ApplicationException(string.Format("Test file, '{0}', not found in working directory path.", c_SrcMp4));
                    }
                }

                bool success = true;

                string srcFilename = Path.Combine(workingDirectory, c_SrcMp4);
                using (var isomMetadata = new FileMeta.IsomCoreMetadata(srcFilename))
                {
                    // === Check All Values ===

                    Console.WriteLine($"MajorBrand: {isomMetadata.MajorBrand}");
                    if (!isomMetadata.MajorBrand.Equals(c_expectedMajorBrand, StringComparison.Ordinal))
                    {
                        Console.WriteLine($"   Expected: {c_expectedMajorBrand}");
                        success = false;
                    }

                    Console.WriteLine($"MinorVersion: {isomMetadata.MinorVersion}");
                    if (isomMetadata.MinorVersion != c_expectedMinorVersion)
                    {
                        Console.WriteLine($"   Expected: {c_expectedMinorVersion}");
                        success = false;
                    }

                    Console.WriteLine($"CompatibleBrands: {string.Join(";", isomMetadata.CompatibleBrands)}");
                    if (!ArraysEqual(isomMetadata.CompatibleBrands, s_expectedCompatibleBrands))
                    {
                        Console.WriteLine($"   Expected: {string.Join(";", s_expectedCompatibleBrands)}");
                        success = false;
                    }

                    Console.WriteLine($"CreationTime: {isomMetadata.CreationTime:o}");
                    if (isomMetadata.CreationTime != s_expectedCreationTime)
                    {
                        Console.WriteLine($"   Expected: {s_expectedCreationTime:o}");
                        success = false;
                    }

                    Console.WriteLine($"ModificationTime: {isomMetadata.ModificationTime:o}");
                    if (isomMetadata.ModificationTime != s_expectedModificationTime)
                    {
                        Console.WriteLine($"   Expected: {s_expectedModificationTime:o}");
                        success = false;
                    }

                    Console.WriteLine($"Duration: {isomMetadata.Duration:g}");
                    if (isomMetadata.Duration != s_expectedDuration)
                    {
                        Console.WriteLine($"   Expected: {s_expectedDuration:g}");
                        success = false;
                    }
                    
                }

                // === Copy file ===

                File.Copy(Path.Combine(workingDirectory, c_SrcMp4), Path.Combine(workingDirectory, c_TestMp4), true);

                // === Generate New Creation and Modificaiton Times

                Console.WriteLine();
                Console.WriteLine("Update Test");
                Console.WriteLine();

                var newCreationTime = new DateTime(2018, 4, 19, 11, 22, 24, 0, DateTimeKind.Utc);
                var newModificationTime = newCreationTime.AddSeconds(45);

                using (var isomMetadata = new FileMeta.IsomCoreMetadata(Path.Combine(workingDirectory, c_TestMp4), true))
                {
                    isomMetadata.CreationTime = newCreationTime;
                    isomMetadata.ModificationTime = newModificationTime;
                    isomMetadata.Commit();
                }

                // Validate changed values
                using (var isomMetadata = new FileMeta.IsomCoreMetadata(Path.Combine(workingDirectory, c_TestMp4)))
                {
                    Console.WriteLine($"CreationTime: {isomMetadata.CreationTime:o}");
                    if (isomMetadata.CreationTime != newCreationTime)
                    {
                        Console.WriteLine($"   Expected: {newCreationTime:o}");
                        success = false;
                    }

                    Console.WriteLine($"ModificationTime: {isomMetadata.ModificationTime:o}");
                    if (isomMetadata.ModificationTime != newModificationTime)
                    {
                        Console.WriteLine($"   Expected: {newModificationTime:o}");
                        success = false;
                    }
                }

                Console.WriteLine();

                if (success)
                {
                    Console.WriteLine("All tests succeeded.");
                }
                else
                {
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Certain tests failed.");
                    Console.ForegroundColor = oldColor;
                }

            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

            Win32Interop.ConsoleHelper.PromptAndWaitIfSoleConsole();
        }

        static bool ArraysEqual(string[] a, string[] b)
        {
            if (a.Length != b.Length) return false;

            for(int i=0; i<a.Length; ++i)
            {
                if (!string.Equals(a[i], b[i], StringComparison.Ordinal)) return false;
            }

            return true;
        }
    }
}
