using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APlayTest.Client.Factories
{
    public interface IAplayClientFactory
    {
        APlayClient Create();
    }

    public class AplayClientFactory : IAplayClientFactory
    {
        public APlayClient Create()
        {
            return new APlayClient();
        }
    }
}
