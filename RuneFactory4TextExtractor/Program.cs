using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RuneFactory4TextExtractor
{
    class Program
    {

        struct Header
        {
           public byte[] idtype;
           public int idcode;
           
  
        }
        public static int SwapEndianness(int value)
        {
            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;
            var b3 = (value >> 16) & 0xff;
            var b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }
        static void Main(string[] args)
        {
            Header header;
            if (args.Length > 0)
            {
                Console.WriteLine("argumento: " + args[0]);
            
            String namefile = args[0];
            
            using (BinaryReader reader = new BinaryReader(File.Open(namefile, FileMode.Open)))
            {
                header.idtype = reader.ReadBytes(4);
                header.idcode = reader.ReadInt32();
                long savepos = reader.BaseStream.Position;
                int firstLength = reader.ReadInt32();
                int firstOffset = reader.ReadInt32();
                string text = Encoding.UTF8.GetString(header.idtype);
                Console.WriteLine("Type: " + text);
                Console.WriteLine("Code: " + header.idcode.ToString("X"));
                
                Console.WriteLine("Initial Offset: " + firstOffset.ToString("X"));
                reader.BaseStream.Position = savepos;
                
                List<string> result = new List<string>();
                while (reader.BaseStream.Position < firstOffset)
                {
                    int textLength = reader.ReadInt32();
                    int textOffset = reader.ReadInt32();
                    Console.WriteLine("Length: " + textLength.ToString("X"));
                    Console.WriteLine("Offset: " + textOffset.ToString("X"));
                    long gotooffset = reader.BaseStream.Position;
                    reader.BaseStream.Position = textOffset;
                    Byte[] texttemp = reader.ReadBytes(textLength);
                    string texttemputf8 = Encoding.UTF8.GetString(texttemp).Replace("\n", "{LF}");
                    result.Add(texttemputf8);

                  
                    Console.WriteLine("Line: " + texttemputf8);
                    reader.BaseStream.Position = gotooffset;
                   
                    
                }
                reader.Close();
                File.WriteAllLines(namefile + ".txt", result);
                }
            }
            Console.WriteLine("inserta el fichero en ingles");
        }
    }
}
