using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyClipBoardList {
    /// <summary>
    /// ClipItem.xaml の相互作用ロジック
    /// </summary>
    public partial class ClipItem : UserControl {

        #region Declaration
        // Event
        public class ClipItemEventArgs:EventArgs {
            public int Index { private set; get; }
            public ClipItemEventArgs(int index) {
                this.Index = index;
            }
        }
        public delegate void ClipItemEventHander(object sender, ClipItemEventArgs e);
        public event ClipItemEventHander ClipItemSelected;


        // Public Property
        public int Index { set; get; }
        public string Text {
            set { this.cItem.Text = value; }
            get { return this.cItem.Text; }
        }

        private bool _isSelected = false;
        public bool IsSelected {
            get { return this._isSelected; }
            set {
                this._isSelected = value;
                this.cSeparator.Background = new SolidColorBrush(this._isSelected ? Colors.Blue : Colors.Silver);
            }
        }
        #endregion

        #region Constructor
        public ClipItem() {
            InitializeComponent();
        }
        #endregion

        #region Event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_MouseDown(object sender, MouseButtonEventArgs e) {
            if (1 == e.ClickCount) {
                if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None) {
                    Clipboard.SetText(this.cItem.Text, TextDataFormat.Text);
                }

                if (!this.IsSelected) {
                    this.IsSelected = true;
                    if (null != this.ClipItemSelected) {
                        var args = new ClipItemEventArgs(this.Index);
                        this.ClipItemSelected(this, args);
                    }
                }
            } else {
                Clipboard.SetText(this.cItem.Text, TextDataFormat.Text);
            }
        }
        #endregion
    }
}
