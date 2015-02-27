namespace iTrading.IB
{
    using System;
    using System.IO;
    using System.Xml;
    using iTrading.Core.Kernel;

    internal class Order
    {
        internal string Account = "";
        internal string Action = "";
        internal double AuxPrice = 0.0;
        internal bool BlOrder = false;
        internal int ConnectionId = 0;
        internal double discretionaryAmt = 0.0;
        internal int DisplaySize = 0;
        internal string faGroup = "";
        internal string faMethod = "";
        internal string faPercentage = "";
        internal string faProfile = "";
        internal string goodAfterTime = "";
        internal string goodTillDate = "";
        internal bool Hidden = false;
        internal bool IgnoreRth = true;
        internal double LmtPrice = 0.0;
        internal string OcaGroup = "";
        internal string OpenClose = "O";
        internal string OrderRef = "";
        internal string OrderType = "";
        internal iTrading.IB.Origin Origin = iTrading.IB.Origin.CUSTOMER;
        internal int ParentId = 0;
        internal int permId = 0;
        internal string SharesAllocation = "";
        internal bool SweepToFill = false;
        internal string Tif = "";
        internal int totalQuantity = 0;
        internal bool Transmit = true;
        internal int TriggerMethod = 0;

        internal Order(Adapter adapter, string account, int quantity, string action, string orderType, string tif, double lmtPrice, double auxPrice, string ocaGroup, string customText)
        {
            this.Account = !adapter.isFaAccount ? "" : account;
            this.Action = action;
            this.AuxPrice = auxPrice;
            this.IgnoreRth = adapter.Options.IgnoreRth;
            this.LmtPrice = lmtPrice;
            this.totalQuantity = quantity;
            this.OcaGroup = adapter.Options.UseNativeOcaOrders ? ocaGroup : "";
            this.OrderType = orderType;
            this.SharesAllocation = !adapter.isFaAccount ? "" : (account + "/" + quantity);
            this.Tif = tif;
            if ((customText != null) && (customText.Length > 0))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    string str = adapter.connection.Options.Provider.Id.ToString();
                    XmlTextReader reader = new XmlTextReader(new StringReader(customText));
                    document.Load(reader);
                    reader.Close();
                    if ((document["TradeMagic"] != null) && (document["TradeMagic"][str] != null))
                    {
                        try
                        {
                            if (document["TradeMagic"][str]["AccountGroup"] != null)
                            {
                                this.Account = "";
                                this.faGroup = document["TradeMagic"][str]["AccountGroup"].InnerText;
                                this.faMethod = document["TradeMagic"][str]["DefaultMethod"].InnerText;
                                if (this.faMethod == "PctChange")
                                {
                                    this.faPercentage = document["TradeMagic"][str]["Percentage"].InnerText;
                                }
                                this.SharesAllocation = "";
                            }
                            else if (document["TradeMagic"][str]["AllocationProfile"] != null)
                            {
                                this.Account = "";
                                this.faProfile = document["TradeMagic"][str]["AllocationProfile"].InnerText;
                                this.SharesAllocation = "";
                            }
                        }
                        catch (Exception exception)
                        {
                            throw new TMException(ErrorCode.CustomText, exception.Message);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

