using System;
using Shop.Model;

namespace Shop.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<good, Exception> callback)
        {
            // Use this to create design time data

            var item = new good();
            callback(item, null);
        }
        public void GetData(Action<user, Exception> callback)
        {
            // Use this to create design time data

            var item = new user();
            callback(item, null);
        }
    }
}