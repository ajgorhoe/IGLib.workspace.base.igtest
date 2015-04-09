using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Wolfram.NETLink;

using IG.Lib;
using IG.Num;

namespace IG.Forms
{
    public partial class MathematicaCalculatorControl : UserControl
    {
        public MathematicaCalculatorControl()
        {
            InitializeComponent();
            btnEvaluate.Text = ButtonTextEvaluate;
        }

        #region Resources

        public const string ButtonTextEvaluate = "Evaluate";

        public const string ButtonTextAbort = "Abort";

        public const string DefaultImageName = "imgGraphics.Image";

        System.ComponentModel.ComponentResourceManager _resources;
        //= new
        //    System.ComponentModel.ComponentResourceManager(typeof(MathematicaCalculatorControl));

        /// <summary>Resource manager for the current control.</summary>
        System.ComponentModel.ComponentResourceManager Resources
        {
            get
            {
                if (_resources == null)
                    _resources = new
            System.ComponentModel.ComponentResourceManager(typeof(MathematicaCalculatorControl));
                return _resources;
            }
        }

        System.Drawing.Image _defaultImage;

        /// <summary>Default image to be displayed when kernel does not generate any.</summary>
        public System.Drawing.Image DefaultImage
        {
            get
            {
                if (_defaultImage == null)
                    _defaultImage = ((System.Drawing.Image)(Resources.GetObject(DefaultImageName)));
                return _defaultImage;
            }
        }

        #endregion Resources 


        #region ILockable

        private readonly object _lock = new object();

        /// <summary>Object's lock</summary>
        public object Lock
        {
            get { return _lock; }
        }

        #endregion ILockable


        #region Mathematica

        protected MathematicaInterface _mathInt;
        
        /// <summary>Mathematica interface object used to perform calculations.</summary>
        protected MathematicaInterface MathInt
        {
            get
            {
                lock (Lock)
                {
                    if (_mathInt == null)
                        _mathInt = new MathematicaInterface();
                    return _mathInt;
                }
            }
            set
            {
                lock (Lock)
                {
                    _mathInt = value;
                }
            }
        }


        /// <summary>Mathematica's link object.
        /// <para>Getter always returns a valid link object, creating a new one if not yet created.</para></summary>
        public IKernelLink MathLink
        {
            get
            {
                return MathInt.MathLink;
            }
        }


        /// <summary>Mathematica kernel.</summary>
        public MathKernel MathKernel
        {
            get
            {
                lock (Lock)
                {
                    return MathInt.MathKernel;
                }
            }
        }

        #endregion Mathematica


        protected int _whichImage;

        private void txtInput_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void btnEvaluate_Click(object sender, EventArgs e)
        {

            if (MathKernel.IsComputing)
            {
                MathKernel.Abort();
            }
            else
            {
                MathKernel mathKernel = this.MathKernel;

                // Clear out any results from previous computation.
                textResult.Text = "";
                txtMessages.Text = "";
                txtPrint.Text = "";
                imgGraphics.Image = null;

                // This could be done in the initialization code.
                mathKernel.GraphicsHeight = imgGraphics.Height;
                mathKernel.GraphicsWidth = imgGraphics.Width;

                //btnEvaluate = null;

                // Perform the computation. Compute() will not return until the result has arrived.
                btnEvaluate.Text = ButtonTextAbort;
                string expression = txtInput.Text;
                if (chkNumeric.Checked)
                    expression = "N[" + expression + "]";
                mathKernel.Compute(expression);
                btnEvaluate.Text = ButtonTextEvaluate;

                // Populate the various boxes with results.
                textResult.Text = (string)mathKernel.Result;

                foreach (string msg in mathKernel.Messages)
                {
                    txtMessages.Text += (msg + "\r\n");
                }

                foreach (string p in mathKernel.PrintOutput)
                {
                    txtPrint.Text += p;
                }

                // The Graphics property returns an array of images, so it can accommodate
                // more than one graphic produced, but we only have room for one image.
                Image image = null;
                if (mathKernel.Graphics.Length > 0)
                {
                    image = mathKernel.Graphics[0];
                    imgGraphics.Image = image;
                }
                else
                {
                    if (Resources != null)
                    {
                        image = ((System.Drawing.Image)(Resources.GetObject("imgGraphics.Image")));
                    }
                    if (image != null)
                        imgGraphics.Image = ((System.Drawing.Image)(Resources.GetObject("imgGraphics.Image")));
                }
            }
        }


    }
}
