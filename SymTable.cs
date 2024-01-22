using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedSimpleLexer
{
    public class SymTable
    {
        //private Dictionary<string, string> table;
        //public SymTable()
        //{
        //    this.table = new Dictionary<string, string>();
        //}

        private Dictionary<string, TYPE?> table;
        public SymTable()
        {
            this.table = new Dictionary<string, TYPE?>();
        }

        public void Add(string id, TYPE type)
        {
            table.Add(id, type);
        }

        public bool isExist(string id)
        {
            TYPE? tableType = null;
            table.TryGetValue(id, out tableType);
            if (tableType == null)
                return false;
            return true;
        }

        public TYPE? getTYPE(string id)
        {
            TYPE? tableType = null;
            table.TryGetValue(id, out tableType);
            return tableType;
        }
        public override string ToString()
        {
            string dictionaryString = "";
            foreach (KeyValuePair<string, TYPE?> keyValues in table)
            {
                dictionaryString += "name = " + keyValues.Key + " type = " + keyValues.Value + "\n";
            }
            return new StringBuilder()
                        .Append("---------------------------------\n")
                        .Append(dictionaryString)
                        .Append("---------------------------------\n")
                        .ToString();
        }


    }
}
