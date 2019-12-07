using System.Windows.Media;

namespace MyClipBoardList.Util {
    internal static class Constant {
        public readonly static Brush SelectedBackGround = new SolidColorBrush(Color.FromRgb(82, 190, 128));
        public readonly static Brush SelectedForeGround = new SolidColorBrush(Colors.White);
        public readonly static Brush DefaultForeGround = new SolidColorBrush(Color.FromRgb(33, 33, 33));
        public readonly static int TabCount = 9;
        public readonly static int ItemCount = 9;
    }
}
