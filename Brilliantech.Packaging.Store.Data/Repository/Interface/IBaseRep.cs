using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.Packaging.Store.Data.Repository.Interface
{
    public interface IBaseRep<T>
    {
        /// <summary>
        /// get single obj by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetSingleById(string id);

        /// <summary>
        /// validate the obj
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Valid(T obj);

        /// <summary>
        /// validate the obj by the id
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        bool Valid(string objId);
    }
}
