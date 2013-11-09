using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace EFA_WS
{
    public delegate void ExceptionEventDelegate(object sender, ExceptionEventArgs e);

    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs()
            : base()
        {

        }

        private Exception _ex;
        public Exception Ex
        {
            get
            {
                return _ex;
            }
            set
            {
                _ex = value;
            }
        }

        //TODO: move paketId out
        //private decimal _paketId;
        //public decimal PaketId
        //{
        //    get
        //    {
        //        return _paketId;
        //    }
        //    set
        //    {
        //        _paketId = value;
        //    }
        //}
    }

    class BulkCopyException : Exception
    {
        public BulkCopyException(string message)
            : base(message)
        {

        }
        public BulkCopyException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public enum BulkCopyExceptionType
        {

        }

    }
    class InsertIntoEfaPaketException : Exception
    {
        public InsertIntoEfaPaketException(string message)
            : base(message)
        {

        }
        public InsertIntoEfaPaketException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    class SaveXmlToDiskException : Exception
    {
        public SaveXmlToDiskException(string message)
            : base(message)
        {

        }
        public SaveXmlToDiskException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    class PaketStatusPovratnicaException : Exception
    {
        public PaketStatusPovratnicaException(string message)
            : base(message)
        {

        }
        public PaketStatusPovratnicaException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
