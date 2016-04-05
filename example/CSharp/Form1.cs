using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsApplication1
{
	public partial class Form1 : Form
	{
		const int EVERYTHING_OK	= 0;
		const int EVERYTHING_ERROR_MEMORY = 1;
		const int EVERYTHING_ERROR_IPC = 2;
		const int EVERYTHING_ERROR_REGISTERCLASSEX = 3;
		const int EVERYTHING_ERROR_CREATEWINDOW = 4;
		const int EVERYTHING_ERROR_CREATETHREAD = 5;
		const int EVERYTHING_ERROR_INVALIDINDEX = 6;
		const int EVERYTHING_ERROR_INVALIDCALL = 7;

        [DllImport("Everything32.dll", CharSet = CharSet.Unicode)]
		public static extern int Everything_SetSearchW(string lpSearchString);
		[DllImport("Everything32.dll")]
		public static extern void Everything_SetMatchPath(bool bEnable);
		[DllImport("Everything32.dll")]
		public static extern void Everything_SetMatchCase(bool bEnable);
		[DllImport("Everything32.dll")]
		public static extern void Everything_SetMatchWholeWord(bool bEnable);
		[DllImport("Everything32.dll")]
		public static extern void Everything_SetRegex(bool bEnable);
		[DllImport("Everything32.dll")]
		public static extern void Everything_SetMax(int dwMax);
		[DllImport("Everything32.dll")]
		public static extern void Everything_SetOffset(int dwOffset);

		[DllImport("Everything32.dll")]
		public static extern bool Everything_GetMatchPath();
		[DllImport("Everything32.dll")]
		public static extern bool Everything_GetMatchCase();
		[DllImport("Everything32.dll")]
		public static extern bool Everything_GetMatchWholeWord();
		[DllImport("Everything32.dll")]
		public static extern bool Everything_GetRegex();
		[DllImport("Everything32.dll")]
		public static extern UInt32 Everything_GetMax();
		[DllImport("Everything32.dll")]
		public static extern UInt32 Everything_GetOffset();
		[DllImport("Everything32.dll")]
		public static extern string Everything_GetSearchW();
		[DllImport("Everything32.dll")]
		public static extern int Everything_GetLastError();

		[DllImport("Everything32.dll")]
		public static extern bool Everything_QueryW(bool bWait);

		[DllImport("Everything32.dll")]
		public static extern void Everything_SortResultsByPath();

		[DllImport("Everything32.dll")]
		public static extern int Everything_GetNumFileResults();
		[DllImport("Everything32.dll")]
		public static extern int Everything_GetNumFolderResults();
		[DllImport("Everything32.dll")]
		public static extern int Everything_GetNumResults();
		[DllImport("Everything32.dll")]
		public static extern int Everything_GetTotFileResults();
		[DllImport("Everything32.dll")]
		public static extern int Everything_GetTotFolderResults();
		[DllImport("Everything32.dll")]
		public static extern int Everything_GetTotResults();
		[DllImport("Everything32.dll")]
		public static extern bool Everything_IsVolumeResult(int nIndex);
		[DllImport("Everything32.dll")]
		public static extern bool Everything_IsFolderResult(int nIndex);
		[DllImport("Everything32.dll")]
		public static extern bool Everything_IsFileResult(int nIndex);
        [DllImport("Everything32.dll", CharSet = CharSet.Unicode)]
        public static extern void Everything_GetResultFullPathNameW(int nIndex, StringBuilder lpString, int nMaxCount);
		[DllImport("Everything32.dll")]
		public static extern void Everything_Reset();

        private int _visibleItems;
        private int _Results;
        private int _LoopEnd;


        public Form1()
		{
			InitializeComponent();
		}

        private void Form1_Load(object sender, EventArgs e)
        {
            // set the search
            Everything_SetSearchW(textBox1.Text);

            // execute the query
            Everything_QueryW(true);

            updateListBoxItem();
            vScrollBar1.Value = 0;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            // set the search
            Everything_SetSearchW(textBox1.Text);

            // execute the query
            Everything_QueryW(true);

            updateListBoxItem();
            vScrollBar1.Value = 0;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            updateListBoxItem();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            updateListBoxItem();

            label1.Text = vScrollBar1.Maximum.ToString() + ", " + vScrollBar1.Value.ToString();
        }

        private void updateListBoxItem()
        {
            const int bufsize = 260;
            StringBuilder buf = new StringBuilder(bufsize);

            //속도 향상을 위해 List컨트롤에 파일목록을 한꺼번에 모두 넣지 않고 눈에 보이는 것만 넣는다.
            // _Results <= _LoopEnd <= _visibleItems
            _visibleItems = listBox1.ClientRectangle.Height / listBox1.ItemHeight;
            _Results = Everything_GetNumResults();
            _LoopEnd = (_Results <= _visibleItems) ? _Results : _visibleItems;

            toolStripStatusLabel1.Text = _Results + "개 항목";

            if (_Results <= _visibleItems)
            {
                vScrollBar1.Maximum = 0;
                vScrollBar1.Value = 0;
            } else {
                vScrollBar1.Maximum = _Results - _visibleItems;
            }

            // 정렬
            // Everything_SortResultsByPath();

            // 지우고 새로 그린다.
            listBox1.Items.Clear();

            

            //검색한 전체 파일목록 중 눈에 보이는 부분을 가져와서 그린다.
            for (int i = 0; i < _LoopEnd; i++)
            {
                Everything_GetResultFullPathNameW(vScrollBar1.Value + i, buf, bufsize);
                listBox1.Items.Insert(i, buf);
            }
        }
    }
}