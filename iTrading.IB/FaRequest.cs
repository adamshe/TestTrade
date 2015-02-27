namespace iTrading.IB
{
    using System;
    using System.Text.RegularExpressions;

    internal class FaRequest : Request
    {
        private Fa fa;

        internal FaRequest(Adapter adapter, Fa fa) : base(adapter.connection)
        {
            this.fa = fa;
        }

        internal static void Process(Adapter adapter, int version)
        {
            adapter.ibSocket.ReadInteger();
            string input = adapter.ibSocket.ReadString();
            adapter.faCustom = adapter.faCustom + Regex.Replace(input, @"<\?xml.*\?>", "");
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(0x12);
            adapter.ibSocket.Send(1);
            adapter.ibSocket.Send((int) this.fa);
        }
    }
}

