using System.Collections.Generic;

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

        public List<Piece> GetUnions()
        {
            return this.unions;
        }

        public Piece GetConcatenation()
        {
            return this.concatenation;
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
            
            this.hasContent = true;
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
}