using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Extensions.Attribute;

namespace Extensions.EntityHelper
{
    public static class ExtentionMethods
    {
        public static T[] Each<T>(this T[] value, Func<T, bool> func)
        {
            var lst = new T[0];
            var i = 0;
            foreach (var item in value)
            {
                if (func(item))
                {
                    i++;
                    Array.Resize(ref lst, i);
                    lst[i - 1] = item;
                };
            }

            return lst;
        }

        public static void Each<T>(this IEnumerable<T> value, Func<T, bool> func)
        {
            foreach (var item in value)
            {
                func(item);
            }

        }

        #region String Helper

        public static string SubDescriptionString(this string value, int len)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < len)
            {
                return value;
            }
            return value.Substring(0, len);
        }
        #endregion
    }
    public class PropertyContainer
    {
        private readonly Dictionary<string, object> _ids;
        private readonly Dictionary<string, object> _values;

        #region Properties

        public IEnumerable<string> IdNames
        {
            get { return _ids.Keys; }
        }

        public IEnumerable<string> ValueNames
        {
            get { return _values.Keys; }
        }

        internal IEnumerable<string> AllNames
        {
            get { return _ids.Keys.Union(_values.Keys); }
        }

        public IDictionary<string, object> IdPairs
        {
            get { return _ids; }
        }

        public IDictionary<string, object> ValuePairs
        {
            get { return _values; }
        }

        public IEnumerable<KeyValuePair<string, object>> AllPairs
        {
            get { return _ids.Concat(_values); }
        }

        #endregion

        #region Constructor

        internal PropertyContainer()
        {
            _ids = new Dictionary<string, object>();
            _values = new Dictionary<string, object>();
        }

        #endregion

        #region Methods

        internal void AddId(string name, object value)
        {
            _ids.Add(name, value);
        }

        internal void AddValue(string name, object value)
        {
            _values.Add(name, value);
        }

        #endregion
    }

    public static class EntityHelper
    {
        public static string GetName<T>(T obj) where T : class, new()
        {
            return typeof(T).Name;
        }

        public static PropertyInfo[] GetAllPublicProps(Type obj)
        {
            return obj.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
        }

        public static string[] GetPrimaryKeys(Type obj)
        {
            var props = GetAllPublicProps(obj);
            var i = 0;
            var _props = new string[0];

            var pi = props.Each(p => { return p.IsDefined(typeof(HrmPrimaryKey), false); });

            pi.Each((item) =>
            {
                Array.Resize(ref _props, i + 1);
                _props[i] = item.Name;
                i++;
                return true;
            });


            return _props;
        }

        public static string BuildWhere(string[] keys)
        {
            var s = keys.Aggregate("",
                (current, key) => current + string.Format((string)" {0} = @{0} AND", (object)key));
            return s.Substring(0, s.Length - 3);
        }

        /// <summary>
        /// Create a commaseparated list of value pairs on 
        /// the form: "key1=@value1, key2=@value2, ..."
        /// </summary>
        public static string GetSqlPairs
            (IEnumerable<string> keys, string separator = ", ")
        {
            var pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
            return string.Join(separator, pairs);
        }

        /// <summary>
        /// Retrieves a Dictionary with name and value 
        /// for all object properties matching the given criteria.
        /// </summary>
        public static PropertyContainer ParseProperties<T>(T obj)
        {
            var propertyContainer = new PropertyContainer();

            var typeName = typeof(T).Name;
            var validKeyNames = new[]
            {
                "Id",
                string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName)
            };

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                // Skip reference types (but still include string!)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(HrmIgnoreField), false))
                    continue;

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                if (property.IsDefined(typeof(HrmPrimaryKey), false) || validKeyNames.Contains(name))
                {
                    propertyContainer.AddId(name, value);
                }
                else
                {
                    propertyContainer.AddValue(name, value);
                }
            }

            return propertyContainer;
        }

        public static void SetId<T>(T obj, object id, IDictionary<string, object> propertyPairs)
        {
            if (propertyPairs.Count == 1)
            {
                var propertyName = propertyPairs.Keys.First();
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                var outId = Convert.ChangeType(id, propertyInfo.PropertyType);
                propertyInfo.SetValue(obj, outId, null);
            }
        }

        public static string GetINSql<T>(IEnumerable<T> items)
        {
            var keys = GetPrimaryKeys(typeof(T));
            if (keys.Length > 0)
            {
                var _in = new List<string[]>();
                var i = 0;

                //initialize each item in list:
                for (int j = 0; j < keys.Length; j++)
                {
                    _in.Add(new string[items.Count()]);
                }

                foreach (var item in items)
                {
                    for (int j = 0; j < keys.Length; j++)
                    {
                        var value = typeof(T).GetProperty(keys[j]).GetValue(item, null);
                        _in[j][i] = value.ToString();
                    }
                    i++;
                }

                var _inStr = "";
                for (int j = 0; j < keys.Length; j++)
                {
                    _inStr += string.Format(" {0} IN ({1}) AND", keys[j], string.Join(",", _in[j].ToArray()));
                }

                return _inStr.Substring(0, _inStr.Length - 3);
            }
            return string.Empty;
        }
    }
}
