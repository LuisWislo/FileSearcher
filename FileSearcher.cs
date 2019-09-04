using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSearcher
{
    class Piece
    {
        int unary = 0; //0 - no lock, 1 - positive, 2 - kleene
        string symbol;
        List<Piece> spawningAutomata;
        bool isAtomic = true;
        

        public Piece(){}

        public Piece(string symbol)
        {
            this.symbol = symbol;
        }

        public Piece(string symbol, int unary)
        {
            this.symbol = symbol;
            this.unary = unary;
        }

        public void AddSymbol(string symbol)
        {
            this.symbol = symbol;
        }

        public void SetUnary(int unary)
        {
            this.unary = unary;
        }

        public void SetAtomic(bool isAtomic)
        {
            this.isAtomic = isAtomic;
        }

        public override string ToString()
        {
            string unary = "";
            switch(this.unary)
            {
                case 1: unary="#"; break;
                case 2: unary="*"; break;
            }
            return this.symbol+unary;
        }

        public bool IsAtomic()
        {
            return this.isAtomic;
        }



    }
    class FileSearcher
    {
        static void Main(String[] args)
        {
            //PrintFiles("C:\\Users\\roflm\\Desktop\\Carpeta");
            FilesWithRegex("a*+b*c*+d","lol");
        }
        
        static int GetUnary(int current, string regex) //supposed to only work with chracters
        {
            if(current<regex.Length-1)
            {
                if(regex[current+1] == '*') //checks for unary operation (kleene)
                {
                    return 2;
                }

                if(regex[current+1] == '#') //checks for unary operation (positive)
                {
                    return 1;
                }

                return 0;
            }

            return 0;
            // none -> 0
            // positive -> 1
            // kleene -> 2
        }

        private static int GetBinary(int current, string regex)
        {
            switch(regex[current])
            {
                case '+': return 1; //we have union
                default: return 0;  //we have concatenation
            }
        }

        static void FilesWithRegex(string regex, string directory)
        {
            List<List<Piece>> automatas = ParseRegex(regex);

            foreach(List<Piece> automata in automatas)
            {
                TestAutomata(automata, regex, 0);
            }
            
            foreach(List<Piece> automata in automatas)
            {
                Console.Write("Automata: ");
                foreach(Piece piece in automata)
                {
                    Console.Write(piece.ToString()+", ");
                }

                Console.WriteLine();
            } 
            //we need to create another automata
            //check if there is anything between them
            //nothing - concatenation
            //+ create new automata
            // parentheses make up a whole symbol
            //check for locks
 
        }

        static void TestAutomata(List<Piece> automata, string regex, int currentChar) //will also need directory
        {
            foreach(Piece piece in automata)
            {
                if(piece.IsAtomic())
                {

                }
                else
                {
                    //TestAutomata()
                }
            }
        }

        static List<List<Piece>> ParseRegex(string regex)
        {
            List<List<Piece>> automatas = new List<List<Piece>>();
            //while loop
            List<Piece> currentAutomata = new List<Piece>();
            //a+b*
            for (int i = 0; i < regex.Length; i++)
            {
                int binary = GetBinary(i,regex);
                if(binary == 1) //means we found union
                {
                    automatas.Add(currentAutomata);
                    currentAutomata = new List<Piece>();
                    i++;
                }
                //check if we have parentheses first
                //if(regex[i] == '('){}
                    //Do the parentheses thing
                int unary = GetUnary(i, regex);
                Piece p = new Piece(regex[i].ToString(), unary);
                currentAutomata.Add(p);
                if(unary>0) i++;
            }

            automatas.Add(currentAutomata);
            return automatas;
        }

        static void PrintFiles(string directory)
        {
            foreach(string f in Directory.GetFiles(directory))
            {
                Console.WriteLine(f);
            }

            foreach(string d in Directory.GetDirectories(directory))
            {
                PrintFiles(d);
            }
        }


    }
}
    

