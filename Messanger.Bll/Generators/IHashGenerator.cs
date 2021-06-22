using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Bll.Generators
{
    public interface IHashGenerator
    {
        string GenerateHash(string password);
    }
}
