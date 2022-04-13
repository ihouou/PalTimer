using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace HFrame.ENT
{
    /// <summary>
    /// 像JS的对象一样使用C#对象
    /// </summary>
    public class HObj : IEnumerable
    {
        /// <summary>
        /// 使用TypeOf获取值类型为“未定义”时的字符串
        /// </summary>
        public const string UNDEFINED = "undefined";
        private Dictionary<string, object> _dic;
        private List<object> _lst;
        private short type = -1;//0 dic , 1 list
        /// <summary>
        /// 仅初始化，不指定存储类型
        /// </summary>
        public HObj()
        {
        }
        /// <summary>
        /// 使用C#对象初始化，如果初始化对象为List或者Array，则存储类型为列表，否则为键值对存储类型
        /// </summary>
        /// <param name="obj">将该对象初始化为HObj</param>
        public HObj(object obj)
        {
            if (obj == null) return;

            if (obj is Dictionary<string, object>)
            {
                type = 0;
                _dic = new Dictionary<string, object>();
                if (obj != null)
                {
                    foreach (KeyValuePair<string, object> kv in (Dictionary<string, object>)obj)
                    {
                        _dic.Add(kv.Key, _ConvertData(kv.Value));
                    }
                }
            }
            else if (obj is ArrayList)
            {
                type = 1;
                _lst = new List<object>();
                if (obj != null)
                {
                    foreach (object o in (ArrayList)obj)
                    {
                        _lst.Add(_ConvertData(o));
                    }
                }
            }
            else if (obj is List<object>)
            {
                type = 1;
                _lst = new List<object>();
                foreach (var v in (List<object>)obj)
                {
                    _lst.Add(_ConvertData(v));
                }
            }
            else if (obj is object[])
            {
                type = 1;
                _lst = new List<object>();
                foreach (var v in (object[])obj)
                {
                    _lst.Add(_ConvertData(v));
                }
            }
            else
            {
                type = 0;
                _dic = new Dictionary<string, object>();
                CopyDataFrom(obj);
            }
        }
        /// <summary>
        /// 使用json初始化，如果json为Array，则存储类型为列表，否则为键值对存储类型
        /// </summary>
        /// <param name="json">合法的json字符串</param>
        public HObj(string json)
        {
            json = json.Trim();
            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                type = 1;
                _lst = new List<object>();
                //array
                JavaScriptSerializer s = new JavaScriptSerializer();
                List<object> lst = s.Deserialize<List<object>>(json);
                foreach (object ss in lst)
                {
                    _lst.Add(_ConvertData(ss));
                }
            }
            else if (json.StartsWith("{") && json.EndsWith("}"))
            {
                type = 0;
                _dic = new Dictionary<string, object>();
                //object
                JavaScriptSerializer s = new JavaScriptSerializer();

                Dictionary<string, object> dic = s.Deserialize<Dictionary<string, object>>(json);
                foreach (KeyValuePair<string, object> kv in dic)
                {
                    _dic.Add(kv.Key, _ConvertData(kv.Value));
                }
            }
        }
        /// <summary>
        /// 使用DataRow初始化为键值对存储类型
        /// </summary>
        /// <param name="dr">DataRow必须要有合法的表头</param>
        public HObj(DataRow dr)
        {
            type = 0;
            _dic = new Dictionary<string, object>();
            if (dr != null)
            {
                for (int i = 0; i < dr.Table.Columns.Count; ++i)
                {
                    string k = dr.Table.Columns[i].ColumnName;
                    if (_dic.ContainsKey(k))
                    {
                        _dic[k] = dr[k];
                    }
                    else
                    {
                        _dic.Add(k, dr[k]);
                    }
                }
            }
        }
        /// <summary>
        /// 工具函数：判断一个字符串是不是json格式（Array或者Object）
        /// </summary>
        /// <param name="str">需要判断的字符串</param>
        /// <returns></returns>
        public static bool IsJson(string str)
        {
            if ((str.StartsWith("[") && str.EndsWith("]")) || (str.StartsWith("{") && str.EndsWith("}")))
            {
                return true;
            }
            return false;
        }

        private static object _ConvertData(object ori)
        {
            if (ori != null)
            {
                if (ori is string)
                {
                    if (IsJson((string)ori))
                    {
                        return new HObj((string)ori);
                    }
                    else
                    {
                        return ori;
                    }
                }
                else if (ori is short || ori is int || ori is long || ori is float || ori is double || ori is decimal)
                {
                    return ori;
                }
                else if (ori is bool)
                {
                    return ori;
                }
                else if (ori is Dictionary<string, object> || ori is ArrayList)
                {
                    return new HObj(ori);
                }
                else
                {
                    return new HObj(ori);
                }
            }
            return null;
        }
        /// <summary>
        /// 无存储类型时返回0，有存储类型时返回真实的元素个数
        /// </summary>
        public int Count
        {
            get
            {
                if (type == 0) return _dic.Count;
                if (type == 1) return _lst.Count;
                return 0;
            }
        }
        /// <summary>
        /// 获取或者设置存储类型为列表的元素，获取时，若存储类型不匹配或者找不到值时，返回null；设置时，若无存储类型，则初始化为列表类型。
        /// </summary>
        /// <param name="index">元素位置</param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (_lst == null) return null;
                if (index >= 0 && index < _lst.Count)
                {
                    return _lst[index];
                }
                return null;
            }
            set
            {
                if (type == -1)
                {
                    type = 1;
                    _lst = new List<object>();
                    _lst.Add(value);
                }
                else if (type == 1)
                {
                    if (index >= 0 && index < _lst.Count)
                    {
                        _lst[index] = value;
                    }
                    else if (index >= _lst.Count)
                    {
                        _lst.Add(value);
                    }
                }
            }
        }
        /// <summary>
        /// 获取或者设置存储类型为键值对的元素，获取时，若存储类型不匹配或者找不到值时，返回null；设置时，若无存储类型，则初始化为键值对类型。
        /// </summary>
        /// <param name="attr">属性名称</param>
        /// <returns></returns>
        public object this[string attr]
        {
            get
            {
                if (_dic == null) return null;
                if (_dic.ContainsKey(attr))
                {
                    return _dic[attr];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (type == -1)
                {
                    type = 0;
                    _dic = new Dictionary<string, object>();
                    _dic.Add(attr, value);
                }
                else if (type == 0)
                {
                    if (_dic.ContainsKey(attr))
                    {
                        _dic[attr] = value;
                    }
                    else
                    {
                        _dic.Add(attr, value);
                    }
                }
            }
        }
        /// <summary>
        /// 添加元素，只适用于列表类型和无类型
        /// </summary>
        /// <param name="obj">要存储的对象</param>
        public void Add(object obj)
        {
            if (type == 1)
            {
                _lst.Add(obj);
            }
            else if (type == -1)
            {
                type = 1;
                _lst = new List<object>();
                _lst.Add(obj);
            }
        }
        /// <summary>
        /// 向指定位置添加元素，只适用于列表类型
        /// </summary>
        /// <param name="indexAt">位置</param>
        /// <param name="obj">要存储的对象</param>
        public void Insert(int indexAt, object obj)
        {
            if (_lst == null) return;
            if (indexAt <= 0)
            {
                _lst.Insert(0, obj);
            }
            else if (indexAt >= _lst.Count)
            {
                _lst.Add(obj);
            }
            else
            {
                _lst.Insert(indexAt, obj);
            }
        }
        /// <summary>
        /// 移除指定位置的元素，只适用于列表类型
        /// </summary>
        /// <param name="index">位置</param>
        public void Remove(int index)
        {
            if (type == 1)
            {
                if (index >= 0 && index < _lst.Count)
                {
                    _lst.RemoveAt(index);
                }
            }
        }
        /// <summary>
        /// 移除指定键元素，只适用于键值对类型
        /// </summary>
        /// <param name="attr">属性</param>
        public void Remove(string attr)
        {
            if (type == 0)
            {
                if (_dic.ContainsKey(attr))
                {
                    _dic.Remove(attr);
                }
            }
        }
        /// <summary>
        /// 获取指定键的元素并转换类型，若不是键值对类型，则返回指定类型默认值
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="attr">属性</param>
        /// <returns></returns>
        public T GetValue<T>(string attr)
        {
            /*if (_dic == null) return default(T);
            if (_dic[attr] is int && typeof(T) == typeof(short))
            {
                return (T)Convert.ChangeType(_dic[attr], typeof(T));
            }
            else if (_dic[attr] is int && typeof(T) == typeof(long))
            {
                return (T)Convert.ChangeType(_dic[attr], typeof(T));
            }*/
            if (_dic == null) return default(T);
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(_dic[attr], typeof(T));
            }

            if ((_dic[attr] is int || _dic[attr] is short || _dic[attr] is long || _dic[attr] is float || _dic[attr] is double || _dic[attr] is decimal) && (typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(long) || typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal)))
            {
                return (T)Convert.ChangeType(_dic[attr], typeof(T));
            }

            if (_dic.ContainsKey(attr) && (_dic[attr] is T))
            {
                return (T)(_dic[attr]);
            }
            else if (_dic.ContainsKey(attr) && (_dic[attr] is object))
            {
                return (T)Convert.ChangeType(_dic[attr], typeof(T));
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// 获取指定位置的元素并转换类型，若不是列表类型，则返回指定类型默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetValue<T>(int index)
        {
            if (_lst == null) return default(T);

            /*if (_lst[index] is int && typeof(T) == typeof(short))
            {
                return (T)Convert.ChangeType(_lst[index], typeof(T));
            }
            else if (_lst[index] is int && typeof(T) == typeof(long))
            {
                return (T)Convert.ChangeType(_lst[index], typeof(T));
            }*/

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(_lst[index], typeof(T));
            }

            if ((_lst[index] is int || _lst[index] is short || _lst[index] is long || _lst[index] is float || _lst[index] is double || _lst[index] is decimal) && (typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(long) || typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal)))
            {
                return (T)Convert.ChangeType(_lst[index], typeof(T));
            }

            if (index >= 0 && index < _lst.Count && (_lst[index] is T))
            {
                return (T)(_lst[index]);
            }
            else if (index >= 0 && index < _lst.Count && (_lst[index] is object))
            {
                return (T)Convert.ChangeType(_lst[index], typeof(T));
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// 转换为json
        /// </summary>
        /// <param name="IsLocalTime">是否将时间对象转换为当前时区的时间</param>
        /// <returns></returns>
        public string ToJson(bool IsLocalTime = false)
        {
            if (type == 0)
            {
                string res = "{";
                foreach (KeyValuePair<string, object> kv in _dic)
                {
                    res += "\"" + kv.Key + "\":";
                    if (kv.Value is HObj)
                    {
                        res += ((HObj)(kv.Value)).ToJson(IsLocalTime) + ",";
                    }
                    else if (kv.Value is string || kv.Value is char)
                    {
                        res += "\"" + kv.Value.ToString() + "\",";
                    }
                    else if (kv.Value is int || kv.Value is long || kv.Value is short || kv.Value is float || kv.Value is double || kv.Value is decimal)
                    {
                        res += kv.Value.ToString() + ",";
                    }
                    else if (kv.Value is bool)
                    {
                        if ((bool)(kv.Value))
                        {
                            res += "true,";
                        }
                        else
                        {
                            res += "false,";
                        }
                    }
                    else
                    {
                        JavaScriptSerializer s = new JavaScriptSerializer();
                        res += s.Serialize(kv.Value) + ",";
                    }
                }
                if (res.EndsWith(","))
                {
                    res = res.Substring(0, res.Length - 1);
                }
                res += "}";
                if (IsLocalTime)
                {
                    //将时间格式转换为适合阅读习惯的格式
                    res = System.Text.RegularExpressions.Regex.Replace(res, @"\\/Date\((\d+)\)\\/", match =>
                    {
                        DateTime dt = new DateTime(1970, 1, 1);
                        dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                        dt = dt.ToLocalTime(); //本地时间
                        return dt.ToString(); ;
                    });
                }
                return res;
            }
            else if (type == 1)
            {
                string res = "[";
                foreach (var v in _lst)
                {
                    if (v is HObj)
                    {
                        res += ((HObj)v).ToJson(IsLocalTime) + ",";
                    }
                    else if (v is string || v is char)
                    {
                        res += "\"" + v.ToString() + "\",";
                    }
                    else if (v is int || v is long || v is short || v is float || v is double || v is decimal)
                    {
                        res += v.ToString() + ",";
                    }
                    else if (v is bool)
                    {
                        if ((bool)v)
                        {
                            res += "true,";
                        }
                        else
                        {
                            res += "false,";
                        }
                    }
                    else
                    {
                        JavaScriptSerializer s = new JavaScriptSerializer();
                        res += s.Serialize(v) + ",";
                    }
                }
                if (res.EndsWith(","))
                {
                    res = res.Substring(0, res.Length - 1);
                }
                res += "]";
                if (IsLocalTime)
                {
                    //将时间格式转换为适合阅读习惯的格式
                    res = System.Text.RegularExpressions.Regex.Replace(res, @"\\/Date\((\d+)\)\\/", match =>
                    {
                        DateTime dt = new DateTime(1970, 1, 1);
                        dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                        dt = dt.ToLocalTime(); //本地时间
                        return dt.ToString(); ;
                    });
                }
                return res;
            }
            else
            {
                return "{}";
            }
        }

        public IEnumerator GetEnumerator()
        {
            if (this._dic != null)
            {
                return this._dic.GetEnumerator();
            }
            else if (this._lst != null)
            {
                return this._lst.GetEnumerator();
            }
            return null;
        }

        public T ToEntity<T>() where T : new()
        {
            var ent = new T();
            if (this._dic != null)
            {
                CopyDataTo(ent);
            }
            return ent;
        }
        public List<object> ToList()
        {
            return _lst;
        }
        public Dictionary<string, object> ToDic()
        {
            return _dic;
        }
        public string TypeOf(string attr)
        {
            if (_dic != null)
            {
                if (_dic.ContainsKey(attr))
                {
                    return _dic[attr].GetType().ToString();
                }
                else
                {
                    return UNDEFINED;
                }
            }
            else
            {
                return UNDEFINED;
            }
        }
        public bool ExistAttrs(params string[] attrs)
        {
            if (_dic == null) return false;
            foreach (string a in attrs)
            {
                if (!_dic.ContainsKey(a))
                {
                    return false;
                }
            }
            return true;
        }
        public bool ExistAttrs(out string info, params string[] attrs)
        {
            info = "";
            if (_dic == null) return false;
            foreach (string a in attrs)
            {
                if (a.Contains("|"))
                {
                    string[] spli = a.Split('|');
                    if (!_dic.ContainsKey(spli[0]))
                    {
                        info = spli[1];
                        return false;
                    }
                }
                else
                {
                    if (!_dic.ContainsKey(a))
                    {
                        info = a;
                        return false;
                    }
                }
            }
            return true;
        }
        public bool ExistAttrs(object ent, out string info)
        {
            info = "";
            if (_dic == null) return false;
            foreach (PropertyInfo p in ent.GetType().GetProperties())
            {
                if (!_dic.ContainsKey(p.Name))
                {
                    info = p.Name;
                    return false;
                }
            }
            return true;
        }
        public bool ExistAttrs(object ent)
        {
            string info = "";
            return ExistAttrs(ent, out info);
        }
        public bool HasValue(string attr)
        {
            if (_dic == null) return false;
            if (_dic.ContainsKey(attr))
            {
                if (_dic[attr] == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public bool HasValue(int index)
        {
            if (_lst == null) return false;
            if (index >= 0 && index < _lst.Count)
            {
                if (_lst[index] == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public void SetDatas(HObj data)
        {
            if (data != null)
            {
                if (data.type == 0)//dic
                {
                    foreach (KeyValuePair<string, object> kv in data._dic)
                    {
                        if (kv.Value is HObj)
                        {
                            if (this.HasValue(kv.Key))
                            {
                                if (this[kv.Key] is HObj)
                                {
                                    ((HObj)(this[kv.Key])).SetDatas((HObj)kv.Value);
                                }
                            }
                            else
                            {
                                HObj h = new HObj();
                                h.SetDatas((HObj)kv.Value);
                                this[kv.Key] = h;
                            }
                        }
                        else
                        {
                            this[kv.Key] = kv.Value;
                        }
                    }
                }
                else//list
                {
                }
            }
        }

        public void CopyDataTo(object ent)
        {
            if (_dic != null)
            {
                foreach (PropertyInfo p in ent.GetType().GetProperties())
                {
                    if (_dic.ContainsKey(p.Name))
                    {
                        //throw new Exception((p.PropertyType==typeof(string)).ToString());
                        if (p.PropertyType == typeof(int) || p.PropertyType == typeof(short) || p.PropertyType == typeof(long) || p.PropertyType == typeof(float) || p.PropertyType == typeof(double) || p.PropertyType == typeof(decimal) || p.PropertyType == typeof(bool))
                        {
                            p.SetValue(ent, ConvertObject(Convert.ChangeType(_dic[p.Name], p.PropertyType), p.PropertyType), null);
                        }
                        else
                        {
                            p.SetValue(ent, ConvertObject(_dic[p.Name], p.PropertyType), null);
                        }
                    }
                }
            }
        }
        public void CopyDataFrom(object ent)
        {
            if (type == -1)
            {
                type = 0;
                _dic = new Dictionary<string, object>();
            }
            if (type == 0)
            {
                foreach (PropertyInfo p in ent.GetType().GetProperties())
                {
                    if (_dic.ContainsKey(p.Name))
                    {
                        _dic[p.Name] = p.GetValue(ent, null);
                    }
                    else
                    {
                        _dic.Add(p.Name, p.GetValue(ent, null));
                    }
                }
            }
        }

        public static object ConvertObject(object obj, Type type)
        {
            if (type == null) return obj;
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType())) // 如果待转换对象的类型与目标类型兼容，则无需转换
            {
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum) // 如果待转换的对象的基类型为枚举
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString())) // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值
                {
                    return null;
                }
                else
                {
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
                }
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return converter.ConvertFrom(obj);
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ConvertObject(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }
    }
}
