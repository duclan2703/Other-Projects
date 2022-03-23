using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Parse.Forms.CustomUC;

namespace Parse.Forms
{
    public partial class frmInvoicePdf : DevExpress.XtraEditors.XtraForm
    {
        IMainForm Main;
        public frmInvoicePdf()
        {
            InitializeComponent();
        }

        public frmInvoicePdf(string pathFile, IMainForm main)
        {
            InitializeComponent();
            Main = main;
            ViewPdf(pathFile);
        }

        void ViewPdf(string pathFile)
        {
            pdfViewerInvoice.LoadDocument(pathFile);
        }
    }
}