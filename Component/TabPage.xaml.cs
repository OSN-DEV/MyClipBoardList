using MyClipBoardList.Util;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyClipBoardList.Component {
    /// <summary>
    /// カスタムタブ
    /// </summary>
    public partial class TabPage : UserControl {

        #region Declaration
        internal class TabSelectEventArgs:EventArgs {
            public int Index = -1;
            public TabSelectEventArgs(int index) {
                this.Index = index;
            }
        }
        internal delegate void TabPageEventHandler(TabSelectEventArgs args);
        internal event TabPageEventHandler TabSelected;
        private Button _selectedItem = null;
        #endregion

        #region Public Property
        public int CurrentTabIndex {
            get {
                return Int32.Parse(this._selectedItem.Content.ToString())-1;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        public TabPage() {
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Event
        /// <summary>
        /// Tab button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tab_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            this.SelectTab(button);
            var index = Int32.Parse(button.Content.ToString());
            this.TabSelected?.Invoke(new TabSelectEventArgs(index - 1));
        }
        #endregion

        #region Public Method
        /// <summary>
        /// select tab
        /// </summary>
        /// <param name="index">tab index</param>
        public void SelectTab(int index) {
            this.SelectTab(this.cContainer.Children[index] as Button);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize 
        /// </summary>
        private void Initialize() {
            // this.Width = Constant.TabCount * 30;
            for (int i=0; i < Constant.TabCount; i++) {
                var button = new Button();
                button.Background = null;
                button.Foreground = Constant.DefaultForeGround;
                button.Content = ((i+1).ToString());
                button.Click += Tab_Click;
                this.cContainer.Children.Add(button);
            }
            this.SelectTab(0);
        }

        /// <summary>
        /// select tab
        /// </summary>
        /// <param name="button"></param>
        private void SelectTab(Button button) {
            if (null != this._selectedItem) {
                this._selectedItem.Background = null;
                this._selectedItem.IsEnabled = true;
                this._selectedItem.Foreground = Constant.DefaultForeGround;
            }
            button.IsEnabled = false;
            button.Foreground = Constant.SelectedForeGround;
            button.Background = Constant.SelectedBackGround;
            this._selectedItem = button;
        }
        #endregion
    }
}
