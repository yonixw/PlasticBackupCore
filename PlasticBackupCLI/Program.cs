using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/*
    Special thank you to 
     **** StackOverflow ****
    and the many, many users who help me everyday!
*/

namespace PlasticBackupCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStream memory = new MemoryStream();
            GZipStream zippedMemory = new GZipStream(memory, CompressionMode.Compress);

            string EncryptionKey = "MAKV2SPBNI99212";
            Aes encryptor = Aes.Create();

            Rfc2898DeriveBytes pdb = new
                Rfc2898DeriveBytes(EncryptionKey, new byte[]
                { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            CryptoStream cs = new CryptoStream(zippedMemory, encryptor.CreateEncryptor(), CryptoStreamMode.Write);

            string toWrite = "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello";
            byte[] toWriteByte = Encoding.ASCII.GetBytes(toWrite);
            byte[] readBuffer = new byte[2048];
            int bytesRead = 0;

            memory.Position = 0;
            memory.Write(toWriteByte, 0, toWriteByte.Length);
            memory.Position = 0;
            bytesRead = memory.Read(readBuffer, 0, readBuffer.Length);
            Console.WriteLine("ms:" + Encoding.ASCII.GetString(readBuffer, 0, bytesRead));

            memory.Position = 0;
            zippedMemory.Write(toWriteByte, 0, toWriteByte.Length);
            memory.Position = 0;
            bytesRead = memory.Read(readBuffer, 0, readBuffer.Length);
            Console.WriteLine("zipped:" + Encoding.ASCII.GetString(readBuffer, 0, bytesRead));

            memory.Position = 0;
            cs.Write(toWriteByte, 0, toWriteByte.Length);
            memory.Position = 0;
            bytesRead = memory.Read(readBuffer, 0, readBuffer.Length);
            Console.WriteLine("cs:" + Encoding.ASCII.GetString(readBuffer, 0, bytesRead));

            Console.ReadLine();

            /*
            SHA256 hashAlg = new SHA256Managed();
            CryptoStream cs = new CryptoStream(_out, hashAlg, CryptoStreamMode.Write);
            // Write data here
            cs.FlushFinalBlock();
            byte[] hash = hashAlg.Hash;
            */
        }
    }
}
