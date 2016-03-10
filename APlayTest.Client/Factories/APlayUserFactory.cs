using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APlayTest.Client.Factories
{
    public interface IAPlayUserFactory
    {
        User Create();
    }

    public class APlayUserFactory : IAPlayUserFactory
    {
        public User Create()
        {
            return new User(){Name = Environment.UserName};
        }
    }
}
