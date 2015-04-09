using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Wolfram.NETLink;

using IG.Num;

namespace IG.Lib
{




    /// <summary>Interface with Mathematica.</summary>
    public class MathematicaInterface: ILockable
    {

        #region Construction

        /// <summary>Constructs a new Mathematica interface object, without initializing a Mathematica link.
        /// <para>Link is initialized the first time it is used.</para></summary>
        public MathematicaInterface()
        {
            Init();
        }


        /// <summary>Constructs a new Mathematica interface object, initialized with the specified
        /// Mathematica link.</summary>
        /// <param name="ml">The Mathematica link which is assigned to the object for communication with Mathematica.
        /// If null then no link is assigned and a link will be created the first time it is used.</param>
        public MathematicaInterface(IKernelLink ml)
        {
            Init(ml);
        }


        /// <summary>Constructs a new Mathematica interface object, initialized with the specified
        /// Mathematica link.</summary>
        /// <param name="kernel">The Mathematica kernel which is assigned to the object for communication with Mathematica.</param>
        public MathematicaInterface(MathKernel kernel)
        {
            Init(kernel);
        }

        /// <summary>Initializes the current Mathematica interface object.</summary>
        protected virtual void Init()
        {
            MathKernel = null;
            // MathLink = null;
            //Init((IKernelLink) null);
        }

        /// <summary>Initializes the current Mathematica interface object.
        /// <para>This sets the mathematica link if the specified link (argument <paramref name="ml"/>) is not null.</para></summary>
        /// <param name="ml">The Mathematica link which is assigned to the object for communication with Mathematica.</param>
        protected virtual void Init(IKernelLink ml)
        {
            this.MathLink = ml;         
        }

        /// <summary>Initializes the current Mathematica interface object.
        /// <para>This sets the mathematica kernel if the specified kernel (argument <paramref name="ml"/>) is not null.</para></summary>
        /// <param name="kernel">The Mathematica kernel which is assigned to the object for communication with Mathematica.</param>
        public virtual void Init(MathKernel kernel)
        {
            this.MathKernel = kernel;
        }

        #endregion Construction


        #region ILockable

        private readonly object _lock = new object();

        /// <summary>Object's lock</summary>
        public object Lock
        {
            get { return _lock; }
        }

        #endregion ILockable

        #region Global

        protected static MathematicaInterface _global;

        /// <summary>Gets the global Mathematica interface.</summary>
        public static MathematicaInterface Global
        {
            get {
                if (_global == null)
                {
                    lock (Util.LockGlobal)
                    {
                        if (_global == null)
                            _global = new MathematicaInterface();
                    }
                }
                return _global;
            }
        }

        #endregion Global


        #region MathLink


        // This launches the Mathematica kernel:
        protected IKernelLink _mathLink;

        /// <summary>Mathematica's link object.
        /// <para>Getter always returns a valid link object, creating a new one if not yet created.</para></summary>
        public IKernelLink MathLink
        {
            get
            {
                lock (Lock)
                {
                    if (_mathLink == null)
                    {
                        if (MathKernel != null)
                            _mathLink = MathKernel.Link;
                        else
                            _mathLink = MathLinkFactory.CreateKernelLink();
                        // Discard the initial InputNamePacket the kernel will send when launched.
                        _mathLink.WaitAndDiscardAnswer();
                    }
                    return _mathLink;
                }
            }
            protected set
            {
                lock (Lock)
                {
                    if (value != null)
                    {
                        if (_mathKernel != null)
                        {
                            if (value != _mathKernel.Link)
                            {
                                _mathKernel = null;
                            }
                        }
                    }
                    _mathLink = value;
                }
            }
        }


        protected Wolfram.NETLink.MathKernel _mathKernel;

        /// <summary>Mathematica kernel.</summary>
        public MathKernel MathKernel
        {
            get
            {
                lock (Lock)
                {
                    if (_mathKernel == null)
                    {
                        if (_mathLink != null)
                        {
                            // Wolfram.NETLink.IKernelLink ml = Wolfram.NETLink.MathLinkFactory.CreateKernelLink(args);
                            this._mathKernel = new MathKernel(_mathLink);
                        }
                        else 
                        {
                            this._mathKernel = new MathKernel();
                        }

                        // Adjust MathKernel properties:
                        this._mathKernel.AutoCloseLink = true;
                        this._mathKernel.CaptureGraphics = true;
                        this._mathKernel.CaptureMessages = true;
                        this._mathKernel.CapturePrint = true;
                        this._mathKernel.GraphicsFormat = "Automatic";
                        this._mathKernel.GraphicsHeight = 0;
                        this._mathKernel.GraphicsResolution = 0;
                        this._mathKernel.GraphicsWidth = 0;
                        this._mathKernel.HandleEvents = true;
                        this._mathKernel.Input = null;
                        this._mathKernel.LinkArguments = null;
                        this._mathKernel.PageWidth = 60;
                        this._mathKernel.ResultFormat = Wolfram.NETLink.MathKernel.ResultFormatType.OutputForm;
                        this._mathKernel.UseFrontEnd = true;

                    }
                    return _mathKernel;
                }
            }
            protected set
            {
                lock (Lock)
                {
                    _mathKernel = value;
                    if (value != null)
                    {
                        MathLink = value.Link;
                    }
                }
            }
        }


