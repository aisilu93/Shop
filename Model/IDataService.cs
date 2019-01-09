using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Model
{
    public interface IDataService
    {
        void GetData(Action<good, Exception> callback);
        void GetData(Action<user, Exception> callback);
    }
}
