using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testInterface
{
    interface IPizza
    {

        int Price { get; set; }
        string DisplayName();

        void Order();
    }
}
