using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Calculator
{
    

    //абстракный класс
    abstract class Formula
    {
        protected String _f;
        protected String _answer;
        protected List<Formula> parts;
        protected List<char> act;

        public Formula(String f)
        {
            this._f = f;
            this.parts = new List<Formula>();
            this.act = new List<char>();
            this.Normal();
            if (f[0] != '-') this.act.Add('+');
            this.main();
        }

        //основный расчёты
        abstract protected void main();

        public String getResult()
        {
            return this._answer;
        }

        //метод выбирающий объект какого типа создать
        //если в примере присутсвую + или -, то это HardFormula
        //если в примере присутсвую * или /, то это SimpleFormula
        //если в примере присутсвую ^ , то это LestFormula
        //если в примере присутсвет тлько 1 чило и выражение в скобках то это OneDigitFormula
        virtual protected char choise(String f)
        {
            char kind = 'n';
            char[] PM = { '+', '-' };
            char[] MD = { '*', '/' };
            char[] P = { '^' };
            int pm = this.Checker(f, PM), md = this.Checker(f, MD), p = this.Checker(f, P);
            if (pm >= 1) { kind = 'H'; }
            if (pm == 0 && md >= 1) { kind = 'S'; }
            if (pm == 0 && md == 0 && p > 0) { kind = 'L'; }
            if (pm == 0 && md == 0 && p == 0) { kind = 'O'; }
            switch (kind)
            {
                case 'H':
                    this.parts.Add(new HardFormula(f));
                    break;
                case 'S':
                    this.parts.Add(new SimpleFormula(f));
                    break;
                case 'L':
                    this.parts.Add(new LestFormula(f));
                    break;
                case 'O':
                    this.parts.Add(new OneDigitFormula(f));
                    break;
            }
            return kind;
        }

        //метод вычисляет результат полученного выражения
        protected string summator()
        {
            string answer;
            double sum = 0;
            int j = 0;
            try
            {
                foreach (Formula i in this.parts)
                {
                    char kind = this.act.ElementAt(j);
                    int e = 1, zero = 0;
                    switch (kind)
                    {
                        case '-': sum -= Convert.ToDouble(i.getResult()); break;
                        case '+': sum += Convert.ToDouble(i.getResult()); break;
                        case '*': sum *= Convert.ToDouble(i.getResult()); break;
                        case '/': sum = sum / Convert.ToDouble(i.getResult()); if (Convert.ToDouble(i.getResult()) == 0) e /= zero; break;
                        case '^': sum = Math.Pow(sum, Convert.ToDouble(i.getResult())); break;
                    }
                    j++;
                }
                answer = sum.ToString();
            }
            catch (Exception e)
            {
                if (e.GetType().FullName != "DivideByZeroException")
                    answer = "Math Error";
                else answer = "Syntax Error";
            }
            return answer;
        }

        //считает количество операндов в строке f, из массива g
        protected int Checker(String f, char[] g)
        {
            int result = 0, s = 0;
            for (int i = 0; i < f.Length; i++)
            {
                s = (f[i] == '(') ? s + 1 : s;
                s = (f[i] == ')') ? s - 1 : s;
                for (int j = 0; j < g.Length && s == 0; j++)
                {
                    result = (f[i] == g[j]) ? result + 1 : result;
                }
            }
            return result;
        }

        //нормализует выражение, если оно находится в скобках, то скобки убираются
        protected void Normal()
        {
            while (this._f[0] == '(' && this._f[this._f.Length - 1] == ')')
            {
                String n = "";
                for (int i = 1; i < this._f.Length - 1; i++)
                {
                    n += this._f[i];
                }
                this._f = n;
            }
        }
    }

    //это класс, объекты которого делят полученное выражение на слагаемые
    class HardFormula : Formula
    {

        private char[] separator = { '+', '-' };

        public HardFormula(String f) : base(f) { this._f += "_"; }

        protected override void main()
        {

            int s = 0;
            String parter = "";
            Formula x;
            for (int i = 0; i < this._f.Length; i++)
            {
                if (this._f[i] != '-' && this._f[i] != '+' && this._f[i] != '_')
                {
                    parter += this._f[i];
                    if (this._f[i] == '(') s++;
                    if (this._f[i] == ')') s--;
                }
                else
                {
                    if (s == 0)
                    {
                        this.choise(parter);
                        parter = "";
                        this.act.Add(this._f[i]);
                    }
                    else parter += this._f[i];
                }
            }
            this.choise(parter);
            this._answer = this.summator().ToString();
        }
    }

    //это класс, объекты которого делят полученное выражение на множетели
    class SimpleFormula : Formula
    {
        private char[] separator = { '*', '/' };

        public SimpleFormula(String f) : base(f) { this._f += "_"; }

        protected override void main()
        {
            int s = 0;
            String parter = "";
            for (int i = 0; i < this._f.Length; i++)
            {
                if (this._f[i] != '*' && this._f[i] != '/')
                {
                    parter += this._f[i];
                    if (this._f[i] == '(') s++;
                    if (this._f[i] == ')') s--;
                }
                else
                {
                    if (s == 0)
                    {
                        this.choise(parter);
                        parter = "";
                        this.act.Add(this._f[i]);
                    }
                    else parter += this._f[i];
                }
            }
            this.choise(parter);

            this._answer = this.summator().ToString();
        }
    }

    //это класс, объекты которого возводят в степень
    class LestFormula : Formula
    {
        public LestFormula(string f) : base(f) { }

        protected override void main()
        {
            int s = 0;
            String parter = "";
            for (int i = 0; i < this._f.Length; i++)
            {
                if (this._f[i] != '^')
                {
                    parter += this._f[i];
                    if (this._f[i] == '(') s++;
                    if (this._f[i] == ')') s--;
                }
                else
                {
                    if (s == 0)
                    {
                        this.choise(parter);
                        parter = "";
                        this.act.Add(this._f[i]);
                    }
                    else parter += this._f[i];
                }
            }
            this.choise(parter);

            this._answer = this.summator().ToString();
        }

    }

    //это класс, объекты которого преобразуют число из string в double
    //или если выражение было в собках создаёт новые объекты для вычисления результата
    class OneDigitFormula : Formula
    {
        public OneDigitFormula(String f) : base(f) { }

        protected override void main()
        {
            char kind = this.choise(this._f);
            if (kind != 'O')
            {
                this._answer = this.parts.ElementAt(0).getResult();
            }
        }

        override protected char choise(String f)
        {
            char kind = 'n';
            char[] PM = { '+', '-' };
            char[] MD = { '*', '/' };
            char[] P = { '^' };
            if (this.Checker(f, PM) >= 1) { kind = 'H'; }
            if (this.Checker(f, PM) == 0 && this.Checker(f, MD) >= 1) { kind = 'S'; }
            if (this.Checker(f, PM) == 0 && this.Checker(f, PM) == 0 && this.Checker(f, P) == 0) { kind = 'O'; }
            if (this.Checker(f, PM) == 0 && this.Checker(f, PM) == 0 && this.Checker(f, P) > 0) { kind = 'L'; }
            switch (kind)
            {
                case 'H':
                    this.parts.Add(new HardFormula(f));
                    break;
                case 'S':
                    this.parts.Add(new SimpleFormula(f));
                    break;
                case 'L':
                    this.parts.Add(new LestFormula(f));
                    break;
                case 'O':
                    this._answer = this.R(this._f);
                    break;
            }
            return kind;
        }

        private string R(string f)
        {
            if (f == "pi") { return Math.PI.ToString(); }
            if (f == "e") { return Math.E.ToString(); }
            else { return f; }
        }
    }
}

