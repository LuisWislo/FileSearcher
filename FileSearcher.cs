using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSearcher
{
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
            Console.WriteLine(automatas.GetUnions().Count);
            Print(automatas);
        }

        static void Print(Piece automata)
        {
            if(automata!=null){
                foreach(Piece p in automata.GetUnions())
                {
                    Console.WriteLine("Next automata's unions: " + p.GetUnions().Count);
                    Print(p);
                }

                if(automata.GetSymbol() != null || automata.GetSymbol() != "")
                {
                    
                    Console.WriteLine(automata.ToString()+" "+automata.GetUnary());
                }
                
                Print(automata.GetConcatenation());
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

                    if(!currentPiece.HasContent()){
                        currentPiece = newPiece;
                    }
                    else
                    {
                        currentPiece.Concatenate(newPiece);
                    }

                    //currentPiece.AddUnion(newPiece); //i think this was actually wrong
                    
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
            return mainPiece;
        }

        static void PrintAutomata(Piece piece, int tabs)
        {   
            PrintTabs(tabs);
            Console.WriteLine("Automata " + piece.GetUnary()+":");
            foreach(Piece p in piece.GetUnions())
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