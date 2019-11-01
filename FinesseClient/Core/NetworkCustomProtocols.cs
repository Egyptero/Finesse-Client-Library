using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace FinesseClient.Core
{
    public class NetworkCustomProtocols
    {
        public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
        public const SslProtocols _Tls11 = (SslProtocols)0x00000300;
        public const SecurityProtocolType Tls11 = (SecurityProtocolType)_Tls11;

    }
}
