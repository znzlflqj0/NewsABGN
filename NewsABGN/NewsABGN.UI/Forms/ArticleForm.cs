﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using NewsABGN.Logic;

namespace NewsABGN.UI
{
    public partial class ArticleForm : Form
    {
        private string _link;
        public ArticleForm(string link)
        {
            InitializeComponent();
            var details = LogicRepository.Controller.ArticleDetail.FillMainContent(link);
            uscArticleMainControl.FillTitle(details["title"]);
            uscArticleMainControl.FillContent(details["content"]);
            _link = link;            
        }

        #region window function: Exit / Drag / Resize
        private void UscTitleBar_ExitButtonClicked(object sender, User_Controls.TitleBar.TitleBarControl.ExitButtonClickedEventArgs e)
        {
            this.Close();
        }
        // drag & move

        private int _mouseOffsetX;
        private int _mouseOffsetY;
        private bool _mouseDown;

        private void UscTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseOffsetX = MousePosition.X - this.Left;
            _mouseOffsetY = MousePosition.Y - this.Top;
            _mouseDown = true;
        }

        private void UscTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
                this.SetDesktopLocation(MousePosition.X - _mouseOffsetX, MousePosition.Y - _mouseOffsetY);
        }

        private void UscTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        // window resize
        protected override void WndProc(ref Message m)
        {
            const int RESIZE_HANDLE_SIZE = 10;

            switch (m.Msg)
            {
                case 0x0084/*NCHITTEST*/ :
                    base.WndProc(ref m);

                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)12/*HTTOP*/ ;
                            else
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                        }
                        else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)10/*HTLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                            else
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }
                        else
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc(ref m);
        }
        #endregion

        private void ArticleTitleBarControl1_ExitButtonClicked(object sender, User_Controls.ArticleTitleBarControl.ExitButtonClickedEventArgs e)
        {
            this.Close();
        }

        private void UscArticleMainControl_LinkClicked(object sender, User_Controls.ArticleMainControl.LinkClickedEventArgs e)
        {
            uscArticleMainControl.Open_url(_link);
        }
    }
}
