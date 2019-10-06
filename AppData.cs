using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Data;

namespace MyClipBoardList {
    public class AppData : AppDataBase<AppData> {
        #region Declaration
        private static readonly string FileName = "app.data";
        #endregion

        #region Public Property
        public List<string> Items { set; get; } = new List<string>();
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
