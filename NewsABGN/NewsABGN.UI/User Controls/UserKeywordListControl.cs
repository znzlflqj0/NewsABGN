﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewsABGN.DB;

namespace NewsABGN.UI.User_Controls
{
    public partial class UserKeywordListControl : UserControl
    {
        public UserKeywordListControl()
        {
            InitializeComponent();
        }

        private List<UserKeywordControl> _userKeywordControls = new List<UserKeywordControl>();
        public List<UserKeywordControl> TestFill()
        {
            flpKeywordList.Controls.Clear();
            List<UserKeywordControl> list = new List<UserKeywordControl>();

            for (int i=0; i< 20; i++)
            {
                UserKeywordControl userKeyword = new UserKeywordControl((i+1)+"    "+ "keyword" + i);
                _userKeywordControls.Add(userKeyword);
                flpKeywordList.Controls.Add(userKeyword);
            }

            return _userKeywordControls;
        }

        public List<UserKeywordControl> FillKeywords(int memberId)
        {
            List<Keyword> keywords = DataRepository.Keyword.GetByMemberId(memberId);

            foreach(var keyword in keywords)
            {
                var userKeywordControl = new UserKeywordControl(keyword.PushName);
                _userKeywordControls.Add(userKeywordControl);
                flpKeywordList.Controls.Add(userKeywordControl);
            }
            return _userKeywordControls;
        }

        public void EmptyKeywords()
        {
            _userKeywordControls.Clear();
            flpKeywordList.Controls.Clear();
        }
    }
}
