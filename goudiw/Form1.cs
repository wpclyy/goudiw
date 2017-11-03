using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goudiw
{
    public partial class Form1 : Form
    {
        DataTable cate = null;
        string strlocal = "server=127.0.0.1;user id=root;password=123456;database=GCollection";
        string strrote = "server=127.0.0.1;user id=root;password=123456;database=dasc";
        public Form1()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            DataView dv = new DataView();
            DataSet ds = dv.viewspec(strlocal);
            DataRowCollection dc = ds.Tables[0].Rows;
            for (int i = 0; i < dc.Count; i++)
            {
                ListViewItem li = new ListViewItem(dc[i]["id"].ToString());
                li.SubItems.Add(dc[i]["productID"].ToString());
                li.SubItems.Add(dc[i]["productTitle"].ToString());
                li.SubItems.Add(dc[i]["catId"].ToString());
                li.SubItems.Add(dc[i]["offerstatus"].ToString());
                li.SubItems.Add(dc[i]["supplystock"].ToString());
                li.Tag = dc[i];
                listView1.Items.Add(li);
            }

            DataSet cads = dv.viewcat(strlocal);
            cate = cads.Tables[0];

            DataSet gtds = dv.getgoodtype(strrote);
            comboBox1.DisplayMember = "cat_name";
            comboBox1.ValueMember = "cat_id";
            comboBox1.DataSource = gtds.Tables[0];
        }


        private void getbtn_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void descbtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, string[]> attrlist = new Dictionary<string, string[]>();
            if(comboBox1.SelectedItem!=null)
            {
                DataView dv = new DataView();
                DataSet ds = dv.getattrrow(strrote,comboBox1.SelectedValue.ToString());
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string key = ds.Tables[0].Rows[i]["attr_name"].ToString();
                    string val = ds.Tables[0].Rows[i]["attr_values"].ToString();

                    string[] vals = val.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    attrlist.Add(key, vals);
                }
            }

            if (listView1.SelectedItems.Count > 0)
            {

                ListView.SelectedListViewItemCollection slic = listView1.SelectedItems;

                //规格值sd
                //Dictionary<string, Dictionary<string, string>> spec = new Dictionary<string, Dictionary<string, string>>();
                foreach (ListViewItem li in slic)
                {
                    DataRow dr = (DataRow)li.Tag;
                    JObject sm = (JObject)JsonConvert.DeserializeObject(dr[6].ToString());
                    Dictionary<string, Dictionary<string, string>> spec =  Goodsattr(attrlist, sm);
                    foreach(KeyValuePair<string, Dictionary<string, string>> s in spec )
                    {
                        richTextBox1.AppendText(s.Key+":\r\n");
                        foreach(KeyValuePair<string, string> ss in s.Value)
                        {
                            richTextBox1.AppendText(ss.Key +ss.Value+ "\r\n");
                        }
                    }
                    //if (sm != null)
                    //{
                    //    //获取所有规格值
                    //    JToken mm = sm["skuList"];

                    //    foreach (JToken j in mm.Children())
                    //    {
                    //        JToken jc = j["attributeModelList"];
                    //        foreach (JToken jcc in jc)
                    //        {
                    //            string key = "";
                    //            string val = "";
                    //            if (jcc["sourceAttributeValue"] != null && jcc["sourceAttributeName"].ToString() != "")
                    //            {
                    //                key = jcc["sourceAttributeName"].ToString();
                    //                val = jcc["sourceAttributeValue"].ToString();
                    //            }
                    //            else if (jcc["targetAttributeValue"] != null && jcc["targetAttributeValue"].ToString() != "")
                    //            {
                    //                key = jcc["targetAttributeName"].ToString();
                    //                val = jcc["targetAttributeValue"].ToString();
                    //            }
                    //            //先进行数据库中规格值的对比，已存在就跳过
                    //            string[] vals = attrlist[key];

                    //            if(vals.Contains(val))
                    //            {
                    //                continue;
                    //            }

                    //            if (spec.ContainsKey(key))
                    //            {
                    //                if (!spec[key].ContainsKey(val))
                    //                {
                    //                    spec[key].Add(val, "");
                    //                }
                    //            }
                    //            else
                    //            {
                    //                Dictionary<string, string> lt = new Dictionary<string, string>();
                    //                lt.Add(val, "");
                    //                spec.Add(key, lt);
                    //            }
                    //        }
                    //    }
                    //}



                    //string smstr = dr[10].ToString();
                    //JArray sms = (JArray)JsonConvert.DeserializeObject(dr[10].ToString());
                    ////有图片的规格值
                    //Dictionary<string, string> specval = new Dictionary<string, string>();
                    //if (sms != null)
                    //{
                    //    foreach (JToken info in sms.Children())
                    //    {
                    //        JToken infoc = info["attributes"];

                    //        foreach (JToken jurl in infoc.Children())
                    //        {
                    //            string ss = jurl["attributeValue"].ToString();
                    //            string simg = jurl["skuImageUrl"].ToString().Trim();
                    //            if (simg != "" && !specval.ContainsKey(ss))
                    //            {
                    //                specval.Add(ss, simg);
                    //            }
                    //        }
                    //    }
                    //}

                    //string goodsql = "INSERT INTO `dsc_goods`(`goods_id`, `cat_id`, `user_cat`, `user_id`, `goods_sn`, `bar_code`, `goods_name`, `goods_name_style`, `click_count`, `brand_id`, `provider_name`, `goods_number`, `goods_weight`, `default_shipping`, `market_price`, `cost_price`, `shop_price`, `promote_price`, `promote_start_date`, `promote_end_date`, `warn_number`, `keywords`, `goods_brief`, `goods_desc`, `desc_mobile`, `goods_thumb`, `goods_img`, `original_img`, `is_real`, `extension_code`, `is_on_sale`, `is_alone_sale`, `is_shipping`, `integral`, `add_time`, `sort_order`, `is_delete`, `is_best`, `is_new`, `is_hot`, `is_promote`, `is_volume`, `is_fullcut`, `bonus_type_id`, `last_update`, `goods_type`, `seller_note`, `give_integral`, `rank_integral`, `suppliers_id`, `is_check`, `store_hot`, `store_new`, `store_best`, `group_number`, `is_xiangou`, `xiangou_start_date`, `xiangou_end_date`, `xiangou_num`, `review_status`, `review_content`, `goods_shipai`, `comments_number`, `sales_volume`, `comment_num`, `model_price`, `model_inventory`, `model_attr`, `largest_amount`, `pinyin_keyword`, `goods_product_tag`, `goods_tag`, `stages`, `stages_rate`, `freight`, `shipping_fee`, `tid`, `goods_unit`, `goods_cause`, `commission_rate`, `from_seller`) VALUES ()";


                    //Dictionary<string, int> attrid = new Dictionary<string, int>();

                    ////获取attribute表所需数据
                    //foreach (KeyValuePair<string, Dictionary<string, string>> kv in spec)
                    //{
                    //    string sval = "";
                    //    foreach (KeyValuePair<string, string> val in kv.Value)
                    //    {
                    //        sval += val.Key;
                    //    }
                    //    MySqlParameter[] parm = {
                    //              MySqlHelper.CreateInParam("@cat_id",     MySqlDbType.Int16,  20,     1       ),//待解决
                    //              MySqlHelper.CreateInParam("@attr_name",     MySqlDbType.VarChar,  100,     kv.Key   ),
                    //              MySqlHelper.CreateInParam("@attr_cat_type",  MySqlDbType.Int16,  20,     0         ),
                    //              MySqlHelper.CreateInParam("@attr_input_type",  MySqlDbType.Int16,  20,     1        ),
                    //              MySqlHelper.CreateInParam("@attr_type",  MySqlDbType.Int16,  20,     1       ),
                    //              MySqlHelper.CreateInParam("@attr_values",  MySqlDbType.VarChar,  1000,     sval       ),
                    //              MySqlHelper.CreateInParam("@color_values",  MySqlDbType.VarChar,1000,""       ),
                    //              MySqlHelper.CreateInParam("@attr_index",  MySqlDbType.Int16,20,0        ),
                    //              MySqlHelper.CreateInParam("@sort_order",   MySqlDbType.Int16,20,0       ),
                    //                MySqlHelper.CreateInParam("@is_linked",  MySqlDbType.Int16,20,0 ),
                    //                  MySqlHelper.CreateInParam("@attr_group",  MySqlDbType.Int16,20,0 ),
                    //                    MySqlHelper.CreateInParam("@attr_input_category", MySqlDbType.VarChar,20,"")
                    //              };

                    //    //attribute表添加规格属性
                    //    string sql = "INSERT INTO `dsc_attribute`( `cat_id`, `attr_name`, `attr_cat_type`, `attr_input_type`, `attr_type`, `attr_values`, `color_values`, `attr_index`, `sort_order`, `is_linked`, `attr_group`, `attr_input_category`) VALUES(@cat_id,@attr_name,@attr_cat_type,@attr_input_type,@attr_type,@attr_values,@color_values,@attr_index,@sort_order,@is_linked,@attr_group,@attr_input_category)";
                    //    int rowid = MySqlHelper.ExecuteNonQuery(strrote, CommandType.Text, sql, parm);
                    //    attrid.Add(kv.Key, rowid);



                    //sql = "INSERT INTO `dsc_goods_attr`( `goods_id`, `attr_id`, `attr_value`, `color_value`, `attr_price`, `attr_sort`, `attr_img_flie`, `attr_gallery_flie`, `attr_img_site`, `attr_checked`, `attr_value1`, `lang_flag`, `attr_img`, `attr_thumb`, `img_flag`, `attr_pid`, `admin_id`) VALUES(@goods_id,@attr_id,@attr_value,@color_value,@attr_price,@attr_sort,@attr_img_flie,@attr_gallery_flie,@attr_img_site,@attr_checked,@attr_value1,@lang_flag,@attr_img,@attr_thumb,@img_flag,@attr_pid,@admin_id)";

                    //foreach (KeyValuePair<string, string> val in kv.Value)
                    //{
                    //    string imgurl = "";
                    //    if (specval.ContainsKey(val.Key))
                    //    {
                    //        imgurl = specval[val.Key];
                    //    }
                    //    MySqlParameter[] parms = {
                    //          MySqlHelper.CreateInParam("@goods_id",     MySqlDbType.Int16,  20,     1       ),//待解决
                    //          MySqlHelper.CreateInParam("@attr_id",     MySqlDbType.Int16,  20,    rowid   ),
                    //          MySqlHelper.CreateInParam("@attr_value",  MySqlDbType.Text,  -1,     val.Key        ),
                    //          MySqlHelper.CreateInParam("@color_value",  MySqlDbType.Text, -1,     ""        ),
                    //          MySqlHelper.CreateInParam("@attr_price",  MySqlDbType.Double,  20,     1       ),
                    //          MySqlHelper.CreateInParam("@attr_sort",  MySqlDbType.Int16,  20,     0       ),
                    //          MySqlHelper.CreateInParam("@attr_img_flie",  MySqlDbType.VarChar,1000,imgurl    ),
                    //          MySqlHelper.CreateInParam("@attr_gallery_flie",  MySqlDbType.VarChar,1000,""       ),
                    //          MySqlHelper.CreateInParam("@attr_img_site",   MySqlDbType.VarChar,1000,""      ),
                    //          MySqlHelper.CreateInParam("@attr_checked",  MySqlDbType.Int16,20,0 ),
                    //          MySqlHelper.CreateInParam("@attr_value1",  MySqlDbType.Text,-1,"" ),
                    //          MySqlHelper.CreateInParam("@lang_flag", MySqlDbType.Int16,2,0),
                    //          MySqlHelper.CreateInParam("@attr_img", MySqlDbType.VarChar,1000,""),
                    //          MySqlHelper.CreateInParam("@attr_thumb", MySqlDbType.VarChar,1000,""),
                    //          MySqlHelper.CreateInParam("@img_flag", MySqlDbType.Int16,20,0),
                    //          MySqlHelper.CreateInParam("@attr_pid", MySqlDbType.VarChar,60,""),
                    //          MySqlHelper.CreateInParam("@admin_id", MySqlDbType.Int16,20,77),
                    //          };
                    //    MySqlHelper.ExecuteNonQuery(strrote, CommandType.Text, sql, parms);
                    //}
                }
                }
            //}
        }


        /// <summary>
        /// 商品规格处理
        /// </summary>
        /// <param name="attrlist">现有规格值</param>
        /// <param name="sm">新规格值</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> Goodsattr(Dictionary<string, string[]> attrlist, JObject sm)
        {
            //对比用
            Dictionary<string, Dictionary<string, string>> spec = new Dictionary<string, Dictionary<string, string>>();

            if (sm != null)
            {
                //获取所有规格值
                JToken mm = sm["skuList"];

                foreach (JToken j in mm.Children())
                {
                    JToken jc = j["attributeModelList"];
                    foreach (JToken jcc in jc)
                    {
                        string key = "";
                        string val = "";
                        if (jcc["sourceAttributeValue"] != null && jcc["sourceAttributeName"].ToString() != "")
                        {
                            key = jcc["sourceAttributeName"].ToString();
                            val = jcc["sourceAttributeValue"].ToString();
                        }
                        else if (jcc["targetAttributeValue"] != null && jcc["targetAttributeValue"].ToString() != "")
                        {
                            key = jcc["targetAttributeName"].ToString();
                            val = jcc["targetAttributeValue"].ToString();
                        }
                        //先进行数据库中规格值的对比，已存在就跳过
                        string[] vals = attrlist.ContainsKey(key)? attrlist[key]:null;
                        key = attrlist.ContainsKey(key) ? key + "_0" : key + "_1";
                        if (vals!=null&&vals.Contains(val))
                        {
                            continue;
                        }

                        if (spec.ContainsKey(key))
                        {
                            if (!spec[key].ContainsKey(val))
                            {
                                spec[key].Add(val, "");
                            }
                        }
                        else
                        {
                            Dictionary<string, string> lt = new Dictionary<string, string>();
                            lt.Add(val, "");
                            spec.Add(key, lt);
                        }
                    }
                }
            }
            return spec;
        }
    }
}
