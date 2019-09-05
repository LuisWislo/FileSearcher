using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSearcher
{
    class Piece
    {
        int unary; //0 - no lock, 1 - positive, 2 - kleene
        string symbol;
        List<Piece> spawningAutomata;
        bool isAtomic = true;
        

        public Piece()
        {
            this.spawningAutomata = new List<Piece>();
        }

        public Piece(string symbol)
        {
            this.symbol = symbol;
            this.spawningAutomata = new List<Piece>();
        }

        public Piece(string symbol, int unary)
        {
            this.symbol = symbol;
            this.unary = unary;
            this.spawningAutomata = new List<Piece>();
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

        public List<Piece> GetSpawningAutomata()
        {
            return this.spawningAutomata;
        }

        public void AddPiece(Piece piece)
        {
            this.spawningAutomata.Add(piece);
        }

        public string GetUnary()
        {
            switch(this.unary)
            {
                case 1: return "-Positive";
                case 2: return "-Kleene";
                default: return "-Default";
            }
        }


    }
    class FileSearcher
    {
        static void Main(String[] args)
        {
            //PrintFiles("C:\\Users\\roflm\\Desktop\\Carpeta");
            //Console.WriteLine("hola");
            FilesWithRegex("(abc+(abc+a#)*)*","lol");
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
                default: return 0;  
            }
        }

        static void FilesWithRegex(string regex, string directory)
        {
            Piece automatas = ParseReg(regex);
            //Console.WriteLine(automatas.GetSpawningAutomata().Count);
            PrintAutomata(automatas,0);
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

        static Piece ParseReg(string regex)
        {
            Piece mainPiece = new Piece();
            mainPiece.SetAtomic(false);
            Piece currentPiece = new Piece();
            currentPiece.SetAtomic(false);                                                             
            
            for (int i = 0; i < regex.Length; i++)
            {
                if(regex[i] == '(')
                {
                    string newRegex = "";
                    int ptheses = 1;

                    while(ptheses!=0)
                    {
                        i++;
                        if(regex[i]=='(')
                            ptheses++;
                        else if(regex[i]==')')
                            ptheses--;
                        newRegex+=regex[i];
                    }

                    int u = GetUnary(i,regex); 
                    if(u>0) i++;
                    newRegex = newRegex.Substring(0,newRegex.Length-1);
                    Piece newPiece = ParseReg(newRegex);
                    newPiece.SetUnary(u);
                    currentPiece.AddPiece(newPiece);
                    //currentPiece = ParseReg(newRegex); 
                    //currentPiece.SetUnary(u);
                    
                }
                else
                {
                    int binary = GetBinary(i,regex);
                    if(binary == 1)
                    {
                        mainPiece.AddPiece(currentPiece);
                        currentPiece = new Piece();
                        currentPiece.SetAtomic(false);
                        continue;
                    }
                    int unary = GetUnary(i, regex);
                    Piece p = new Piece(regex[i].ToString(), unary);
                    currentPiece.AddPiece(p);
                    if(unary>0) i++;
                }
                
            }
            mainPiece.AddPiece(currentPiece);
            //List<Piece> pieces = mainPiece.GetSpawningAutomata();

            return mainPiece;
        }

        static void PrintAutomata(Piece piece, int tabs)
        {   
            PrintTabs(tabs);
            Console.WriteLine("Automata " + piece.GetUnary()+":");
            foreach(Piece p in piece.GetSpawningAutomata())
            {
                if(p.IsAtomic())
                {
                    PrintTabs(tabs);
                    Console.WriteLine(p.ToString()+" "+p.GetUnary());
                }
                    
                else
                    PrintAutomata(p, tabs+1);
            }
        }

        static void PrintTabs(int tabs)
        {
            for (int i = 0; i < tabs; i++)
            {
                Console.Write("   ");
            }
        }

        /*static List<List<Piece>> ParseRegex(string regex)
        {
            List<List<Piece>> automatas = new List<List<Piece>>();
            //while loop
            List<Piece> currentAutomata = new List<Piece>();

            for (int i = 0; i < regex.Length; i++)
            {
                if(regex[i] == '(')
                {
                    //when you find a parenthesis, you make a piece that is not atomic and leads to another automata
                }
                int binary = GetBinary(i,regex);
                if(binary == 1) //means we found union
                {
                    automatas.Add(currentAutomata);
                    currentAutomata = new List<Piece>();
                    i++;
                }
                int unary = GetUnary(i, regex);
                Piece p = new Piece(regex[i].ToString(), unary);
                currentAutomata.Add(p);
                if(unary>0) i++;
            }

            automatas.Add(currentAutomata);
            return automatas;
        }*/

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
    

/*Piece newPiece = new Piece();
                    newPiece.SetUnary(u);
                    
                    Console.WriteLine("The new regex is: " + newRegex);
                    Piece toInsert = ParseReg(newRegex);
                    newPiece.AddPiece(toInsert);
                    currentPiece.AddPiece(newPiece);
                    //recursively call ParseReg, check for any lock
                    //when you find a parenthesis, you make a piece that is not atomic and leads to another automata*/