using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace JavaScience
{
public class PswdDialog : Form
{
  private String 	TITLE =	"PswdDialog";
  private String	ICONIMAGE = "JavaScience.keylock.ico";
  private Label		lbl;
  private TextBox	pwbox;
  private Button	okButton;
  private Button	cancelButton;
  private PictureBox    imagebox;
  public PswdDialog(String title) : base()
  { 
   this.TITLE = title;
   InitializeComponent();
   LoadImageIcon();
  }

  public PswdDialog() : base()
  { 
   InitializeComponent();
   LoadImageIcon();
  }

 public String GetPswd() {
  string pwd = this.pwbox.Text;
  pwbox.Text = "";
  return pwd;
 }

 private void LoadImageIcon()
 {
  Assembly oass = Assembly.GetExecutingAssembly();
  try
  {
   Stream icostream = oass.GetManifestResourceStream(ICONIMAGE);
   imagebox.Image = (Image) new Bitmap(icostream);
  }
  catch { }
 }

 private void InitializeComponent()
 {
  this.Size = new Size(250, 160);
  this.Text = TITLE;
  this.StartPosition = FormStartPosition.Manual;
  this.SetDesktopLocation(150,150) ;
  this.TopMost = true;

  imagebox = new PictureBox();
  imagebox.Location = new Point(20, 10);
  imagebox.SizeMode = PictureBoxSizeMode.AutoSize ;

  lbl = new Label();
  lbl.Text = "Password:";
  lbl.Location = new Point(20, 50);

  pwbox = new TextBox();
  pwbox.Location = new Point(120,50);
  pwbox.Size = new Size(100, 100);
  pwbox.Text = "";
  pwbox.PasswordChar = '*';

  okButton = new Button();
  okButton.Text = "OK";
  okButton.Size = new Size(80, 24);
  okButton.Location =  new Point( 20, this.Height - 70 );
  okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
  okButton.DialogResult = DialogResult.OK;
  okButton.BackColor = Color.LightGreen;
  cancelButton = new Button();
  cancelButton.Text = "Cancel";
  cancelButton.Size = new Size(80, 24);
  cancelButton.Location = new Point( this.ClientSize.Width-100, this.Height - 70 );
  cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
  cancelButton.DialogResult = DialogResult.Cancel;
  cancelButton.BackColor = Color.Red;

  this.Controls.Add(imagebox);
  this.Controls.Add(lbl);
  this.Controls.Add(pwbox);
  this.Controls.Add(okButton);
  this.Controls.Add(cancelButton);
 }


 [STAThread]
 public static void Main1()
  {
   PswdDialog dlg = new PswdDialog("PFX Password");
   DialogResult result = dlg.ShowDialog();

  //---- process the dialogbox textfield ------
   if (result == DialogResult.OK)
	Console.WriteLine("pswd: {0}",  dlg.GetPswd());
   else
	Console.WriteLine("Cancelled: {0}", result);
   dlg.Dispose();
  }

 }
}

