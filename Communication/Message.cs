using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{

    public interface Message
    {
        string ToString();
    }

    [Serializable]
    public class Expr : Message
    {
        private double op1, op2;
        private char op;

        public Expr(double op1, double op2, char op)
        {
            this.op1 = op1;
            this.op2 = op2;
            this.op = op;
        }


        public double Op1
        {
            get { return op1; }
        }

        public double Op2
        {
            get { return op2; }
        }

        public char Op
        {
            get { return op; }
        }

        public override string ToString()
        {
            return op1 + " " + op + " " + op2;
        }

    }

    [Serializable]
    public class Result : Message
    {
        private double val;
        private bool error;

        public Result(double val, bool err)
        {
            this.val = val;
            error = err;
        }

        public double Val
        {
            get { return val; }
        }

        public bool Error
        {
            get { return error; }
        }

        public override string ToString()
        {
            return val + " ";
        }

    }
}
