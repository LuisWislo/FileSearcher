using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSearcher
{
    class FileSearcher
    {
        private int currentCharG=-1;

        public FileSearcher(string regex, string directory)
        {
            FilesWithRegex(regex,directory);
        }
        static void Main(String[] args)
        {
            //PrintFiles("C:\\Users\\roflm\\Desktop\\Carpeta");
            //Console.WriteLine("hola");
            new FileSearcher("a*(bc)","lol"); 
        }
        
        int GetUnary(int current, string regex) //supposed to only work with chracters
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

        private int GetBinary(int current, string regex)
        {
            switch(regex[current])
            {
                case '+': return 1; //we have union
                default: return 0;  
            }
        }

        void FilesWithRegex(string regex, string directory)
        {
            Piece automatas = ParseReg(regex);
            //Print(automatas,1);
            Console.WriteLine(EvaluateTitle(automatas, "bcabpepe", 0));
            //should be done with every substring bcabedario
        }

        void Print(Piece automata, int tabs)
        {
            if(automata!=null)
            {
                Console.WriteLine("Automata's unions: " + automata.GetUnions().Count + " " + automata.GetUnary());
                foreach(Piece p in automata.GetUnions())
                {   
                    //PrintTabs(tabs);
                    Print(p,tabs+1);
                }

                if(automata.GetSymbol() != null)
                {
                    //PrintTabs(tabs);
                    Console.WriteLine(automata.ToString()+" "+automata.GetUnary());
                }
                
                Print(automata.GetConcatenation(),tabs+1);
            }
        }

        Piece ParseReg(string regex)
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

        bool EvaluateTitle(Piece automata, string title, int currentChar)
        { //may need a unary attribute
            if(automata!=null)
            {
                if(automata.GetNumericUnary()==2) //kleene spotted
                { //may not be xactly as the one below, since this one is kleene and accepts 0 ocurrences
                    
                    if(automata.GetSymbol() != null)
                    {
                        bool comparison = CompareSymbol(title[currentChar].ToString(),automata.GetSymbol());
                        if(comparison)
                        {
                            currentCharG = currentChar;
                        }

                        while(comparison)
                        {
                            comparison = CompareSymbol(title[++currentChar].ToString(),automata.GetSymbol());
                            if(!comparison) currentChar--;
                            else currentCharG = currentChar;
                            
                        }

                        //currentCharG = currentChar;

                        return EvaluateTitle(automata.GetConcatenation(),title,currentChar+1); 
                    }

                    else
                    {
                        //remove kleene attribute
                        automata.SetUnary(0);
                        bool comparison = EvaluateTitle(automata,title,currentChar);
                        while(comparison)
                        { //use global variable?
                            comparison = EvaluateTitle(automata,title,currentCharG+1);
                        }

                        return EvaluateTitle(automata.GetConcatenation(),title,currentCharG+1);
                    }
                    
                    //EvaluateTitle(automata, title, currentChar);
                    //if it doesnt have unions, then use symbol only
                    //if it has unions, 
                   
                    //while()
                }
                if(automata.GetUnions().Count>0)
                {
                    bool goToConcats = false;

                    foreach(Piece p in automata.GetUnions())
                    {
                        bool matchesUnion = EvaluateTitle(p,title,currentChar); //use normal variables here
                        if(matchesUnion) goToConcats = true;
                    }

                    if(!goToConcats) return false;
                    return EvaluateTitle(automata.GetConcatenation(),title,currentCharG+1); //problem with current character, when connecting a container to another
                    //use global varaible here
                }
                
                else
                {
                    if(automata.GetSymbol() != null)
                    {
                        //check if kleene or positive
                        bool comparison = CompareSymbol(title[currentChar].ToString(), automata.GetSymbol());
                        if(!comparison) return false;
                        currentCharG = currentChar;
                    }
                    
                    return EvaluateTitle(automata.GetConcatenation(), title, currentChar+1);
                    //else
                    //EvaluateTitle(sameAutomata)
                }
            }
            else
            {
                return true;
            }

            //return false;
        }

        private bool CompareSymbol(string symbolA, string symbolB)
        {

            return symbolA == symbolB;
        }


        void PrintAutomata(Piece piece, int tabs)
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

        void PrintTabs(int tabs)
        {
            for (int i = 0; i < tabs; i++)
            {
                Console.Write(" ");
            }
        }

        void PrintFiles(string directory)
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