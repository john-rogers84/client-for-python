using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DBCHM
{
    public static class CommonExtension
    {
        public static string FormatString(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static void SetHidden(this FileInfo file)
        {
            if (file != null && file.Exists)
            {
                file.Attributes = FileAttributes.Hidden;
            }
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T source)
        {
            if (source == null)
            {
                return default(T);
            }

            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, source);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
    }
}
