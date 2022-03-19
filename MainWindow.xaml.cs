using MyClipBoardList.Component;
using MyClipBoardList.Data;
using MyClipBoardList.Util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyClipBoardList {
    /// <summary>
    /// メイン
    /// </summary>
    public partial class MainWindow : Window {

        #region Declaration
        private List<ClipItem> _clipItems = new List<ClipItem>();
        private ClipItem _selectedClipItem = null;
        private List<List<string>> _items;
        private int _currentTab = 0;
        private readonly int GridOffset = 2;
        private AppData _settings;
        private int _inputNum = -1;
        private readonly string TopMostTitle = "[T] - ";
        private readonly HotKeyHelper _hotkey;
        private readonly System.Windows.Forms.NotifyIcon _notifyIcon = new System.Windows.Forms.NotifyIcon();
        #endregion

        #region Constructor
        public MainWindow() {
            InitializeComponent();
            this._hotkey = new HotKeyHelper(this);
            this.Initialize();
        }
        #endregion;

        #region Event

        /// <summary>
        /// window close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e) {
            //this._settings.Pos.X = this.Left;
            //this._settings.Pos.Y = this.Top;
            //this._settings.Save();
            this._hotkey.Dispose();
        }

        /// <summary>
        /// key event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            // Shift + 1～9でクリップボードにコピー
            if (Key.D1 <= e.Key && e.Key <= Key.D9) {
                e.Handled = true;
                this._inputNum = e.Key - Key.D1;
                if (this.IsModifierPressed(ModifierKeys.Shift)) {
                    AppUtil.SetClipBoard(this._clipItems[this._inputNum].Text);
                    this._inputNum = -1;
                }
                return;
            }

            switch (e.Key) {
                // T: 最前面表示の切り替え
                case Key.T:
                    this.Topmost = !this.Topmost;
                    if (this.Topmost) {
                        this.Title = TopMostTitle + this.Title;
                    } else {
                        this.Title = this.Title.Replace(TopMostTitle, "");
                    }
                    break;
                // 矢印でタブ移動
                case Key.Left:
                case Key.Right:
                    this.ChangeTab(e.Key == Key.Left);
                    break;
                case Key.Escape:
                    this.SetWindowsState(true);
                    break;
            }

            if (-1 == this._inputNum) {
                return;
            }
            switch(e.Key) {
                case Key.E:
                    var dialog = new EditItem(this, this.GetCurrentItems()[this._inputNum]);
                    if (true == dialog.ShowDialog()) {
                        this._clipItems[this._inputNum].Text = dialog.Item;
                        GetCurrentItems()[this._inputNum] = this._clipItems[this._inputNum].Text;
                        this._settings.Save();
                    }
                    break;

                case Key.P:
                    this.cTab.SelectTab(this._inputNum);
                    this.Tab_TabSelected(new TabPage.TabSelectEventArgs(this._inputNum));
                    break;

                case Key.D:
                    this._clipItems[this._inputNum].Text = "";
                    this.GetCurrentItems()[this._inputNum] = "";
                    this._settings.Save();
                    break;

                case Key.C:
                    AppUtil.SetClipBoard(this._clipItems[this._inputNum].Text);
                    break;

                case Key.V:
                    this._clipItems[this._inputNum].Text = Clipboard.GetText();
                    GetCurrentItems()[this._inputNum] = this._clipItems[this._inputNum].Text;
                    this._settings.Save();
                    break;
            }

            this._inputNum = -1;
        }

        /// <summary>
        /// ta select event
        /// </summary>
        /// <param name="args"></param>
        private void Tab_TabSelected(TabPage.TabSelectEventArgs args) {
            this._currentTab = args.Index;
            this.ShowClipItems(args.Index);
            this._settings.SelectedTab = args.Index;
            this._settings.Save();
        }

        /// <summary>
        /// clip item is select event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClipItem_Selected(object sender, ClipItem.ClipItemEventArgs e) {
            var currentItem = sender as ClipItem;
            if (null != this._selectedClipItem) {
                this._selectedClipItem.IsSelected = false;
            }
            this._selectedClipItem = currentItem;
        }

        /// <summary>
        /// [show] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyMenuShow_Click(object sender, EventArgs e) {
            this.SetWindowsState(false);
            this.Activate();
        }

        /// <summary>
        /// [exit] click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyMenuExit_Click(object sender, EventArgs e) {
            this._notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize
        /// </summary>
        private void Initialize() {
            this._settings = AppData.GetInstance();
            this._items = this._settings.Items;
            this.ContentRendered += (sender, e) => {
                this.SetWindowsState(true);
            };

            // set window position
            if (0 <= this._settings.Pos.X && (this._settings.Pos.X + this.Width) < SystemParameters.VirtualScreenWidth) {
                this.Left = this._settings.Pos.X;
            }
            if (0 <= this._settings.Pos.Y && (this._settings.Pos.Y + this.Height) < SystemParameters.VirtualScreenHeight) {
                this.Top = this._settings.Pos.Y;
            }

            // initialize tab
            this.cTab.TabSelected += Tab_TabSelected;
            this._currentTab = AppData.GetInstance().SelectedTab;
            this.cTab.SelectTab(this._currentTab);

            // initalize saved data
            while (this._items.Count < Constant.TabCount) {
                this._items.Add(new List<string>());
            }
            foreach (var items in this._items) {
                while (items.Count < Constant.ItemCount) {
                    items.Add("");
                }
            }

            // create grid item
            for (int i = 0; i < Constant.ItemCount; i++) {
                this.cContainer.RowDefinitions.Add(new RowDefinition());

                // add label
                var text = new TextBlock();
                text.Text = (i + 1).ToString();
                text.Width = 20;
                text.VerticalAlignment = VerticalAlignment.Center;
                text.TextAlignment = TextAlignment.Center;
                text.FontSize = 14;
                Grid.SetRow(text, i + GridOffset);
                Grid.SetColumn(text, 0);
                this.cContainer.Children.Add(text);

                // add clip item
                var clipItem = new ClipItem();
                clipItem.ClipItemSelected += ClipItem_Selected;
                clipItem.Text = this._items[this._currentTab][i];
                Grid.SetRow(clipItem, i + GridOffset);
                Grid.SetColumn(clipItem, 1);
                this.cContainer.Children.Add(clipItem);
                this._clipItems.Add(clipItem);
            }

            // set title
            var fullname = typeof(App).Assembly.Location;
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullname);
            this.Title = $"MyClipBoard({info.FileVersion})";

            // register hot key
            this._hotkey.Register(ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt, Key.P, (_, __) => {
                if (!this.ShowInTaskbar) {
                    this.SetWindowsState(false);
                } else {
                    if (this.WindowState == WindowState.Minimized) {
                        this.WindowState = WindowState.Normal;
                    }
                    this.Activate();
                }
            }
            );

            // set up notify icon
            this._notifyIcon.Text = "MyClipBoard";
            this._notifyIcon.Icon = new System.Drawing.Icon("app.ico");
            var menu = new System.Windows.Forms.ContextMenuStrip();
            var menuItemShow = new System.Windows.Forms.ToolStripMenuItem {
                Text = "show"
            };
            menuItemShow.Click +=  this.NotifyMenuShow_Click;
            menu.Items.Add(menuItemShow);

            var menuItemExit = new System.Windows.Forms.ToolStripMenuItem {
                Text = "exit"
            };
            menuItemExit.Click += this.NotifyMenuExit_Click;
            menu.Items.Add(menuItemExit);

            this._notifyIcon.ContextMenuStrip = menu;
            this._notifyIcon.Visible = false;
        }

        /// <summary>
        /// check if modifiered key is pressed
        /// </summary>
        /// <param name="key">modifier key</param>
        /// <returns>true:modifiered key is pressed, false:otherwise</returns>
        private bool IsModifierPressed(ModifierKeys key) {
            return (Keyboard.Modifiers & key) != ModifierKeys.None;
        }

        /// <summary>
        /// show Clip items
        /// </summary>
        /// <param name="index">tab index</param>
        private void ShowClipItems(int index) {
            for(int i=0; i < Constant.ItemCount; i++) {
                this._clipItems[i].Text = this.GetCurrentItems()[i];
            }
        }

        /// <summary>
        /// Get current items
        /// </summary>
        /// <returns></returns>
        private List<string> GetCurrentItems() {
            return this._items[this._currentTab];
        }


        /// <summary>
        /// set window state
        /// </summary>
        /// <param name="minimize">true:minize, false:normalize</param>
        private void SetWindowsState(bool minimize) {
            this.WindowState = minimize ? WindowState.Minimized : WindowState.Normal;
            this.ShowInTaskbar = !minimize;
            this._notifyIcon.Visible = minimize;
            if (minimize) {
                this._settings.Pos.X = this.Left;
                this._settings.Pos.Y = this.Top;
                this._settings.Save();
            } else {
                this.Activate();
            }
        }

        /// <summary>
        /// change current tab
        /// </summary>
        /// <param name="prev"></param>
        private void ChangeTab(bool prev) {
            var index = this.cTab.CurrentTabIndex;
            index = index + (prev ? -1 : 1);
            if (index < 0) {
                index = Constant.TabCount-1;
            } else if (Constant.TabCount <= index) {
                index = 0;
            }
            this.cTab.SelectTab(index);
            this.Tab_TabSelected(new TabPage.TabSelectEventArgs(index));
        }
        #endregion

    }
}
