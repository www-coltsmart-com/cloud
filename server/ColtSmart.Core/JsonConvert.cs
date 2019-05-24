using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ColtSmart.Core
{
    public static class JsonConvert
    {

        private static readonly JsonSerializerSettings JsonSettings;

        private const string EmptyJson = "";

        static JsonConvert()
        {
            var datetimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

            JsonSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            JsonSettings.Converters.Add(datetimeConverter);
        }



        #region Public Methods

        /// <summary>
        /// 应用Formatting.None和指定的JsonSerializerSettings设置,序列化对象到JSON格式的字符串
        /// </summary>
        /// <param name="obj">任意一个对象</param>
        /// <param name="jsonSettings">在一个 Newtonsoft.Json.JsonSerializer 对象上指定设置，如果为null，则使用默认设置</param>
        /// <returns>标准的JSON格式的字符串</returns>
        public static string ToJson(object obj, JsonSerializerSettings jsonSettings)
        {
            return ToJson(obj, Formatting.None, jsonSettings);
        }

        /// <summary>
        /// 应用指定的Formatting枚举值None和指定的JsonSerializerSettings设置,序列化对象到JSON格式的字符串
        /// </summary>
        /// <param name="obj">任意一个对象</param>
        /// <param name="format">指定 Newtonsoft.Json.JsonTextWriter 的格式设置选项</param>
        /// <param name="jsonSettings">在一个 Newtonsoft.Json.JsonSerializer 对象上指定设置，如果为null，则使用默认设置</param>
        /// <returns>标准的JSON格式的字符串</returns>
        public static string ToJson(object obj, Formatting format, JsonSerializerSettings jsonSettings)
        {
            try
            {
                return obj == null ? EmptyJson : Newtonsoft.Json.JsonConvert.SerializeObject(obj, format, jsonSettings ?? JsonSettings);
            }
            catch (Exception)
            {
                //TODO LOG
                return EmptyJson;
            }
        }

        /// <summary>
        /// 应用Formatting.None和指定的JsonSerializerSettings设置,反序列化JSON数据到指定的.NET类型对象
        /// <para>如果发生JsonSerializationException异常，再以集合的方式重试一次，取出集合的第一个T对象。</para>
        /// <para>转换失败，或发生其它异常，则返回T对象的默认值</para>
        /// </summary>
        /// <param name="json">需要反序列化的JSON字符串</param>
        /// <param name="jsonSettings">在一个 Newtonsoft.Json.JsonSerializer 对象上指定设置，如果为null，则使用默认设置</param>
        /// <typeparam name="T">反序列化对象的类型</typeparam>
        /// <returns></returns>
        public static T FromJson<T>(string json, JsonSerializerSettings jsonSettings) where T : class, new()
        {
            return FromJson<T>(json, Formatting.None, jsonSettings);
        }

        /// <summary>
        /// 应用指定的Formatting枚举值None和指定的JsonSerializerSettings设置,反序列化JSON数据到指定的.NET类型对象
        /// <para>如果发生JsonSerializationException异常，再以集合的方式重试一次，取出集合的第一个T对象。</para>
        /// <para>转换失败，或发生其它异常，则返回T对象的默认值</para>
        /// </summary>
        /// <param name="json">需要反序列化的JSON字符串</param>
        /// <param name="format">指定 Newtonsoft.Json.JsonTextWriter 的格式设置选项</param>
        /// <param name="jsonSettings">在一个 Newtonsoft.Json.JsonSerializer 对象上指定设置，如果为null，则使用默认设置</param>
        /// <typeparam name="T">反序列化对象的类型</typeparam>
        /// <returns></returns>
        public static T FromJson<T>(string json, Formatting format, JsonSerializerSettings jsonSettings) where T : class, new()
        {
            T result;

            if (jsonSettings == null)
            {
                jsonSettings = JsonSettings;
            }

            try
            {
                result = string.IsNullOrWhiteSpace(json) ? default(T) : Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsonSettings);
            }
            catch (JsonSerializationException) //在发生该异常后，再以集合的方式重试一次.
            {
                //LOG
                try
                {
                    var array = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T>>(json, jsonSettings);
                    result = array.FirstOrDefault();
                }
                catch (Exception)
                {
                    //LOG
                    result = default(T);
                }
            }
            catch (Exception)
            {
                //LOG
                result = default(T);
            }
            return result;
        }

        /// <summary>
        /// 应用Formatting.None和指定的JsonSerializerSettings设置,反序列化JSON数据到指定的.NET类型对象
        /// <para>如果发生JsonSerializationException异常，再以集合的方式重试一次，取出集合的第一个T对象。</para>
        /// <para>转换失败，或发生其它异常，则返回T对象的默认值</para>
        /// </summary>
        /// <param name="json">需要反序列化的JSON字符串数据流</param>
        /// <typeparam name="T">反序列化对象的类型</typeparam>
        /// <returns></returns>
        public static T FromJson<T>(Stream stream) where T : class, new()
        {
            return FromJson<T>(stream, JsonSettings);
        }


        /// <summary>
        /// 应用Formatting.None和指定的JsonSerializerSettings设置,反序列化JSON数据到指定的.NET类型对象
        /// <para>如果发生JsonSerializationException异常，再以集合的方式重试一次，取出集合的第一个T对象。</para>
        /// <para>转换失败，或发生其它异常，则返回T对象的默认值</para>
        /// </summary>
        /// <param name="json">需要反序列化的JSON字符串数据流</param>
        /// <param name="jsonSettings">在一个 Newtonsoft.Json.JsonSerializer 对象上指定设置，如果为null，则使用默认设置</param>
        /// <typeparam name="T">反序列化对象的类型</typeparam>
        /// <returns></returns>
        public static T FromJson<T>(Stream stream, JsonSerializerSettings jsonSettings) where T : class, new()
        {
            return FromJson<T>(stream, Formatting.None, jsonSettings);
        }


        /// <summary>
        /// 应用指定的Formatting枚举值None和指定的JsonSerializerSettings设置,反序列化JSON数据到指定的.NET类型对象
        /// <para>如果发生JsonSerializationException异常，再以集合的方式重试一次，取出集合的第一个T对象。</para>
        /// <para>转换失败，或发生其它异常，则返回T对象的默认值</para>
        /// </summary>
        /// <param name="json">需要反序列化的JSON字符串数据流</param>
        /// <param name="format">指定 Newtonsoft.Json.JsonTextWriter 的格式设置选项</param>
        /// <param name="jsonSettings">在一个 Newtonsoft.Json.JsonSerializer 对象上指定设置，如果为null，则使用默认设置</param>
        /// <typeparam name="T">反序列化对象的类型</typeparam>
        /// <returns></returns>
        public static T FromJson<T>(Stream stream, Formatting format, JsonSerializerSettings jsonSettings) where T : class, new()
        {
            T result;

            if (jsonSettings == null)
            {
                jsonSettings = JsonSettings;
            }

            try
            {
                var serializer = JsonSerializer.CreateDefault(JsonSettings);
                serializer.Formatting = format;
                using (var sr = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        result = serializer.Deserialize(jsonTextReader, typeof(T)) as T;
                    }
                }

            }
            catch (Exception)
            {
                //LOG
                result = default(T);
            }
            return result;
        }
        #endregion

        #region Public Extend Methods

        /// <summary>
        /// 反序列化JSON数据到指定的.NET类型对象
        /// <para>如果发生JsonSerializationException异常，再以集合的方式重试一次，取出集合的第一个T对象。</para>
        /// <para>转换失败，或发生其它异常，则返回T对象的默认值</para>
        /// </summary>
        /// <param name="json">需要反序列化的JSON字符串</param>
        /// <typeparam name="T">反序列化对象的类型</typeparam>
        /// <returns></returns>
        public static T FromJson<T>(this string json) where T : class, new()
        {
            return FromJson<T>(json, Formatting.None, JsonSettings);
        }

        /// <summary>
        /// 应用默认的Formatting枚举值None和默认的JsonSerializerSettings设置,序列化对象到JSON格式的字符串
        /// </summary>
        /// <param name="obj">任意一个对象</param>
        /// <returns>标准的JSON格式的字符串</returns>
        public static string ToJson(this object obj)
        {
            return ToJson(obj, Formatting.None, JsonSettings);
        }


        #endregion

    }
}
