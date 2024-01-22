using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ImprovedSimpleLexer
{
    public sealed class CalcLexer
    {
        private bool eof;
        internal string text;
        private int pos;
        private string accum;
        private string curChar;
        internal int col;
        internal int line;

        private readonly Dictionary<string, TYPE> types = new Dictionary<string, TYPE>()
        {
            {"(", TYPE.LBR}, {")", TYPE.RBR}, {"+", TYPE.PLUS}, {"-", TYPE.MINUS}, {"*", TYPE.MUL}, {"/", TYPE.DIV},
            {"[", TYPE.LSBR}, {"]", TYPE.RSBR}, {"namespace", TYPE.NAMESPACE}, {";", TYPE.SEMICOLON},
            {"for", TYPE.FOR}, {"do", TYPE.DO}, {"while", TYPE.WHILE}, {"public", TYPE.PUBLIC}, {"private", TYPE.PRIVATE}, {"{", TYPE.LFBR},
            {"}", TYPE.RFBR}, {"=", TYPE.ASSIGN}, {"class", TYPE.CLASS}, {"static", TYPE.STATIC}, {"if", TYPE.IF}, {"new", TYPE.NEW},
            {"void", TYPE.VOID}, {"<", TYPE.LESS}, {">", TYPE.GREATER}, {"<=", TYPE.LESS_EQUAL}, {">=", TYPE.GREATER_EQUAL}, {"==", TYPE.EQUAL},
            {"!=", TYPE.NOT_EQUAL}, {"!", TYPE.NOT}, {"+=", TYPE.PLUS_SUM}, {"-=", TYPE.MINUS_SUM}, {"*=", TYPE.MUL_SUM}, {"/=", TYPE.DIV_SUM},
            {"++", TYPE.PLUS_PLUS}, {"--", TYPE.MINUS_MINUS}, {"else", TYPE.ELSE}, {"Int", TYPE.INT_TYPE}, {"Float", TYPE.FLOAT_TYPE}, {"return", TYPE.RETURN}, {"&&", TYPE.AND}, {"||", TYPE.OR},
            {"var", TYPE.VAR_TYPE}, {":", TYPE.COLON}, {"Bool", TYPE.BOOL_TYPE}, {"true", TYPE.BOOL}, {"false", TYPE.BOOL}
        };

        private readonly string[] symbolsCanBeMissed = new string[] { " ", "\t", "\r", "\n" };

        public CalcLexer(string text)
        {
            this.eof = false;
            this.text = text;
            this.pos = -1;
            this.accum = "";
            this.col = 0;
            this.line = 0;
            this.getChar();
        }

        public bool EOF()
        {
            return eof;
        }

        private void getChar()
        {
            pos++;
            col++;
            if (pos < text.Length)
                curChar = text.Substring(pos, 1);
            else
                eof = true;
        }

        private bool doesArrayContainsSymbol(string[] arr, string symbol)
        {
            foreach (string elem in arr)
            {
                if (elem == symbol) return true;
            }

            return false;
        }

        public Token getToken()
        {
            accum = "";

            while (doesArrayContainsSymbol(symbolsCanBeMissed, curChar) && !eof)
            {
                if (curChar == "\n")
                {
                    line++;
                    col = 0;
                }
                getChar();
            }

            if (types.ContainsKey(curChar))
            {
                TYPE this_type;
                types.TryGetValue(curChar, out this_type);

                string prevChar = curChar;
                Token new_token;
               
                getChar();

                if (((prevChar == "<" || prevChar == ">" || prevChar == "=" || prevChar == "!" || prevChar == "+") && curChar == "=") ||
                     (prevChar == "+" && curChar == "+") || (prevChar == "-" && curChar == "-"))
                {
                    types.TryGetValue(prevChar + curChar, out this_type);
                    new_token = new Token(this_type, prevChar + curChar);
                    getChar();
                }
                else new_token = new Token(this_type, prevChar);
                return new_token;
            }
            else if (curChar == "!")
            {
                return new Token(TYPE.NOT, "!");
            }
            else if (curChar == "&")
            {
                getChar();
                if (curChar == "&")
                {
                    return new Token(TYPE.AND, "&&");
                }
                throw new WrongOp(col, line);
            }
            else if (curChar == "|")
            {
                getChar();
                if (curChar == "|")
                {
                    return new Token(TYPE.OR, "||");
                }
                throw new WrongOp(col, line);
            }
            else if (Regex.IsMatch(curChar, "^[a-zA-Z]+$"))
            {
                while (!eof && 
                      (Regex.IsMatch(curChar, "^[a-zA-Z]+$")))
                {
                    accum += curChar;
                    getChar();
                }

                if (types.ContainsKey(accum))
                {
                    TYPE this_type;
                    types.TryGetValue(accum, out this_type);
                    Token new_token = new Token(this_type, accum);
                    return new_token;
                }

                while (!eof &&
                      (Regex.IsMatch(curChar, "^[a-zA-Z]$") || Regex.IsMatch(curChar, "^[0-9]$")))
                {
                    accum += curChar;
                    getChar();
                }

                return new Token(TYPE.ID, accum);
            }
            else if (Regex.IsMatch(curChar, "^[0-9]$"))
            {
                int dot_count = 0;
                while (Regex.IsMatch(curChar, "^([0-9]|[.])$") && !eof)
                {
                    if (curChar == ".") dot_count++;
                    accum += curChar;
                    getChar();
                }

                if (dot_count > 1) 
                    { getChar(); throw new WrongNumeric(col, line); }

                if (!Regex.IsMatch(curChar, "^[a-zA-Z]$"))
                    if (dot_count == 0)
                        return new Token(TYPE.INT, accum);
                    else
                        return new Token(TYPE.FLOAT, accum);
                else { getChar(); throw new WrongID(col, line); }

            }
            else 
            { 
                eof = true; 
            }

            return new Token(TYPE.EOF, "EOF");
            //throw new EOF();
        }
    }
}
