using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APlayTest.Client.Modules.SheetTree.ViewModels;

namespace APlayTest.Client.Modules.SheetTree.Factories
{
    //public interface IConnectionViewModelFactory
    //{
    //    ConnectionViewModel Create(Connection connection);
    //    void Free(Connection connection);
    //    void Remove(int id);
    //}

    //[Export(typeof(IConnectionViewModelFactory))]
    //public class ConnectionViewModelFactory : IConnectionViewModelFactory
    //{
    //    private static readonly object MyLock = new object();
    //    private readonly Dictionary<int, ConnectionViewModel> _cache = new Dictionary<int, ConnectionViewModel>();
    //    public ConnectionViewModel Create(Connection connection)
    //    {
    //        //lock (MyLock)
    //        //{
    //        //    ConnectionViewModel connectionViewModel;
    //        //    if (!_cache.TryGetValue(connection.Id,out connectionViewModel))
    //        //    {
    //        //        connectionViewModel = new ConnectionViewModel(connection);
    //        //        _cache[connection.Id] = connectionViewModel;
    //        //    }

    //        //    return connectionViewModel;
    //        //}
    //    }

    //    public void Free(Connection connection)
    //    {
    //        lock (MyLock)
    //        {
    //            _cache.Remove(connection.Id);
    //        }
    //    }

    //    public void Remove(int id)
    //    {
    //        _cache.Remove(id);
    //    }
    //}
}
