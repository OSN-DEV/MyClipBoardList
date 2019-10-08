using System;
using System.Collections;
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
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        #region Declaration
        private List<ClipItem> _clipItems;
        private int _currentIndex = -1;

        #endregion
        public MainWindow() {
            InitializeComponent();

            this._clipItems = new List<ClipItem> {
                this.cItem00
               ,this.cItem01
               ,this.cItem02
               ,this.cItem03
               ,this.cItem04
               ,this.cItem05
               ,this.cItem06
               ,this.cItem07
               ,this.cItem08
               ,this.cItem09
            };

            var savedItems = AppData.GetInstance().Items;
            while(savedItems.Count < this._clipItems.Count) {
                savedItems.Add("");
            }

            for (int i=0; i < this._clipItems.Count; i++) {
                this._clipItems[i].Text = savedItems[i];
                this._clipItems[i].ClipItemSelected += ClipItem_Selected;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.V:
                    if (this.IsModifierPressed(ModifierKeys.Control)) {
                        if (0 <= this._currentIndex) {
                            this._clipItems[this._currentIndex].Text = Clipboard.GetText();
                            AppData.GetInstance().Items[this._currentIndex] = this._clipItems[this._currentIndex].Text;
                            AppData.GetInstance().Save();
                        }
                    }
                    break;
                case Key.Delete:
                    if (0 <= this._currentIndex) {
                        this._clipItems[this._currentIndex].Text = "";
                        AppData.GetInstance().Items[this._currentIndex] = "";
                        AppData.GetInstance().Save();
                    }
                    break;
            }
        }

        private void ClipItem_Selected(object sender, ClipItem.ClipItemEventArgs e) {
            var currentItem = sender as ClipItem;
            this._currentIndex = currentItem.Index;
            for (int i = 0; i < this._clipItems.Count; i++) {
                if (this._currentIndex == i) {
                    continue;
                }
                this._clipItems[i].IsSelected = false;

            }
        }

        /// <summary>
        /// check if modifiered key is pressed
        /// </summary>
        /// <param name="key">modifier key</param>
        /// <returns>true:modifiered key is pressed, false:otherwise</returns>
        private bool IsModifierPressed(ModifierKeys key) {
            return (Keyboard.Modifiers & key) != ModifierKeys.None;
        }

    }
}