        #endregion MathLink


        #region Data


        #endregion Data

        #region Operation

        /// <summary>Evaluates mathematica function that takes one numerical argument and returns a number.
        /// <para>Example: double res = EvaluateDoubleFunction("Sin",0.5);</para></summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="functionArgument">Argument of function.</param>
        /// <returns>Function value.</returns>
        public double EvaluateScalarFunction(string functionName, double functionArgument)
        {
            Expr functionSymbol = new Expr(ExpressionType.Symbol, functionName);
            Expr expr = new Expr(functionSymbol, functionArgument);
            IKernelLink link = MathLink;
            link.Evaluate(expr);
            link.WaitForAnswer();
            double result = link.GetDouble();
            return result;
        }

        
        /// <summary>Evaluates mathematica function that takes one array argument and returns a number.
        /// <para>Example: double res = EvaluateDoubleFunction("Plus",0.5,0.2);</para></summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="functionArguments">Arguments of function.</param>
        /// <returns>Function value.</returns>
        public double EvaluateScalarFunction(string functionName, double[] functionArguments)
        {
            Expr functionSymbol = new Expr(ExpressionType.Symbol, functionName);
            Expr expr = new Expr(functionSymbol, functionArguments);
            IKernelLink link = MathLink;
            link.Evaluate(expr);
            link.WaitForAnswer();
            double result = link.GetDouble();
            return result;
        }
        
        /// <summary>Evaluates mathematica function that takes one array argument and returns a number.
        /// <para>Example: double res = EvaluateDoubleFunction("Plus",0.5,0.2);</para></summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="functionArguments">Arguments of function.</param>
        /// <returns>Function value.</returns>
        public double EvaluateScalarFunction(string functionName, IVector functionArguments)
        {
            Expr functionSymbol = new Expr(ExpressionType.Symbol, functionName);
            Expr expr = new Expr(functionSymbol, functionArguments.ToArray());
            IKernelLink link = MathLink;
            link.Evaluate(expr);
            link.WaitForAnswer();
            double result = link.GetDouble();
            return result;
        }




        /// <summary>Evaluates the specified expression in Mathematica and returns the result in output form 
        /// as string.</summary>
        /// <param name="expression">Mathematical expression to be evaluated.</param>
        /// <returns>Result of evaluation in string form.</returns>
        public string EvaluateExpression(string expression)
        {
            string result = MathLink.EvaluateToInputForm(expression, 0);
            return result;
        }


        /// <summary>Evaluates an integer-valued expression in Mathematica and returns its value.</summary>
        /// <param name="expression">Integer-valued expression to be evaluated.</param>
        /// <returns>V of the expression.</returns>
        public int EvaluateIntegerExpression(string expression)
        {
            IKernelLink link = MathLink;
            link.Evaluate(expression);
            link.WaitForAnswer();
            int result = link.GetInteger();
            return result;
        }

        /// <summary>Evaluates an real (double)-valued expression in Mathematica and returns its value.</summary>
        /// <param name="expression">Expression to be evaluated.</param>
        /// <returns>Value of the expression.</returns>
        public double EvaluateDoubleExpression(string expression)
        {
            IKernelLink link = MathLink;
            link.Evaluate(expression);
            link.WaitForAnswer();
            double result = link.GetDouble();
            return result;
        }

        #endregion Operation

        #region Examples


        
        /// <summary>Example use of Mathematica interface - a simple calculator.
        /// <para>Expressions are evaluated to rounded numerical values when possible
        /// (i.e. they are wrapped in 'N[...]' before sent to evaluation.</para></summary>
        public static void ExampleCalculator()
        {
            ExampleCalculator(true /* wrapNumerical */);
        }

        /// <summary>Example use of Mathematica interface - a simple calculator.</summary>
        /// <param name="wrapNumerical">Specifies whether expressions should return rounded numerical values when possible
        /// (i.e. they are wrapped in 'N[...]' before sent to evaluation).</param>
        public static void ExampleCalculator(bool wrapNumerical)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine +
                "Evaluation of expressions in Mathematica." + Environment.NewLine + Environment.NewLine
                + "Input Expressions to be evaluated, press <Enter> to finish!" + Environment.NewLine);

            bool endEvaluation = false;
            while (!endEvaluation)
            {
                try
                {
                    string expression = null;
                    Console.Write(Environment.NewLine + "MCalc> ");
                    UtilConsole.Read(ref expression);
                    if (!string.IsNullOrEmpty(expression))
                    {
                        if (wrapNumerical)
                            expression = "N[" + expression + "]";
                        string result = Global.EvaluateExpression(expression);
                        Console.WriteLine(" = " + result);
                    }
                    else
                    {
                        Console.WriteLine(Environment.NewLine + Environment.NewLine + "Are you sure you want to exit (0/1)? ");
                        UtilConsole.Read(ref endEvaluation);
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + "**** ERROR: **** " + Environment.NewLine
                        + ex.Message + Environment.NewLine + Environment.NewLine);
                }
            }
        }


        #endregion Examples


    }  // class MathInt


}
