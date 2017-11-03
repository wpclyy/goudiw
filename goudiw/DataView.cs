using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goudiw
{   
    class DataView
    {
        
        public DataView()
        {
           
        }

        /// <summary>
        /// 获取已保存产品规格信息
        /// </summary>
        /// <returns></returns>
        public DataSet viewspec(string str)
        {
            return MySqlHelper.GetDataSet(str, "SELECT * FROM `productinfo` ", null);
        }

        /// <summary>
        /// 获取已保存分类
        /// </summary>
        /// <returns></returns>
        public DataSet viewcat(string str)
        {
            return MySqlHelper.GetDataSet(str, "SELECT * FROM `category` ", null);
        }

        public DataSet getgoodtype(string str)
        {
            return MySqlHelper.GetDataSet(str, "SELECT * FROM `dsc_goods_type` ", null);
        }

        public DataSet getattrrow(string str,string cat_id)
        {
            return MySqlHelper.GetDataSet(str, "SELECT * FROM `dsc_attribute` where cat_id="+ cat_id, null);
        }
    }
}
