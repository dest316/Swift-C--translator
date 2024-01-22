using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedSimpleLexer
{
    public abstract class Node 
    {
        public abstract string getGeneratedText();
        public abstract TYPE? getType();

    }
    public class NodeProgramm : Node
    {
        private List<Node> children;
        public NodeProgramm(List<Node> children)
        {
            this.children = children;
        }

        public override string ToString()
        {
            string str = "";
            foreach (var item in children)
            {
                str += item.ToString() + "\n";
            }
            return str;
        }

        public override string getGeneratedText()
        {
            string str = "";
            foreach (var item in children)
            {
                if (item != null)
                    str += item.getGeneratedText() + "\n";
            }
            return str;
        }
        public override TYPE? getType()
        {
            return null;
        }
    }

    public class NodeBlock : NodeProgramm
    {
        public NodeBlock(List<Node> statements) : base(statements)
        {
        }
    }

    public class NodeDeclaration : Node
    {
        private Token type;
        private Token id;

        public NodeDeclaration(Token type, Token id)
        {
            this.type = type;
            this.id = id;
        }

        public override string ToString()
        {
            return $"DECLARATION <{this.type}, {this.id}>";
        }

        public override string getGeneratedText()
        {
            return type.lexem + " " + id.lexem + ";";
        }
        public override TYPE? getType()
        {
            return null;
        }
    }

    public class NodeAssigning : Node
    {
        private Token id;
        private Node expression;
        private Token idOfType;

        public NodeAssigning(Token id, Node expression, Token idOfType = null)
        {
            this.id = id;
            this.expression = expression;
            this.idOfType = idOfType;
        }

        public override string ToString()
        {
            string strIdOfType = (idOfType == null) ? "" : " " + idOfType.ToString();
            return $"ASSIGNING <{this.id},{strIdOfType} {this.expression}>";
        }

        public override string getGeneratedText()
        {
            return new StringBuilder()
                        .Append(id.lexem)
                        .Append(" ")
                        .Append((idOfType == null) ? "" : idOfType.lexem + " ")
                        .Append("= ")
                        .Append(expression.getGeneratedText() + ";")
                        .ToString();
        }
        public override TYPE? getType()
        {
            return null;
        }
    }

    public class NodeLiteral : Node
    {
        private Token value;

        public NodeLiteral(Token value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override string getGeneratedText()
        {
            return value.lexem;
        }
        public override TYPE? getType()
        {
            return value.type;
        }
    }

    public class NodeIntLiteral : NodeLiteral
    {
        public NodeIntLiteral(Token value) : base(value)
        {
        }
    }
    public class NodeFloatLiteral : NodeLiteral
    {
        public NodeFloatLiteral(Token value) : base(value)
        {
        }
    }
    public class NodeBoolLiteral : NodeLiteral
    {
        public NodeBoolLiteral(Token value) : base(value)
        {
        }
    }

    public class NodeVar : Node
    {
        private Token value;

        public NodeVar(Token value)
        {
            this.value = value;
        }
        public override string ToString()
        {
            return value.ToString();
        }
        public override string getGeneratedText()
        {
            return value.lexem;
        }
        public override TYPE? getType()
        {
            return value.type;
        }
    }

    public class NodeMultiply : Node
    {
        private Node left;
        private Node factor;
        public NodeMultiply(Node left, Node factor)
        {
            this.left = left;
            this.factor = factor;
        }

        public override string ToString()
        {
            return $"MULTIPLY <{this.left}, {this.factor}>";
        }

        public override string getGeneratedText()
        {
            return new StringBuilder()
                        .Append(left.getGeneratedText())
                        .Append(" * ")
                        .Append(factor.getGeneratedText())
                        .ToString();
        }
        public override TYPE? getType()
        {
            return (left.getType() == factor.getType()) ? left.getType() : throw new Exception("exeption in *");
        }
    }

    public class NodeDivision : Node
    {
        private Node left;
        private Node factor;
        public NodeDivision(Node left, Node factor)
        {
            this.left = left;
            this.factor = factor;
        }

        public override string ToString()
        {
            return $"DIVISION <{this.left}, {this.factor}>";
        }

        public override string getGeneratedText()
        {
            return new StringBuilder()
                        .Append(left.getGeneratedText())
                        .Append(" / ")
                        .Append(factor.getGeneratedText())
                        .ToString();
        }
        public override TYPE? getType()
        {
            return (left.getType() == factor.getType()) ? left.getType() : throw new Exception("exeption in /");
        }
    }

    public class NodeReturnStatement : Node
    {
        private Node node;
        public NodeReturnStatement(Node node)
        {
            this.node = node;
        }

        public override string ToString()
        {
            return $"RETURN <{this.node}>";
        }

        public override string getGeneratedText()
        {
            return $"return {node.getGeneratedText()};";
        }
        public override TYPE? getType()
        {
            return null;
        }
    }


    public class NodePlus : Node
    {
        private Node left;
        private Node factor;
        public NodePlus(Node left, Node factor)
        {
            this.left = left;
            this.factor = factor;
        }

        public override string ToString()
        {
            return $"PLUS <{this.left}, {this.factor}>";
        }

        public override string getGeneratedText()
        {
            return new StringBuilder()
                        .Append(left.getGeneratedText())
                        .Append(" + ")
                        .Append(factor.getGeneratedText())
                        .ToString();
        }
        public override TYPE? getType()
        {
            return (left.getType() == factor.getType()) ? left.getType() : throw new Exception("exeption in +");
        }
    }

    public class NodeMinus : Node
    {
        private Node left;
        private Node factor;
        public NodeMinus(Node left, Node factor)
        {
            this.left = left;
            this.factor = factor;
        }

        public override string ToString()
        {
            return $"MINUS <{this.left}, {this.factor}>";
        }

        public override string getGeneratedText()
        {
            return new StringBuilder()
                        .Append(left.getGeneratedText())
                        .Append(" - ")
                        .Append(factor.getGeneratedText())
                        .ToString();
        }
        public override TYPE? getType()
        {
            return (left.getType() == factor.getType()) ? left.getType() : throw new Exception("exeption in -");
        }
    }

    public class NodeUnaryOperator: Node
    {
        private Node operand;
        public NodeUnaryOperator(Node operand)
        {
            this.operand = operand;
        }
        public override string ToString()
        {
            return new StringBuilder()
                        .Append(this.GetType())
                        .Append($"<{operand.ToString()}>")
                        .ToString();
        }
        public override string getGeneratedText()
        {
            return new StringBuilder()
                        .Append((this.Equals(typeof(NodeUnaryMinus))) ? "-" : "!")
                        .Append(operand.getGeneratedText())
                        .ToString();
        }
        public override TYPE? getType()
        {
            return null;
        }
    }

    public class NodeUnaryMinus: NodeUnaryOperator
    {
        public NodeUnaryMinus(Node operand) : base(operand)
        {

        }
    }
    public class NodeNot: NodeUnaryOperator
    {
        public NodeNot(Node operand) : base(operand)
        {

        }
    }


    public class NodeBinaryOperator: Node
    {
        string op;
        Node left;
        Node right;
        public NodeBinaryOperator(Node left, Node right, string op = "")
        {
            this.op = op;
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            return new StringBuilder()
                        .Append(this.GetType())
                        .Append($"<{left.ToString()}, {right.ToString()}>")
                        .ToString();
        }

        public override string getGeneratedText()
        {
            return $"{left.getGeneratedText()} {op} {right.getGeneratedText()}";
        }
        public override TYPE? getType()
        {
            return (left.getType() == right.getType()) ? left.getType() : throw new Exception($"exeption in {op}");
        }
    }

    public class NodeL : NodeBinaryOperator
    {
        public NodeL (Node left, Node right) : base (left, right, "<")
        {
        }
    }

    public class NodeG : NodeBinaryOperator
    {
        public NodeG(Node left, Node right) : base(left, right, ">")
        {
        }
    }

    public class NodeLE : NodeBinaryOperator
    {
        public NodeLE(Node left, Node right) : base(left, right, "<=")
        {
        }
    }

    public class NodeGE : NodeBinaryOperator
    {
        public NodeGE(Node left, Node right) : base(left, right, ">=")
        {
        }
    }

    public class NodeEQ : NodeBinaryOperator
    {
        public NodeEQ(Node left, Node right) : base(left, right, "==")
        {
        }
    }
    public class NodeNEQ : NodeBinaryOperator
    {
        public NodeNEQ(Node left, Node right) : base(left, right, "!=")
        {
        }
    }

    public class NodeAnd : NodeBinaryOperator
    {
        public NodeAnd(Node left, Node right) : base(left, right, "&&")
        {
        }
    }

    public class NodeOr : NodeBinaryOperator
    {
        public NodeOr(Node left, Node right) : base(left, right, "||")
        {
        }
    }

    public class NodeIfConstruction : Node
    {
        private Node condition;
        private NodeBlock block;
        private NodeBlock elseBlock;
        
        public NodeIfConstruction(Node condition, NodeBlock block, NodeBlock elseBlock)
        {
            this.condition = condition;
            this.block = block;
            this.elseBlock = elseBlock;
        }

        public override string ToString()
        {
            return new StringBuilder()
                        .Append("IF-CONSTR:")
                        .Append($"CONDITION <{this.condition}>")
                        .Append($"BLOCK <{this.block}>")
                        .Append($"ELSE-BLOCK <{this.elseBlock}>")
                        .ToString();
        }

        public override string getGeneratedText()
        {
            return new StringBuilder()
                        .Append($"if ({condition.getGeneratedText()})")
                        .Append($"\n{{\n\t{block.getGeneratedText()}}}\n")
                        .Append((elseBlock == null) ? "" : $"else\n{{\n\t{elseBlock.getGeneratedText()}}}\n")
                        .ToString();
        }
        public override TYPE? getType()
        {
            return null;
        }
    }
}
