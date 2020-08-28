using System;

namespace RPCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var RPCClient = new RPCClient();
            var num = args[0];

            Console.WriteLine(" [x] Requesting fib({0})", num);
            var response = RPCClient.Call(num);

            Console.WriteLine(" [.] Got '{0}'", response);
            RPCClient.Close();                        
        }
    }
}
