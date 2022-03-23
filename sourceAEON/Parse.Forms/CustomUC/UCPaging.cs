using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parse.Core.Utils;

namespace Parse.Forms.CustomUC
{
    public partial class UCPaging : UserControl
    {
        public event PagingEventHandler NextClick;
        public event PagingEventHandler PrevClick;
        public event PagingEventHandler FirstClick;
        public event PagingEventHandler LastClick;

        private PagingComponent paging = new PagingComponent(1, 30);
        public int PageIndex
        {
            get
            {
                return this.paging.PageIndex;
            }
            set
            {
                this.paging.PageIndex = value;
            }
        }
        public int PageSize
        {
            get
            {
                return this.paging.PageSize;
            }
            set
            {
                this.paging.PageSize = value;
            }
        }
        public int? Total
        {
            get
            {
                return this.paging.Total;
            }
            set
            {
                this.paging.Total = value;
            }
        }
        public int? TotalPage
        {
            get
            {
                return this.paging.TotalPage;
            }
        }


        public UCPaging()
        {
            InitializeComponent();

            this.btnNext.Click += new System.EventHandler(this.OnNext_Click);
            this.btnPrev.Click += new System.EventHandler(this.OnPrev_Click);
            this.btnFirst.Click += new System.EventHandler(this.OnFirst_Click);
            this.btnLast.Click += new System.EventHandler(this.OnLast_Click);
            this.UpdatePagingState();
        }
        public void UpdatePagingState()
        {

            // set các trạng thái enable của các nút
            btnNext.Enabled = this.paging.HasNext();
            btnPrev.Enabled = this.paging.HasPrev();
            btnLast.Enabled = this.paging.HasNext();
            btnFirst.Enabled = this.paging.HasPrev();

            // set text
            if (this.TotalPage.HasValue)
                this.lblPageInfo.Text = string.Format("Trang {0}/{1}", this.PageIndex, this.TotalPage.Value > 0 ? this.TotalPage.Value : 1);
            else
                this.lblPageInfo.Text = string.Format("Trang {0}/1", this.PageIndex);
        }
        protected void OnNext_Click(object sender, EventArgs e)
        {
            //bubble the event up to the parent
            if (this.NextClick != null)
            {
                // tạo event argument
                PagingEventArgs arg = new PagingEventArgs();
                arg.NextPageIndex = this.paging.TotalPage.HasValue ? (this.PageIndex < this.paging.TotalPage.Value ? this.PageIndex + 1 : this.paging.TotalPage.Value) : this.PageIndex + 1;
                // trigger event
                this.NextClick(sender, arg);

                // cập nhật trạng thái paging
                this.UpdatePagingState();
            }
        }
        protected void OnPrev_Click(object sender, EventArgs e)
        {
            //bubble the event up to the parent
            if (this.PrevClick != null)
            {
                // tạo event argument
                PagingEventArgs arg = new PagingEventArgs();
                arg.NextPageIndex = this.PageIndex > 1 ? this.PageIndex - 1 : 1;
                // trigger event
                this.PrevClick(sender, arg);

                // cập nhật trạng thái paging
                this.UpdatePagingState();
            }
        }
        protected void OnFirst_Click(object sender, EventArgs e)
        {
            //bubble the event up to the parent
            if (this.FirstClick != null)
            {
                // tạo event argument
                PagingEventArgs arg = new PagingEventArgs();
                arg.NextPageIndex = 1;
                // trigger event
                this.FirstClick(sender, arg);

                // cập nhật trạng thái paging
                this.UpdatePagingState();
            }
        }
        protected void OnLast_Click(object sender, EventArgs e)
        {
            //bubble the event up to the parent
            if (this.LastClick != null)
            {
                // tạo event argument
                PagingEventArgs arg = new PagingEventArgs();
                arg.NextPageIndex = this.paging.TotalPage.HasValue ? this.paging.TotalPage.Value : 1;
                // trigger event
                this.LastClick(sender, arg);

                // cập nhật trạng thái paging
                this.UpdatePagingState();
            }
        }

    }

    public delegate void PagingEventHandler(object sender, PagingEventArgs e);

    public class PagingEventArgs : EventArgs
    {
        public int NextPageIndex { get; set; }
    }
}
