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
        List<Piece> unions;
        bool isAtomic = true;
        Piece concatenation;
        bool hasContent;
        

        public Piece()
        {
            this.unions = new List<Piece>();
        }

        public Piece(string symbol)
        {
            this.symbol = symbol;
            this.unions = new List<Piece>();
            this.hasContent = true;
        }

        public Piece(string symbol, int unary)
        {
            this.symbol = symbol;
            this.unary = unary;
            this.unions = new List<Piece>();
            this.hasContent = true;
        }

        public void AddSymbol(string symbol)
        {
            this.symbol = symbol;
            this.hasContent = true;
        }

        public void SetUnary(int unary)
        {
            this.unary = unary;
        }

        public void SetAtomic(bool isAtomic)
        {
            this.isAtomic = isAtomic;
        }

        public void SetNext(Piece piece)
        {
            this.concatenation = piece;
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
            return this.unions;
        }

        public void AddUnion(Piece piece)
        {
            this.unions.Add(piece);
        }

        public void Concatenate(Piece piece)
        {
            Piece current = this;

            while(current.concatenation!=null)
            {
                current = current.concatenation;
            }

            current.concatenation = piece;
        }

        public bool HasContent()
        {
            return this.hasContent;
        }

        public string GetSymbol()
        {
            return this.symbol;
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

        public int GetNumericUnary()
        {
            return this.unary;
        }


    }
    class FileSearcher
    {
        static void Main(String[] args)
        {
            //PrintFiles("C:\\Users\\roflm\\Desktop\\Carpeta");
            //Console.WriteLine("hola");
            FilesWithRegex("ac+b","lol");
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
            //PrintAutomata(automatas,0);
        }

        static void Print(Piece automata)
        {
            foreach(Piece p in automata.GetSpawningAutomata())
            {
                
            }
        }
        static Piece ParseReg(string regex)
        {
            Piece mainPiece = new Piece();
            //mainPiece.SetAtomic(false);
            Piece currentPiece = new Piece();
            //currentPiece.SetAtomic(false);                                                             
            
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
                    //Console.WriteLine(newRegex);
                    Piece newPiece = ParseReg(newRegex);
                    newPiece.SetUnary(u);
                    currentPiece.AddUnion(newPiece);
                    //currentPiece = ParseReg(newRegex); 
                    //currentPiece.SetUnary(u);
                    
                }
                else
                {
                    int binary = GetBinary(i,regex);
                    if(binary == 1) //create different piece
                    {
                        mainPiece.AddUnion(currentPiece);
                        currentPiece = new Piece();
                        //currentPiece.SetAtomic(false);
                        continue;
                    }
                    int unary = GetUnary(i, regex);
                    Piece p = new Piece(regex[i].ToString(), unary);
                    if(!currentPiece.HasContent()){
                        currentPiece = p;
                    }
                    else
                    {
                        currentPiece.Concatenate(p);
                    }
                    
                    if(unary>0) i++;
                }
                
            }
            mainPiece.AddUnion(currentPiece);
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
                Console.Write(" ");
            }
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