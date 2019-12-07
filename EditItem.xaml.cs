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
using System.Windows.Shapes;

namespace MyClipBoardList {
    /// <summary>
    /// EditItem.xaml の相互作用ロジック
    /// </summary>
    public partial class EditItem : Window {
        #region Declaration
        #endregion

        #region Public Property
        public string Item { set; get; } = "";
        #endregion

        #region Constructor
        public EditItem() {
            InitializeComponent();
        }

        public EditItem(Window owner, string item) {
            InitializeComponent();

            this.Owner = owner;
            this.Item = item;
            this.Initialize();
        }
        #endregion

        #region Event

        /// <summary>
        /// CancelButton Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_Click(object sender, RoutedEventArgs e) {
            this.Item = this.cItem.Text;
            this.DialogResult = true;
        }


        /// <summary>
        /// change file url. update icon file if need.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_TextChanged(object sender, TextChangedEventArgs e) {
            this.cOK.IsEnabled = (0 < this.cItem.Text.Length);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize screen
        /// </summary>
        private void Initialize() {
            this.cItem.Text = this.Item;
            this.cOK.IsEnabled = (0 < this.cItem.Text.Length);

            this.Loaded += (sender, e) => {
                this.cItem.Focus();
            };
        }
        #endregion
    }
}
