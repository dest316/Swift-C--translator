using System;
using System.IO;

namespace ImprovedSimpleLexer
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader("TextFile1.txt");
            CalcLexer lex;
            string text = "";
            try
            {
                while (!file.EndOfStream)
                {
                    text += (file.ReadLine() + "\n");
                    
                }
                lex = new CalcLexer(text);
                //while (!lex.EOF())
                //{
                //    Console.WriteLine(lex.getToken());
                //}
                Parser parser = new Parser(lex);
                //Console.WriteLine(parser.parse());
                //parser.printTables();
                
                CodeGeneration codeGeneration = new CodeGeneration(parser.parse());
                Console.WriteLine(codeGeneration);
                //parser.parse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
