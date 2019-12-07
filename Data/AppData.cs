using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Data;

namespace MyClipBoardList.Data {
    public class AppData : AppDataBase<AppData> {

        #region Declaration
        private static readonly string FileName = "app.data";
        #endregion

        #region Public Property
        public int SelectedTab { set; get; } = 0;
        public List<List<string>> Items { set; get; } = new List<List<string>>();

        public class Location {
            public double X { set; get; } = -1;
            public double Y { set; get; } = -1;
        }
        public Location Pos { set; get; } = new Location();
        #endregion

        #region Public Method
        /// <summary>
        /// AppDataのインスタンスを取得する。
        /// </summary>
        /// <returns>AppDataのインスタンス</returns>
        internal static AppData GetInstance() {
            if (null == AppDataBase<AppData>._instance) {
                return AppDataBase<AppData>.GetInstanceBase(AppCommon.GetAppPath() + FileName);
            } else {
                return AppDataBase<AppData>.GetInstanceBase();
            }
        }

        /// <summary>
        /// AppDataの情報を保存する。
        /// </summary>
        internal void Save() {
            base.SaveToXml();
        }
        #endregion
    }
}
