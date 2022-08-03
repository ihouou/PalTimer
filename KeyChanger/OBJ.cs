using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace KeyChanger
{

    public class OBJ : IEnumerable
    {
        public const string UNDEFINED = "undefined";
        public const string OBJECT = "object";
        public const string ARRAY = "array";
        public const string NUMBER = "number";
        public const string STRING = "string";
        public const string BOOLEAN = "boolean";
        public const string DATETIME = "datetime";
        public const string ENUM = "enum";
        public const string NULL = "null";

        public enum TYPE
        {
            Object,
            Array,
            Null = -1,
            Undefined = -2
        }

        private const int tOO = 0;
        private const int tNumber = 1;
        private const int tString = 2;
        private const int tBoolean = 3;
        private const int tDateTime = 4;
        private const int tEnum = 5;
        private const int tNull = -1;
        private const int tUndefined = -2;

        private TYPE _type = TYPE.Undefined;
        public TYPE Type
        {
            get
            {
                return _type;
            }
        }
        private List<object> lst;
        private List<int> tlst;
        private Dictionary<string, object> dic;
        private Dictionary<string, int> tdic;

        private void initForList()
        {
            _type = TYPE.Array;
            lst = new List<object>();
            tlst = new List<int>();
        }
        private void initForDic()
        {
            _type = TYPE.Object;
            dic = new Dictionary<string, object>();
            tdic = new Dictionary<string, int>();
        }

        private void e(int level, string msg)
        {
            //throw new Exception(msg);
        }
        /// <summary>
        /// 检测一个字符串是不是json
        /// </summary>
        /// <param name="str">要检测的字符串</param>
        /// <returns>是否为json</returns>
        public static bool IsJson(string str)
        {
            str = str.Trim();
            if (str == UNDEFINED) return true;
            if (str == "null") return true;
            if (str.StartsWith("[") && str.EndsWith("]")) return true;
            if (str.StartsWith("{") && str.EndsWith("}")) return true;
            return false;
        }
        /// <summary>
        /// 获取Object类型对象的所有属性名
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object>.KeyCollection GetAllKey()
        {
            if (_type == TYPE.Object)
            {
                return dic.Keys;
            }
            e(2, "Not a Dictionary.");
            return null;
        }
        /// <summary>
        /// 检查一个对象是不是C#的基础类型
        /// </summary>
        /// <param name="any">任何对象</param>
        /// <returns>是否为基础类型</returns>
        public static bool IsCSBaseType(object any)
        {
            Type type = any.GetType();
            return IsCSBaseType(type);
        }
        /// <summary>
        /// 检查一个类型是不是C#的基础类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否为基础类型</returns>
        public static bool IsCSBaseType(Type type)
        {
            if (type.IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(DateTime)))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 初始化一个空对象
        /// </summary>
        /// <param name="t">对象类型</param>
        public OBJ(TYPE t)
        {
            if (t == TYPE.Array)
            {
                initForList();
            }
            else if (t == TYPE.Object)
            {
                initForDic();
            }
        }
        /// <summary>
        /// 使用JSON字符串初始化对象
        /// </summary>
        /// <param name="json"></param>
        public OBJ(string json)
        {
            json = json.Trim();
            if (!IsJson(json))
            {
                e(0, "Not a Json string.");
                return;
            }
            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                //array
                initForList();
                CopyFromJson(json);
            }
            else if (json.StartsWith("{") && json.EndsWith("}"))
            {
                //object
                initForDic();
                CopyFromJson(json);
            }
            else if (json == "null")
            {
                _type = TYPE.Null;
            }
            else if (json == UNDEFINED)
            {
                _type = TYPE.Undefined;
            }
        }
        /// <summary>
        /// 用于Json递归生成对象
        /// </summary>
        /// <param name="j2"></param>
        /// <param name="strmap"></param>
        private OBJ(string j2, Dictionary<string, string> strmap)
        {
            j2 = j2.Trim();
            if (!IsJson(j2))
            {
                e(0, "Not a Json string.");
                return;
            }
            if (j2.StartsWith("[") && j2.EndsWith("]"))
            {
                //array
                initForList();
                copy_from_json2(j2, strmap);
            }
            else if (j2.StartsWith("{") && j2.EndsWith("}"))
            {
                //object
                initForDic();
                copy_from_json2(j2, strmap);
            }
            else if (j2 == "null")
            {
                _type = TYPE.Null;
            }
            else if (j2 == UNDEFINED)
            {
                _type = TYPE.Undefined;
            }
        }
        /// <summary>
        /// 从Json里拷贝数据
        /// </summary>
        /// <param name="json">Json字符串</param>
        public void CopyFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                e(0, "Json string is null.");
                return;
            }
            json = json.Trim();
            if (!IsJson(json))
            {
                e(1, "Not a Json string.");
                return;
            }
            if (json == "null" || json == UNDEFINED) return;
            Dictionary<string, string> strmap = new Dictionary<string, string>();
            json = pre_json2_string(json, strmap);
            copy_from_json2(json, strmap);
        }
        /// <summary>
        /// 将Json里的字符串全部转义，并缓存转义前后的map
        /// </summary>
        /// <param name="json"></param>
        /// <param name="strmap"></param>
        /// <returns></returns>
        private string pre_json2_string(string json, Dictionary<string, string> strmap)
        {
            StringBuilder sb = new StringBuilder();
            bool zy = false;
            int idx = 0;
            bool begin = false;
            char nx = '0';
            string tmp = "";
            foreach (char c in json)
            {
                if (!begin)
                {
                    switch (c)
                    {
                        case '"':
                            begin = true;
                            nx = c;
                            break;
                        case '\'':
                            begin = true;
                            nx = c;
                            break;
                        case ' ':
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
                else
                {
                    if (zy)
                    {
                        zy = false;
                        tmp += c;
                        continue;
                    }
                    if (c == '\\')
                    {
                        tmp += c;
                        zy = true;
                        continue;
                    }
                    if (c == nx)
                    {
                        string key = "$" + idx;
                        idx++;
                        sb.Append(key);
                        strmap.Add(key, tmp);
                        tmp = "";
                        begin = false;
                    }
                    else
                    {
                        tmp += c;
                    }
                }

            }

            if (begin || zy || tmp != "")
            {
                e(0, "Json format error.");
            }
            return sb.ToString().Replace("\r","").Replace("\n","").Replace("\t", "");
        }
        private void copy_from_json2(string j2, Dictionary<string, string> strmap)
        {
            if (j2.StartsWith("[") && j2.EndsWith("]"))
            {
                //array
                if (_type != TYPE.Array)
                {
                    e(1, "Cannot copy from Json(Array) to different type.");
                    return;
                }
                j2 = j2.Substring(1, j2.Length - 2);
                string tmp = "";
                int lc = 0;
                foreach (char c in j2)
                {
                    switch (c)
                    {
                        case ',':
                            if (lc <= 0)
                            {
                                if (tmp == UNDEFINED)
                                {
                                    lst.Add(null);
                                    tlst.Add(tUndefined);
                                }
                                else
                                {
                                    push(anaj2obj(tmp, strmap));
                                }
                                tmp = "";
                            }
                            else
                            {
                                tmp += c;
                            }
                            break;
                        case '[':
                            lc++;
                            tmp += c;
                            break;
                        case '{':
                            lc++;
                            tmp += c;
                            break;
                        case ']':
                            lc--;
                            tmp += c;
                            break;
                        case '}':
                            lc--;
                            tmp += c;
                            break;
                        default:
                            tmp += c;
                            break;
                    }
                }
                if (tmp != "")
                {
                    if (tmp == UNDEFINED)
                    {
                        lst.Add(null);
                        tlst.Add(tUndefined);
                    }
                    else
                    {
                        push(anaj2obj(tmp, strmap));
                    }
                    tmp = "";
                }
                if (lc > 0)
                {
                    e(1, "Bad Json format.");
                    return;
                }
            }
            else if (j2.StartsWith("{") && j2.EndsWith("}"))
            {
                //object
                if (_type != TYPE.Object)
                {
                    e(1, "Cannot copy from Json(Object) to different type.");
                    return;
                }
                j2 = j2.Substring(1, j2.Length - 2);
                string[] tmp = new string[2] { "", "" };
                int f = 0;
                int lc = 0;
                foreach (char c in j2)
                {
                    switch (c)
                    {
                        case ':':
                            if (lc <= 0)
                            {
                                f = 1;
                            }
                            else
                            {
                                tmp[f] += c;
                            }
                            break;
                        case ',':
                            if (lc <= 0)
                            {
                                if (f == 1 && tmp[0].StartsWith("$"))
                                {
                                    if (tmp[1] == UNDEFINED)
                                    {
                                        set(strmap[tmp[0]], null, true);
                                    }
                                    else
                                    {
                                        set(strmap[tmp[0]], anaj2obj(tmp[1], strmap));
                                    }
                                }
                                else
                                {
                                    e(1, "Bad Json format.");
                                    return;
                                }
                                f = 0;
                                tmp[0] = "";
                                tmp[1] = "";
                            }
                            else
                            {
                                tmp[f] += c;
                            }
                            break;
                        case '[':
                            lc++;
                            tmp[f] += c;
                            break;
                        case '{':
                            lc++;
                            tmp[f] += c;
                            break;
                        case ']':
                            lc--;
                            tmp[f] += c;
                            break;
                        case '}':
                            lc--;
                            tmp[f] += c;
                            break;
                        default:
                            tmp[f] += c;
                            break;
                    }
                }
                if (tmp[0].StartsWith("$"))
                {
                    if (tmp[1] == UNDEFINED)
                    {
                        set(strmap[tmp[0]], null, true);
                    }
                    else
                    {
                        set(strmap[tmp[0]], anaj2obj(tmp[1], strmap));
                    }
                }
                if (lc > 0)
                {
                    e(1, "Bad Json format.");
                    return;
                }
            }
        }
        private object anaj2obj(string s, Dictionary<string, string> strmap)
        {
            if (s == "null" || s == "") return null;
            if (s.StartsWith("$")) return strmap[s];
            if (s.StartsWith("[") || s.StartsWith("{")) return new OBJ(s, strmap);
            if (s == "true" || s == "false") return (s == "true");
            if (s.IndexOf('.') > 0)
            {
                double d = 0.0D;
                if (double.TryParse(s, out d))
                {
                    return d;
                }
                else
                {
                    e(2, "Bad Json element format.");
                    return d;
                }
            }
            else
            {
                long l = 0L;
                if (long.TryParse(s, out l))
                {
                    return l;
                }
                else
                {
                    e(2, "Bad Json element format.");
                    return l;
                }
            }
        }
        /// <summary>
        /// 使用C#实例初始化对象
        /// </summary>
        /// <param name="obj">C#实例</param>
        public OBJ(object obj)
        {
            if (obj == null)
            {
                _type = TYPE.Null;
                return;
            }
            if (IsCSBaseType(obj))
            {
                e(1, "Cannot parse the base type object.");
                return;
            }
            if (obj is object[])
            {
                initForList();
            }
            else if (obj is IList)
            {
                initForList();
            }
            else if (obj is IDictionary)
            {
                initForDic();
            }
            else if (obj is DataRow)
            {
                initForDic();
            }
            else if (obj is DataTable)
            {
                initForList();
            }
            else if (obj is OBJ)
            {
                OBJ t = obj as OBJ;
                if (t._type == TYPE.Array)
                {
                    initForList();
                }
                else if (t._type == TYPE.Object)
                {
                    initForDic();
                }
                else
                {
                    _type = t._type;
                }
            }
            else
            {
                initForDic();
            }
            CopyFromObject(obj);
        }

        private int getttype(object o)
        {
            if (o == null) return tNull;
            if (IsCSBaseType(o))
            {
                if (o is bool)
                {
                    return tBoolean;
                }
                else if (o is string)
                {
                    return tString;
                }
                else if (o is DateTime)
                {
                    return tDateTime;
                }
                else if (o is Enum)
                {
                    return tEnum;
                }
                else
                {
                    return tNumber;
                }
            }
            else
            {
                return tOO;
            }
        }

        private void push(object o, int idx = -1, bool insert = false)
        {
            int tp = tNull;
            object pend = null;
            if (o != null)
            {
                if (IsCSBaseType(o))
                {
                    tp = getttype(o);
                    switch (tp)
                    {
                        case tDateTime:
                            pend = ((DateTime)o).Ticks;
                            break;
                        case tEnum:
                            pend = (int)o;
                            break;
                        default:
                            pend = o;
                            break;
                    }
                }
                else
                {
                    tp = tOO;
                    if (o is OBJ)
                    {
                        pend = o;
                    }
                    else
                    {
                        pend = new OBJ(o);
                    }
                }
            }

            if (insert)
            {
                if (lst.Count <= 0 || idx >= lst.Count)
                {
                    lst.Add(pend);
                    tlst.Add(tp);
                    return;
                }
                if (idx < 0) idx = 0;
                lst.Insert(idx, pend);
                tlst.Insert(idx, tp);
            }
            else
            {
                if (idx < 0 || idx >= lst.Count)
                {
                    //add
                    lst.Add(pend);
                    tlst.Add(tp);
                }
                else
                {
                    //modify
                    lst[idx] = pend;
                    tlst[idx] = tp;
                }
            }
        }
        private void set(string key, object o, bool isundef = false)
        {
            object pend = null;
            int tp = tUndefined;
            if (!isundef)
            {
                tp = tNull;
                if (o != null)
                {
                    tp = getttype(o);
                    switch (tp)
                    {
                        case tNumber:
                            pend = o;
                            break;
                        case tString:
                            pend = o;
                            break;
                        case tDateTime:
                            pend = ((DateTime)o).Ticks;
                            break;
                        case tEnum:
                            pend = (int)o;
                            break;
                        case tBoolean:
                            pend = o;
                            break;
                        default:
                            if (o is OBJ)
                            {
                                pend = o;
                            }
                            else
                            {
                                pend = new OBJ(o);
                            }
                            break;
                    }
                }
            }
            if (dic.ContainsKey(key))
            {
                dic[key] = pend;
                tdic[key] = tp;
            }
            else
            {
                dic.Add(key, pend);
                tdic.Add(key, tp);
            }
        }
        private string pull_json(int idx)
        {
            if (_type != TYPE.Array)
            {
                e(1, "Not Array.");
                return "\"\"";
            }
            if (lst.Count <= 0)
            {
                e(1, "Empty Array");
                return "\"\"";
            }
            if (idx < 0 || idx >= lst.Count)
            {
                e(1, "Index out of range.");
                return "\"\"";
            }
            int t = tlst[idx];
            switch (t)
            {
                case tString:
                    return "\"" + (lst[idx] as string) + "\"";
                case tBoolean:
                    return (((bool)lst[idx]) ? "true" : "false");
                case tDateTime:
                    return "\"" + (new DateTime((long)lst[idx])).ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"";
                case tNumber:
                    return lst[idx].ToString();
                case tEnum:
                    return lst[idx].ToString();
                case tNull:
                    return "null";
                case tUndefined:
                    return UNDEFINED;
                case tOO:
                    return (lst[idx] as OBJ).ToJson();
            }
            e(2, "Unknown value type.");
            return "\"\"";
        }
        private string get_json(string key)
        {
            if (_type != TYPE.Object)
            {
                e(1, "Not Dictionary.");
                return "\"\"";
            }
            if (!dic.ContainsKey(key))
            {
                e(1, "Key not exist.");
                return "\"\"";
            }
            int t = tdic[key];
            switch (t)
            {
                case tString:
                    return "\"" + (dic[key] as string) + "\"";
                case tBoolean:
                    return (((bool)dic[key]) ? "true" : "false");
                case tDateTime:
                    return "\"" + (new DateTime((long)dic[key])).ToString("yyyy-MM-dd HH:mm:ss.fff") + "\"";
                case tNumber:
                    return dic[key].ToString();
                case tEnum:
                    return dic[key].ToString();
                case tNull:
                    return "null";
                case tUndefined:
                    return UNDEFINED;
                case tOO:
                    return (dic[key] as OBJ).ToJson();
            }
            e(2, "Unknown value type.");
            return "\"\"";
        }
        public string ToJson()
        {
            if (_type == TYPE.Null)
            {
                return "null";
            }
            else if (_type == TYPE.Undefined)
            {
                return UNDEFINED;
            }
            StringBuilder sb = new StringBuilder();
            if (_type == TYPE.Array)
            {
                sb.Append('[');
                for (int i = 0; i < lst.Count; ++i)
                {
                    sb.Append(pull_json(i));
                    if (i < lst.Count - 1)
                    {
                        sb.Append(',');
                    }
                }
                sb.Append(']');
            }
            else if (_type == TYPE.Object)
            {
                sb.Append('{');
                int i = 0;
                foreach (KeyValuePair<string, object> kv in dic)
                {
                    sb.Append("\"" + kv.Key + "\":");
                    sb.Append(get_json(kv.Key));
                    if (i < dic.Count - 1)
                    {
                        sb.Append(',');
                    }
                    i++;
                }
                sb.Append('}');
            }

            return sb.ToString();
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns>Array类型的会返回List的迭代器，Object类型的会返回KeyValuePair迭代器，其他类型返回null</returns>
        public IEnumerator GetEnumerator()
        {
            if (this.dic != null)
            {
                return this.dic.GetEnumerator();
            }
            else if (this.lst != null)
            {
                return this.lst.GetEnumerator();
            }
            return null;
        }
        /// <summary>
        /// 元素的个数，类型为null或undefined的时候只返回0
        /// </summary>
        public int Count
        {
            get
            {
                if (_type == TYPE.Array) return lst.Count;
                if (_type == TYPE.Object) return dic.Count;
                return 0;
            }
        }
        /// <summary>
        /// 获取或者设置存储类型为列表的元素；设置时如果序号不在数组范围内，则会添加新元素。
        /// </summary>
        /// <param name="index">元素序号</param>
        /// <returns>若存储类型不匹配或者找不到值时，返回null</returns>
        public object this[int index]
        {
            get
            {
                if (_type != TYPE.Array) return null;
                if (index >= 0 && index < lst.Count)
                {
                    return lst[index];
                }
                return null;
            }
            set
            {
                if (_type == TYPE.Array)
                {
                    if (index >= 0 && index < lst.Count)
                    {
                        push(value, index);
                    }
                    else if (index >= lst.Count)
                    {
                        push(value);
                    }
                }
                else
                {
                    e(1, "Not an Array,cannot set item.");
                }
            }
        }
        /// <summary>
        /// 获取或者设置存储类型为键值对的元素；设置时如果不存在该属性，则会添加新元素。
        /// </summary>
        /// <param name="attr"></param>
        /// <returns>若存储类型不匹配或者找不到值时，返回null</returns>
        public object this[string attr]
        {
            get
            {
                if (_type != TYPE.Object) return null;
                if (dic.ContainsKey(attr))
                {
                    return dic[attr];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_type == TYPE.Object)
                {
                    set(attr, value);
                }
                else
                {
                    e(1, "Not an Dictionary,cannot set item.");
                }
            }
        }
        /// <summary>
        /// 为Array类型的对象添加元素
        /// </summary>
        /// <param name="o">需要添加的元素</param>
        public void Add(object o)
        {
            if (_type == TYPE.Array)
            {
                push(o);
            }
            else
            {
                e(1, "Cannot add item to not Array.");
            }
        }
        /// <summary>
        /// 为Array类型的对象插入元素
        /// </summary>
        /// <param name="indexAt">插入元素的位置</param>
        /// <param name="o">需要插入的元素</param>
        public void Insert(int indexAt, object o)
        {
            if (_type == TYPE.Array)
            {
                push(o, indexAt, true);
            }
            else
            {
                e(1, "Cannot insert item to not Array.");
            }
        }
        /// <summary>
        /// 移除Array类型对象中的指定位置的元素
        /// </summary>
        /// <param name="index">元素位置</param>
        public void Remove(int index)
        {
            if (_type == TYPE.Array)
            {
                if (index >= 0 && index < lst.Count)
                {
                    lst.RemoveAt(index);
                    tlst.RemoveAt(index);
                }
                else
                {
                    e(3, "Remove out of range item.");
                }
            }
            else
            {
                e(2, "Cannot remove item from not Array.");
            }
        }
        /// <summary>
        /// 移除Object类型对象中的属性
        /// </summary>
        /// <param name="attr">属性名</param>
        public void Remove(string attr)
        {
            if (_type == TYPE.Object)
            {
                if (dic.ContainsKey(attr))
                {
                    dic.Remove(attr);
                    tdic.Remove(attr);
                }
                else
                {
                    e(3, "Remove an unexist item.");
                }
            }
            else
            {
                e(2, "Cannot remove item from not Dictionary.");
            }
        }
        /// <summary>
        /// 从Array中获取指定位置的元素，并转换为想要的类型
        /// </summary>
        /// <typeparam name="T">输出类型</typeparam>
        /// <param name="index">位置</param>
        /// <returns>如果无法索引到元素或者类型无法转换，可能会获取到T的默认值</returns>
        public T GetValue<T>(int index)
        {
            if (_type == TYPE.Array)
            {
                if (index >= 0 && index < lst.Count)
                {
                    int tp = tlst[index];
                    object o = lst[index];
                    return vconvert<T>(tp, o);
                }
                else
                {
                    e(1, "Index out of range");
                }
            }
            else
            {
                e(2, "Cannot use index get item from not Array.");
            }
            return default(T);
        }
        /// <summary>
        /// 从Object中获取指定属性的元素值，并转换为想要的类型
        /// </summary>
        /// <typeparam name="T">输出类型</typeparam>
        /// <param name="attr">属性名</param>
        /// <returns>如果无法索引到元素或者类型无法转换，可能会获取到T的默认值</returns>
        public T GetValue<T>(string attr)
        {
            if (_type == TYPE.Object)
            {
                if (dic.ContainsKey(attr))
                {
                    int tp = tdic[attr];
                    object o = dic[attr];
                    return vconvert<T>(tp, o);
                }
                else
                {
                    e(1, "Attr not existed;");
                }
            }
            else
            {
                e(2, "Cannot use key get item from not Dictiorary.");
            }
            return default(T);
        }

        public override string ToString()
        {
            return ToJson();
        }

        private T vconvert<T>(int tp, object o)
        {
            Type needtype = typeof(T);
            return (T)vconvert(tp, o, needtype);
        }
        private object vconvert(int tp, object o, Type needtype)
        {
            if (IsCSBaseType(needtype))
            {
                //basetype
                if (needtype == typeof(string))
                {
                    switch (tp)
                    {
                        case tString:
                            return o;
                        case tNumber:
                        case tBoolean:
                        case tEnum:
                        case tOO:
                            return o.ToString();
                        case tDateTime:
                            return new DateTime((long)o).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    }
                }
                else if (needtype == typeof(bool))
                {
                    switch (tp)
                    {
                        case tString:
                            return !string.IsNullOrEmpty(o as string);
                        case tNumber:
                            {
                                string numstr = o.ToString().Replace("0", "").Replace(".", "");
                                if (numstr != "") return true;
                                return false;
                            }
                        case tEnum:
                        case tDateTime:
                            return true;
                        case tBoolean:
                            return o;
                        case tOO:
                            {
                                OBJ oo = o as OBJ;
                                if (oo._type != TYPE.Null && oo._type != TYPE.Undefined)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case tNull:
                        case tUndefined:
                            return false;
                    }
                }
                else if (needtype == typeof(DateTime))
                {
                    switch (tp)
                    {
                        case tDateTime:
                            return new DateTime((long)o);
                    }
                }
                else if (needtype.IsEnum)
                {
                    switch (tp)
                    {
                        case tEnum:
                            return o;
                    }
                }
                else
                {
                    //number
                    try
                    {
                        switch (tp)
                        {
                            case tNumber:
                                return Convert.ChangeType(o, needtype);
                        }
                    }
                    catch (Exception ex)
                    {
                        e(1, "Convert to number error:" + ex.Message);
                    }
                }
            }
            else
            {
                if (tp == tOO)
                {
                    if (needtype == typeof(OBJ))
                    {
                        //OO
                        return o;
                    }
                    else
                    {
                        //entity
                        try
                        {
                            var ent = Activator.CreateInstance(needtype);
                            (o as OBJ).CopyToObject(ent);
                            return ent;
                        }
                        catch (Exception ex)
                        {
                            e(2, "Connot convert data to the type of [" + needtype.Name + "]:" + ex.Message);
                        }
                    }
                }
            }
            e(1, "Connot convert data to this type.");
            return null;
        }
        /// <summary>
        /// 将自己所有的元素赋值给对象
        /// </summary>
        /// <param name="o">需要被赋值的对象</param>
        public void CopyToObject(object obj)
        {
            if (obj == null)
            {
                e(0, "Cannot copy to null.");
                return;
            }
            if (IsCSBaseType(obj))
            {
                e(1, "Cannot copy to the base type object.");
                return;
            }
            if (obj is OBJ)
            {
                OBJ t = obj as OBJ;
                if (t._type != _type)
                {
                    e(1, "Cannot copy to different type of me.");
                    return;
                }
                if (_type == TYPE.Array)
                {
                    for (int i = 0; i < lst.Count; ++i)
                    {
                        t.lst.Add(lst[i]);
                        t.tlst.Add(tlst[i]);
                    }
                }
                else if (_type == TYPE.Object)
                {
                    foreach (KeyValuePair<string, object> kv in dic)
                    {
                        if (t.dic.ContainsKey(kv.Key))
                        {
                            t.dic[kv.Key] = kv.Value;
                            t.tdic[kv.Key] = tdic[kv.Key];
                        }
                        else
                        {
                            t.dic.Add(kv.Key, kv.Value);
                            t.tdic.Add(kv.Key, tdic[kv.Key]);
                        }
                    }
                }
            }
            else
            {
                if (_type != TYPE.Object)
                {
                    e(1, "Cannot copy to Object from my Array.");
                    return;
                }
                foreach (PropertyInfo p in obj.GetType().GetProperties())
                {
                    if (dic.ContainsKey(p.Name))
                    {
                        p.SetValue(obj, vconvert(tdic[p.Name], dic[p.Name], p.PropertyType), null);
                    }
                }
            }
        }
        /// <summary>
        /// 从非基础类型的对象中拷贝数据
        /// </summary>
        /// <param name="obj">非基础类型的对象</param>
        public void CopyFromObject(object obj)
        {
            if (obj == null)
            {
                e(0, "Cannot copy from null.");
                return;
            }
            if (IsCSBaseType(obj))
            {
                e(1, "Cannot copy from the base type object.");
                return;
            }
            if (obj is OBJ)
            {
                OBJ t = obj as OBJ;
                if (t._type != _type)
                {
                    e(1, "Cannot copy from different type of me.");
                    return;
                }
                if (_type == TYPE.Array)
                {
                    for (int i = 0; i < t.lst.Count; ++i)
                    {
                        lst.Add(t.lst[i]);
                        tlst.Add(t.tlst[i]);
                    }
                }
                else if (_type == TYPE.Object)
                {
                    foreach (KeyValuePair<string, object> kv in t.dic)
                    {
                        if (dic.ContainsKey(kv.Key))
                        {
                            dic[kv.Key] = kv.Value;
                            tdic[kv.Key] = t.tdic[kv.Key];
                        }
                        else
                        {
                            dic.Add(kv.Key, kv.Value);
                            tdic.Add(kv.Key, t.tdic[kv.Key]);
                        }
                    }
                }
            }
            else if (obj is object[])
            {
                if (_type != TYPE.Array)
                {
                    e(1, "Cannot copy from Array to my Dictionary.");
                    return;
                }
                foreach (object o in (obj as object[]))
                {
                    push(o);
                }
            }
            else if (obj is IList)
            {
                if (_type != TYPE.Array)
                {
                    e(1, "Cannot copy from List to my Dictionary.");
                    return;
                }
                foreach (var o in (IList)obj)
                {
                    push(o);
                }
            }
            else if (obj is IDictionary)
            {
                if (_type != TYPE.Object)
                {
                    e(1, "Cannot copy from Dictionary to my Array.");
                    return;
                }
                foreach (DictionaryEntry kv in (obj as IDictionary))
                {
                    if (kv.Key is string)
                    {
                        set(kv.Key as string, kv.Value);
                    }
                }
            }
            else if (obj is DataRow)
            {
                if (_type != TYPE.Object)
                {
                    e(1, "Cannot copy from DataRow to my Array.");
                    return;
                }
                DataRow dr = ((DataRow)obj);
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    set(dc.ColumnName, dr[dc.ColumnName]);
                }
            }
            else if (obj is DataTable)
            {
                if (_type != TYPE.Array)
                {
                    e(1, "Cannot copy from DataTable to my Dictionary.");
                    return;
                }
                foreach (DataRow dr in ((DataTable)obj).Rows)
                {
                    push(dr);
                }
            }
            else
            {
                if (_type != TYPE.Object)
                {
                    e(1, "Cannot copy from Object to my Array.");
                    return;
                }
                foreach (PropertyInfo p in obj.GetType().GetProperties())
                {
                    set(p.Name, p.GetValue(obj, null));
                }
            }
        }

        /// <summary>
        /// 判断是否有值
        /// </summary>
        /// <param name="attr">属性名</param>
        /// <returns>是否有值</returns>
        public bool HasValue(string attr)
        {
            if (_type != TYPE.Object)
            {
                return false;
            }
            if (dic.ContainsKey(attr))
            {
                if (dic[attr] == null)
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
        /// <summary>
        /// 判断是否有值
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>是否有值</returns>
        public bool HasValue(int index)
        {
            if (_type != TYPE.Array)
            {
                return false;
            }
            if (index >= 0 && index < lst.Count)
            {
                if (lst[index] == null)
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
        /// <summary>
        /// 将自己转换为对象，并将匹配的属性赋值
        /// </summary>
        /// <typeparam name="T">非基础对象类型</typeparam>
        /// <returns>对象</returns>
        public T ToEntity<T>() where T : new()
        {
            if (_type != TYPE.Object)
            {
                return default(T);
            }
            var ent = new T();
            CopyToObject(ent);
            return ent;
        }
        /// <summary>
        /// 获取Array中指定序号元素的类型字符串
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>类型字符串</returns>
        public string TypeOf(int index)
        {
            if (_type != TYPE.Array)
            {
                return UNDEFINED;
            }
            if (index >= 0 && index < lst.Count)
            {
                return typeOf(tlst[index], lst[index]);
            }
            else
            {
                return UNDEFINED;
            }
        }
        /// <summary>
        /// 获取Object中指定名称元素的类型字符串
        /// </summary>
        /// <param name="attr">属性名</param>
        /// <returns>类型字符串</returns>
        public string TypeOf(string attr)
        {
            if (_type != TYPE.Object)
            {
                return UNDEFINED;
            }
            if (dic.ContainsKey(attr))
            {
                return typeOf(tdic[attr], dic[attr]);
            }
            else
            {
                return UNDEFINED;
            }
        }
        private string typeOf(int vt, object o)
        {
            switch (vt)
            {
                case tNumber:
                    return NUMBER;
                case tString:
                    return STRING;
                case tBoolean:
                    return BOOLEAN;
                case tDateTime:
                    return DATETIME;
                case tEnum:
                    return ENUM;
                case tNull:
                    return NULL;
                case tOO:
                    {
                        if (o == null)
                        {
                            return NULL;
                        }
                        else
                        {
                            OBJ oo = o as OBJ;
                            switch (oo._type)
                            {
                                case TYPE.Array:
                                    return ARRAY;
                                case TYPE.Object:
                                    return OBJECT;
                                case TYPE.Null:
                                    return NULL;
                                default:
                                    return UNDEFINED;
                            }
                        }
                    }
                default:
                    return UNDEFINED;
            }
        }
    }
}
