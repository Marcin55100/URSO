using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URSO_LED.Security
{
    public interface IHavePassword
    {
        System.Security.SecureString Password { get; }
    }
}
