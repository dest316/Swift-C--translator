using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedSimpleLexer
{
    public class Parser
    {
        private CalcLexer lexer;
        private Token token;
        private List<SymTable> table;
        private Token prev;
        public Parser(CalcLexer lexer)
        {
            this.lexer = lexer;
            this.token = lexer.getToken();
            this.table = new List<SymTable>();
            this.table.Add(new SymTable());
            prev = new Token(TYPE.NONE,"");
        }

        private void nextToken()
        {
            this.token = lexer.getToken();
        }

        private NodeBlock block()
        {
            List<Node> statements = new List<Node>();
            table.Add(new SymTable());
            while (token.type != TYPE.RFBR && token.type != TYPE.EOF && token.type != TYPE.ELSE)
            {
                statements.Add(statement());
                nextToken();
            }
            return new NodeBlock(statements);
        }

        public Node operand()
        {
            Token firstToken = token;
            if (token.type == TYPE.INT)
            {
                nextToken();
                return new NodeIntLiteral(firstToken);
            }
            else if (token.type == TYPE.FLOAT)
            {
                nextToken();
                return new NodeFloatLiteral(firstToken);
            }
            else if (token.type == TYPE.BOOL)
            {
                nextToken();
                return new NodeBoolLiteral(firstToken);
            }
            else if (token.type == TYPE.ID)
            {
                nextToken();
                return new NodeVar(firstToken);
            }
            else return null;
        }

        public TYPE valueTypeToValue(TYPE type)
        {
            if (type == TYPE.INT_TYPE)
                return TYPE.INT;
            if (type == TYPE.FLOAT_TYPE)
                return TYPE.FLOAT;
            if (type == TYPE.BOOL_TYPE)
                return TYPE.BOOL;
            return TYPE.NONE;
        }
        public Node statement()
        {
            Token firstToken = new Token(TYPE.NONE, "");
            Token secondToken = new Token(TYPE.NONE, "");
            if (prev.type == TYPE.NONE)
                
            switch (token.type)
            {
                case TYPE.ID:
                    firstToken = token;
                    nextToken();
                    if (token.type == TYPE.ID)
                    {
                        throw new ExpectedAssign(lexer.col, lexer.line);
                        //secondToken = token;
                        //nextToken();
                        //return new NodeDeclaration(firstToken, secondToken);
                    }
                    else if (token.type == TYPE.ASSIGN)
                    {
                        //nextToken();
                        //return new NodeAssigning(firstToken, expression());
                        nextToken();
                        Node expression = this.expression();
                        TYPE? test = expression.getType();

                        int flag = -1;
                        for (int i = 0; i < table.Count; i++)
                        {
                            if (table[i].isExist(firstToken.lexem))
                            {
                                flag = i;
                                break;
                            }
                        }
                        //if (!table[table.Count - 1].isExist(firstToken.lexem))
                        if (flag == -1)
                            throw new notDeclared(lexer.col, lexer.line);

                        if (test != table[flag].getTYPE(firstToken.lexem))
                            throw new notComparable(lexer.col, lexer.line);

                        return new NodeAssigning(firstToken, expression);
                    }
                    else if (token.type == TYPE.COLON)
                    {
                        throw new ExpectedVar(lexer.col, lexer.line);
                    }
                    else
                        throw new ExpectedAssign(lexer.col, lexer.line);
                    break;
                case TYPE.VAR_TYPE:
                    firstToken = token;
                    nextToken();
                    if (token.type == TYPE.ID)
                    {
                        secondToken = token;
                        nextToken();

                        for (int i = 0; i < table.Count; i++)
                            if (table[i].isExist(secondToken.lexem))
                                throw new alreadyDeclared(lexer.col, lexer.line);
                        int flag = -1;
                        for (int i = 0; i < table.Count; i++)
                        {
                            if (table[i].isExist(secondToken.lexem))
                            {
                                flag = i;
                                break;
                            }
                        }
                        Token sign = new Token(token.type, token.lexem);
                        nextToken();
                        if (flag == -1)
                        {
                            flag = table.Count - 1;
                            if (token.type == TYPE.INT || token.type == TYPE.FLOAT || token.type == TYPE.BOOL)
                                table[flag].Add(secondToken.lexem, token.type);
                            else 
                                table[flag].Add(secondToken.lexem, valueTypeToValue(token.type));
                            
                        }

                        //if (table[flag].getTYPE(secondToken.lexem) != valueTypeToValue(token.type))
                        //    throw new notComparable(lexer.col, lexer.line);

                        if (sign.type == TYPE.ASSIGN)
                        {
                                //nextToken();
                                //Token prev_token = new Token(token.type,token.lexem);
                            
                            Node expression = this.expression();
                                //token = prev_token;
                            prev = new Token(token.type, token.lexem);
                            TYPE? test = expression.getType();
                            if (test != table[flag].getTYPE(secondToken.lexem))
                                throw new notComparable(lexer.col, lexer.line);
                            
                            return new NodeAssigning(firstToken, expression, secondToken);
                            //return new NodeAssigning(firstToken, expression(), secondToken);
                        }
                        //return new NodeDeclaration(firstToken, secondToken);
                        if (sign.type == TYPE.COLON)
                        {
                            //nextToken();
                            if (token.type == TYPE.INT_TYPE || token.type == TYPE.FLOAT_TYPE || token.type == TYPE.BOOL_TYPE)
                            {
                                firstToken = token;
                                //nextToken();
                                //table[flag].Add(secondToken.lexem, valueTypeToValue(firstToken.type));
                                return new NodeDeclaration(firstToken, secondToken);
                            }

                            throw new ExpectedSupportedType(lexer.col, lexer.line);
                        }
                        throw new ExpectedCOLON(lexer.col, lexer.line);
                    }
                    throw new ExpectedID(lexer.col, lexer.line);
                    break;
                case TYPE.IF:
                    nextToken();
                    if (token.type != TYPE.LBR)
                        throw new ExpectedLBR(lexer.col, lexer.line);
                    else
                        nextToken();
                    Node condition = this.condition();
                    if (token.type != TYPE.RBR)
                        throw new ExpectedRBR(lexer.col, lexer.line);
                    else
                        nextToken();
                    if (token.type == TYPE.LFBR)
                    {
                        nextToken();
                        NodeBlock block = this.block();
                        if (token.type == TYPE.RFBR)
                        {
                            nextToken();
                            if (token.type == TYPE.ELSE)
                            {
                                nextToken();
                                if (token.type == TYPE.LFBR)
                                {
                                    nextToken();
                                    NodeBlock elseBlock = this.block();
                                    if (token.type == TYPE.RFBR)
                                    {
                                        nextToken();
                                        return new NodeIfConstruction(condition, block, elseBlock);
                                    }
                                    else
                                        throw new ExpectedRFBR(lexer.col, lexer.line);
                                }
                                else
                                    throw new ExpectedLFBR(lexer.col, lexer.line);
                            }
                            else
                                return new NodeIfConstruction(condition, block, new NodeBlock(null));
                        }
                        else
                            throw new ExpectedRFBR(lexer.col, lexer.line);
                    }
                    else
                        throw new ExpectedLFBR(lexer.col, lexer.line);
                    break;
                case TYPE.RETURN:
                    nextToken();
                    return new NodeReturnStatement(expression());
                    break;
            }
            else
            {
                Token cur = new Token(prev.type,prev.lexem);
                prev = new Token(TYPE.NONE, "");
                switch (cur.type)
                {
                    case TYPE.ID:
                        firstToken = cur;
                        if (token.type == TYPE.ID)
                        {
                            throw new ExpectedAssign(lexer.col, lexer.line);
                            //secondToken = token;
                            //nextToken();
                            //return new NodeDeclaration(firstToken, secondToken);
                        }
                        else if (token.type == TYPE.ASSIGN)
                        {
                            //nextToken();
                            //return new NodeAssigning(firstToken, expression());
                            nextToken();
                            Node expression = this.expression();
                            TYPE? test = expression.getType();

                            int flag = -1;
                            for (int i = 0; i < table.Count; i++)
                            {
                                if (table[i].isExist(firstToken.lexem))
                                {
                                    flag = i;
                                    break;
                                }
                            }
                            //if (!table[table.Count - 1].isExist(firstToken.lexem))
                            if (flag == -1)
                                throw new notDeclared(lexer.col, lexer.line);

                            if (test != table[flag].getTYPE(firstToken.lexem))
                                throw new notComparable(lexer.col, lexer.line);

                            return new NodeAssigning(firstToken, expression);
                        }
                        else if (token.type == TYPE.COLON)
                        {
                            throw new ExpectedVar(lexer.col, lexer.line);
                        }
                        else
                            throw new ExpectedAssign(lexer.col, lexer.line);
                        break;
                    case TYPE.VAR_TYPE:
                        firstToken = token;
                        if (token.type == TYPE.ID)
                        {
                            secondToken = token;
                            nextToken();

                            for (int i = 0; i < table.Count; i++)
                                if (table[i].isExist(secondToken.lexem))
                                    throw new alreadyDeclared(lexer.col, lexer.line);
                            int flag = -1;
                            for (int i = 0; i < table.Count; i++)
                            {
                                if (table[i].isExist(secondToken.lexem))
                                {
                                    flag = i;
                                    break;
                                }
                            }
                            Token sign = new Token(token.type, token.lexem);
                            nextToken();
                            if (flag == -1)
                            {
                                flag = table.Count - 1;
                                if (token.type == TYPE.INT || token.type == TYPE.FLOAT || token.type == TYPE.BOOL)
                                    table[flag].Add(secondToken.lexem, token.type);
                                else
                                    table[flag].Add(secondToken.lexem, valueTypeToValue(token.type));

                            }

                            //if (table[flag].getTYPE(secondToken.lexem) != valueTypeToValue(token.type))
                            //    throw new notComparable(lexer.col, lexer.line);

                            if (sign.type == TYPE.ASSIGN)
                            {
                                //nextToken();
                                //Token prev_token = new Token(token.type,token.lexem);
                                prev = new Token(token.type,token.lexem);
                                Node expression = this.expression();
                                //token = prev_token;
                                TYPE? test = expression.getType();
                                if (test != table[flag].getTYPE(secondToken.lexem))
                                    throw new notComparable(lexer.col, lexer.line);

                                return new NodeAssigning(firstToken, expression, secondToken);
                                //return new NodeAssigning(firstToken, expression(), secondToken);
                            }
                            //return new NodeDeclaration(firstToken, secondToken);
                            if (sign.type == TYPE.COLON)
                            {
                                //nextToken();
                                if (token.type == TYPE.INT_TYPE || token.type == TYPE.FLOAT_TYPE || token.type == TYPE.BOOL_TYPE)
                                {
                                    firstToken = token;
                                    //nextToken();
                                    //table[flag].Add(secondToken.lexem, valueTypeToValue(firstToken.type));
                                    return new NodeDeclaration(firstToken, secondToken);
                                }

                                throw new ExpectedSupportedType(lexer.col, lexer.line);
                            }
                            throw new ExpectedCOLON(lexer.col, lexer.line);
                        }
                        throw new ExpectedID(lexer.col, lexer.line);
                        break;
                    case TYPE.IF:
                        if (token.type != TYPE.LBR)
                            throw new ExpectedLBR(lexer.col, lexer.line);
                        else
                            nextToken();
                        Node condition = this.condition();
                        if (token.type != TYPE.RBR)
                            throw new ExpectedRBR(lexer.col, lexer.line);
                        else
                            nextToken();
                        if (token.type == TYPE.LFBR)
                        {
                            nextToken();
                            NodeBlock block = this.block();
                            if (token.type == TYPE.RFBR)
                            {
                                nextToken();
                                if (token.type == TYPE.ELSE)
                                {
                                    nextToken();
                                    if (token.type == TYPE.LFBR)
                                    {
                                        nextToken();
                                        NodeBlock elseBlock = this.block();
                                        if (token.type == TYPE.RFBR)
                                        {
                                            nextToken();
                                            return new NodeIfConstruction(condition, block, elseBlock);
                                        }
                                        else
                                            throw new ExpectedRFBR(lexer.col, lexer.line);
                                    }
                                    else
                                        throw new ExpectedLFBR(lexer.col, lexer.line);
                                }
                                else
                                    return new NodeIfConstruction(condition, block, new NodeBlock(null));
                            }
                            else
                                throw new ExpectedRFBR(lexer.col, lexer.line);
                        }
                        else
                            throw new ExpectedLFBR(lexer.col, lexer.line);
                        break;
                    case TYPE.RETURN:
                        return new NodeReturnStatement(expression());
                        break;
                }
            }
            return null;
        }

        public Node factor()
        {
            if (token.type == TYPE.MINUS)
            {
                nextToken();
                return new NodeUnaryMinus(operand());
            }
            else
                return operand();
        }

        public Node term()
        {
            Node left = factor();
            TYPE op = token.type;
            while (op == TYPE.MUL || op == TYPE.DIV) 
            {
                nextToken();
                switch (op)
                {
                    case TYPE.MUL:
                        left = new NodeMultiply(left, factor());
                        break;
                    case TYPE.DIV:
                        left = new NodeDivision(left, factor());
                        break;
                }
                op = token.type;
            }
            return left;
        }

        public Node expression() 
        {
            Node left = term();
            TYPE op = token.type;
            while (op == TYPE.PLUS || op == TYPE.MINUS)
            {
                nextToken();
                switch (op)
                {
                    case TYPE.PLUS:
                        left = new NodePlus(left, term());
                        break;
                    case TYPE.MINUS:
                        left = new NodeMinus(left, term());
                        break;
                }
                op = token.type;
            }
            return left;
        }

        public Node andOperand()
        {
            if (token.type == TYPE.NOT)
            {
                nextToken();
                return new NodeNot(andOperand());
            }
            else
            {
                Node left = expression();
                TYPE op = token.type;
                while (op == TYPE.LESS || op == TYPE.GREATER || op == TYPE.LESS_EQUAL || op == TYPE.GREATER_EQUAL || op == TYPE.EQUAL || op == TYPE.NOT_EQUAL)
                {
                    nextToken();
                    switch (op)
                    {
                        case TYPE.LESS:
                            left = new NodeL(left, expression());
                            break;
                        case TYPE.GREATER:
                            left = new NodeG(left, expression());
                            break;
                        case TYPE.LESS_EQUAL:
                            left = new NodeLE(left, expression());
                            break;
                        case TYPE.GREATER_EQUAL:
                            left = new NodeGE(left, expression());
                            break;
                        case TYPE.EQUAL:
                            left = new NodeEQ(left, expression());
                            break;
                        case TYPE.NOT_EQUAL:
                            left = new NodeNEQ(left, expression());
                            break;
                    }

                    op = token.type;
                }
                return left;
            }
        }

        public Node orOperand()
        {
            Node left = andOperand();
            TYPE op = token.type;
            while (op == TYPE.AND)
            {
                nextToken();
                left = new NodeAnd(left, andOperand());
                op = token.type;
            }
            return left;
        }


        public Node condition()
        {
            Node left = orOperand();
            TYPE op = token.type;
            while (op == TYPE.OR)
            {
                nextToken();
                left = new NodeOr(left, orOperand());
                op = token.type;
            }
            return left;
        }

        public Node parse()
        {
            if (token.type == TYPE.EOF)
            {
                throw new EOF();
            }
            else
            {
                List<Node> statements = new List<Node>();
                while (token.type != TYPE.EOF)
                {
                    statements.Add(statement());
                    if (token.type == TYPE.EOF)
                        break;
                    
                    nextToken();
                }

                return new NodeProgramm(statements);
            }
        }

        public void printTables()
        {
            for (int i = 0; i < table.Count; i++)
            {
                Console.WriteLine(table[i]);
            }
        }

    }
}
