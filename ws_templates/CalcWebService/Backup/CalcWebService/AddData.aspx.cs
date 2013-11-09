using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace CalcWebService
{
    public partial class AddData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            localhost.Service1 ws = new CalcWebService.localhost.Service1();
            ws.EnableDecompression = true;
            double sum = ws.Add(System.Convert.ToInt16(txtFirst.Text), System.Convert.ToInt16(txtSecond.Text), System.Convert.ToInt16(txtThird.Text));
            txtOutput.Text = sum.ToString();

        }
    }
}
