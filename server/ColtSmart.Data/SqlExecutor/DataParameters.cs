using Dapper;
using System.Collections.Generic;
using System.Data;

namespace ColtSmart.Data
{
    public class DataParameters 
    {
        private DynamicParameters _parameters = new DynamicParameters();


        /// <summary>
        /// Add a parameter to this dynamic parameter list.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dbType">The type of the parameter.</param>
        /// <param name="direction">The in or out direction of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        public void Add(string name, object value, DbType? dbType, ParameterDirection? direction, int? size)
        {
            _parameters.Add(name, value, dbType, direction, size);
        }

        /// <summary>
        /// Add a parameter to this dynamic parameter list.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="dbType">The type of the parameter.</param>
        /// <param name="direction">The in or out direction of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <param name="precision">The precision of the parameter.</param>
        /// <param name="scale">The scale of the parameter.</param>
        public void Add(string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
        {
            _parameters.Add(name, value, dbType, direction, size, precision, scale);
        }

        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>The value, note DBNull.Value is not returned, instead the value is returned as null</returns>
        public T Get<T>(string name)
        {
            return _parameters.Get<T>(name);
        }

        /// <summary>
        /// Add parameters to this dynamic parameter list.
        /// </summary>
        /// <param name="parameters"></param>
        public void AddRange(DataParameters parameters)
        {
            _parameters.AddDynamicParams(parameters.ToDynamicParameters());
        }

        public IEnumerable<string> ParameterNames => _parameters.ParameterNames;

        internal DynamicParameters ToDynamicParameters()
        {
            return _parameters;
        }
    }
}
