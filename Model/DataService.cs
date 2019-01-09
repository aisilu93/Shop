using System;

namespace Shop.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<good, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new good();
            callback(item, null);
        }
        public void GetData(Action<user, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new user();
            callback(item, null);
        }
    }
}