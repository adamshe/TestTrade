using iTrading.Core.Data;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using iTrading.Core.Kernel;
using iTrading.Indicator;

namespace iTrading.Test
{
    

    /// <summary>
    /// Test indicators.
    /// </summary>
    public class IndicatorTest : TestBase
    {
        private double[] dataClose = new double[] { 
            91.5, 94.815, 94.375, 95.095, 93.78, 94.625, 92.53, 92.75, 90.315, 92.47, 96.125, 97.25, 98.5, 89.875, 91.0, 92.815, 
            89.155, 89.345, 91.625, 89.875, 88.375, 87.625, 84.78, 83.0, 83.5, 81.375, 84.44, 89.25, 86.375, 86.25, 85.25, 87.125, 
            85.815, 88.97, 88.47, 86.875, 86.815, 84.875, 84.19, 83.875, 83.375, 85.5, 89.19, 89.44, 91.095, 90.75, 91.44, 89.0, 
            91.0, 90.5, 89.03, 88.815, 84.28, 83.5, 82.69, 84.75, 85.655, 86.19, 88.94, 89.28, 88.625, 88.5, 91.97, 91.5, 
            93.25, 93.5, 93.155, 91.72, 90.0, 89.69, 88.875, 85.19, 83.375, 84.875, 85.94, 97.25, 99.875, 104.94, 106.0, 102.5, 
            102.405, 104.595, 106.125, 106.0, 106.065, 104.625, 108.625, 109.315, 110.5, 112.75, 123.0, 119.625, 118.75, 119.25, 117.94, 116.44, 
            115.19, 111.875, 110.595, 118.125, 116.0, 116.0, 112.0, 113.75, 112.94, 116.0, 120.5, 116.62, 117.0, 115.25, 114.31, 115.5, 
            115.87, 120.69, 120.19, 120.75, 124.75, 123.37, 122.94, 122.56, 123.12, 122.56, 124.62, 129.25, 131.0, 132.25, 131.0, 132.81, 
            134.0, 137.38, 137.81, 137.88, 137.25, 136.31, 136.25, 134.63, 128.25, 129.0, 123.87, 124.81, 123.0, 126.25, 128.38, 125.37, 
            125.69, 122.25, 119.37, 118.5, 123.19, 123.5, 122.19, 119.31, 123.31, 121.12, 123.37, 127.37, 128.5, 123.87, 122.94, 121.75, 
            124.44, 122.0, 122.37, 122.94, 124.0, 123.19, 124.56, 127.25, 125.87, 128.86, 132.0, 130.75, 134.75, 135.0, 132.38, 133.31, 
            131.94, 130.0, 125.37, 130.13, 127.12, 125.19, 122.0, 125.0, 123.0, 123.5, 120.06, 121.0, 117.75, 119.87, 122.0, 119.19, 
            116.37, 113.5, 114.25, 110.0, 105.06, 107.0, 107.87, 107.0, 107.12, 107.0, 91.0, 93.94, 93.87, 95.5, 93.0, 94.94, 
            98.25, 96.75, 94.81, 94.37, 91.56, 90.25, 93.94, 93.62, 97.0, 95.0, 95.87, 94.06, 94.62, 93.75, 98.0, 103.94, 
            107.87, 106.06, 104.5, 105.0, 104.19, 103.06, 103.42, 105.27, 111.87, 116.0, 116.62, 118.28, 113.37, 109.0, 109.7, 109.25, 
            107.0, 109.19, 110.0, 109.2, 110.12, 108.0, 108.62, 109.75, 109.81, 109.0, 108.75, 107.87
         };
        private double[] dataHigh = new double[] { 
            93.25, 94.94, 96.375, 96.19, 96.0, 94.72, 95.0, 93.72, 92.47, 92.75, 96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 
            93.405, 90.655, 91.97, 92.25, 90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125, 87.155, 87.25, 
            87.375, 88.97, 90.0, 89.845, 86.97, 85.94, 84.75, 85.47, 84.47, 88.5, 89.47, 90.0, 92.44, 91.44, 92.97, 91.72, 
            91.155, 91.75, 90.0, 88.875, 89.0, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625, 90.75, 88.845, 91.97, 93.375, 
            93.815, 94.03, 94.03, 91.815, 92.0, 91.94, 89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25, 
            104.5, 105.5, 106.125, 107.94, 106.25, 107.0, 108.75, 110.94, 110.94, 114.22, 123.0, 121.75, 119.815, 120.315, 119.375, 118.19, 
            116.69, 115.345, 113.0, 118.315, 116.87, 116.75, 113.87, 114.62, 115.31, 116.0, 121.69, 119.87, 120.87, 116.75, 116.5, 116.0, 
            118.31, 121.5, 122.0, 121.44, 125.75, 127.75, 124.19, 124.44, 125.75, 124.69, 125.31, 132.0, 131.31, 132.25, 133.88, 133.5, 
            135.5, 137.44, 138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44, 125.12, 126.5, 128.69, 126.62, 
            126.69, 126.0, 123.12, 121.87, 124.0, 127.0, 124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37, 
            124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126.0, 128.5, 126.75, 129.75, 132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 
            135.0, 132.38, 131.44, 130.88, 129.63, 127.25, 127.81, 125.0, 126.81, 124.75, 122.81, 122.25, 121.06, 120.0, 123.25, 122.75, 
            119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109.0, 108.5, 113.06, 93.0, 94.62, 95.12, 96.0, 95.56, 95.31, 
            99.0, 98.81, 96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37, 95.0, 94.87, 98.25, 105.12, 
            108.44, 109.87, 105.0, 106.0, 104.94, 104.5, 104.44, 106.31, 112.87, 116.5, 119.19, 121.0, 122.12, 111.94, 112.75, 110.19, 
            107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110.0, 110.75, 110.5, 110.5, 109.5
         };
        private double[] dataLow = new double[] { 
            90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92.0, 89.75, 89.44, 90.625, 92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 
            88.905, 88.78, 89.25, 89.75, 87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97, 84.405, 85.095, 
            85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44, 82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89.0, 
            88.565, 90.095, 89.0, 86.47, 84.0, 83.315, 82.0, 83.25, 84.75, 85.28, 87.19, 88.44, 88.25, 87.345, 89.28, 91.095, 
            89.53, 91.155, 92.0, 90.53, 89.97, 88.815, 86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75, 
            101.72, 101.72, 103.155, 105.69, 103.655, 104.0, 105.53, 108.53, 108.75, 107.75, 117.0, 118.0, 116.0, 118.5, 116.53, 116.25, 
            114.595, 110.875, 110.5, 110.72, 112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31, 112.56, 114.0, 
            114.75, 118.87, 119.0, 119.75, 122.62, 123.0, 121.75, 121.56, 123.12, 122.19, 122.75, 124.37, 128.0, 129.5, 130.81, 130.63, 
            132.13, 133.88, 135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62, 122.75, 123.56, 125.81, 124.62, 
            124.37, 121.81, 118.19, 118.06, 117.56, 121.0, 121.12, 118.94, 119.81, 121.0, 122.0, 124.5, 126.56, 123.5, 121.25, 121.06, 
            122.31, 121.0, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128.0, 128.38, 130.69, 131.63, 134.38, 132.0, 131.94, 
            131.94, 129.56, 123.75, 126.0, 126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120.0, 119.62, 115.5, 116.75, 119.06, 119.06, 
            115.06, 111.06, 113.12, 110.0, 105.0, 104.69, 103.87, 104.69, 105.44, 107.0, 89.0, 92.5, 92.12, 94.62, 92.81, 94.25, 
            96.25, 96.37, 93.69, 93.5, 90.0, 90.19, 90.5, 92.12, 94.12, 94.87, 93.0, 93.87, 93.0, 92.62, 93.56, 98.37, 
            104.44, 106.0, 101.81, 104.12, 103.37, 102.12, 102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87, 
            104.5, 105.75, 108.62, 107.75, 108.06, 108.0, 108.19, 108.12, 109.06, 108.75, 108.56, 106.62
         };
        private double[] dataOpen = new double[] { 
            92.5, 91.5, 95.155, 93.97, 95.5, 94.5, 95.0, 91.5, 91.815, 91.125, 93.875, 97.5, 98.815, 92.0, 91.125, 91.875, 
            93.405, 89.75, 89.345, 92.25, 89.78, 87.94, 87.595, 85.22, 83.5, 83.5, 81.25, 85.125, 88.125, 87.5, 85.25, 86.0, 
            87.19, 86.125, 89.0, 88.625, 86.0, 85.5, 84.75, 85.25, 84.25, 86.75, 86.94, 89.315, 89.94, 90.815, 91.19, 91.345, 
            89.595, 91.0, 89.75, 88.75, 88.315, 84.345, 83.5, 84.0, 86.0, 85.53, 87.5, 88.5, 90.0, 88.655, 89.5, 91.565, 
            92.0, 93.0, 92.815, 91.75, 92.0, 91.375, 89.75, 88.75, 85.44, 83.5, 84.875, 98.625, 96.69, 102.375, 106.0, 104.625, 
            102.5, 104.25, 104.0, 106.125, 106.065, 105.94, 105.625, 108.625, 110.25, 110.565, 117.0, 120.75, 118.0, 119.125, 119.125, 117.815, 
            116.375, 115.155, 111.25, 111.5, 116.69, 116.0, 113.62, 111.75, 114.56, 113.62, 118.12, 119.87, 116.62, 115.87, 115.06, 115.87, 
            117.5, 119.87, 119.25, 120.19, 122.87, 123.87, 122.25, 123.12, 123.31, 124.0, 123.0, 124.81, 130.0, 130.88, 132.5, 131.0, 
            132.5, 134.0, 137.44, 135.75, 138.31, 138.0, 136.38, 136.5, 132.0, 127.5, 127.62, 124.0, 123.62, 125.0, 126.37, 126.25, 
            125.94, 124.0, 122.75, 120.0, 120.0, 122.0, 123.62, 121.5, 120.12, 123.75, 122.75, 125.0, 128.5, 128.38, 123.87, 124.37, 
            122.75, 123.37, 122.0, 122.62, 125.0, 124.25, 124.37, 125.62, 126.5, 128.38, 128.88, 131.5, 132.5, 137.5, 134.63, 132.0, 
            134.0, 132.0, 131.38, 126.5, 128.75, 127.19, 127.5, 120.5, 126.62, 123.0, 122.06, 121.0, 121.0, 118.0, 122.0, 122.25, 
            119.12, 115.0, 113.5, 114.0, 110.81, 106.5, 106.44, 108.0, 107.0, 108.62, 93.0, 93.75, 94.25, 94.87, 95.5, 94.5, 
            97.0, 98.5, 96.75, 95.87, 94.44, 92.75, 90.5, 95.06, 94.62, 97.5, 96.0, 96.0, 94.62, 94.87, 94.0, 99.0, 
            105.5, 108.81, 105.0, 105.94, 104.94, 103.69, 102.56, 103.44, 109.81, 113.0, 117.0, 116.25, 120.5, 111.62, 108.12, 110.19, 
            107.75, 108.0, 110.69, 109.06, 108.5, 109.87, 109.12, 109.69, 109.56, 110.44, 109.69, 109.19
         };
        private int[] dataVolume = new int[] { 
            0x3e37bc, 0x4b9efc, 0x48dd84, 0x3f67a4, 0x4615cc, 0x3768c4, 0x339e10, 0x4b9858, 0x44aa20, 0x33d77c, 0x4027d4, 0x6074f8, 0x9bb1d0, 0x122963c, 0xb267e0, 0x91c594, 
            0x881cec, 0x5b1bd4, 0x4d3e9c, 0x388b00, 0x598080, 0x557eb8, 0x58aebc, 0x8173d8, 0x5b7ac0, 0x528820, 0x52dce4, 0x5fe218, 0x590830, 0x44e6ac, 0x448ff4, 0x4250f4, 
            0x38764c, 0x463188, 0x458990, 0x41f910, 0x4feafc, 0x70fa44, 0x4937c0, 0x42b940, 0x3b162c, 0xa40b50, 0x589300, 0x39c31c, 0x4c8510, 0x3585f4, 0x419830, 0x49e404, 
            0x3c4e48, 0x326cfc, 0x34d370, 0x6eaf8c, 0x94d20c, 0x5ad5e8, 0x4c78f4, 0x5a3110, 0x4b06a4, 0x3f1858, 0x3ddcb8, 0x38ff04, 0x2c95ac, 0x289060, 0x469010, 0x42b6e8, 
            0x58fbb0, 0x412238, 0x2eae00, 0x44988c, 0x3442e8, 0x3766d0, 0x3f99a4, 0x5b0c34, 0x756200, 0x7057c4, 0x646c34, 0x12ae274, 0x9e5d68, 0x8e6fac, 0x9fb780, 0x5689e8, 
            0x5622c8, 0x44f2c8, 0x44f64c, 0x54fca4, 0x40b154, 0x3fb5c4, 0x4c38e4, 0x48e298, 0x3fef30, 0x5c1764, 0xb9aec4, 0x89f3a0, 0x58334c, 0x41ee20, 0x3b7edc, 0x312798, 
            0x34ba0c, 0x41ad48, 0x47c69c, 0x7ee58c, 0xa10a2c, 0x616ca0, 0x6d4da4, 0x6aee38, 0x4e08f4, 0x5051b8, 0x65b774, 0x70e400, 0x551158, 0x4a0a4c, 0x419574, 0x4ac694, 
            0x4a9494, 0x6a4640, 0x476620, 0x8bd8c8, 0x62b8a8, 0x67a340, 0x438078, 0x4fd51c, 0x45aa60, 0x5f2440, 0x4fbdac, 0x7e0f18, 0x5a37b4, 0x361a00, 0x573244, 0x6587cc, 
            0x5cfea4, 0x493ae0, 0x4d11b0, 0x563204, 0x481e1c, 0x553ac0, 0x4e32c0, 0x746d78, 0xda60b0, 0x862e00, 0x8768d8, 0x6a4d48, 0x544ffc, 0x636b2c, 0x50bf7c, 0x572754, 
            0x420914, 0x45b3c0, 0x68a9c0, 0x57da3c, 0x706958, 0x5dc064, 0x3dd6dc, 0x594264, 0x549afc, 0x4cc908, 0x4325d8, 0x4aad94, 0x4e6e20, 0x64dfd4, 0x73bdb0, 0x5afd5c, 
            0x5c3f3c, 0x80d5a4, 0x62a4bc, 0x5f576c, 0x427c50, 0x48db2c, 0x5edcc4, 0x5fa014, 0x5537a0, 0x50a294, 0x7345c4, 0x6e021c, 0x808d60, 0x4dbc64, 0x43cdd0, 0x460dfc, 
            0x565720, 0x5cfd14, 0xe2c778, 0x731f7c, 0x6a9780, 0x7b7d48, 0x73d070, 0x71b2a4, 0x6c0070, 0x6c6b14, 0x85f430, 0x6560bc, 0xa1313c, 0x6b781c, 0x623ff4, 0x7af364, 
            0xd94900, 0x110b5ac, 0x777e3c, 0x7e4ec4, 0xe52748, 0xd86b84, 0xd46b4c, 0x93c038, 0x8c2fe4, 0xada714, 0x423ae84, 0x19701c0, 0xd216d0, 0xad8450, 0x96a348, 0x903404, 
            0xa91e24, 0x91c3a0, 0xa98030, 0x9e4c38, 0xfed9cc, 0xd1af4c, 0x820f50, 0x91c01c, 0x847434, 0x6d2054, 0x93998c, 0x5f977c, 0x8255a0, 0x7f8870, 0x11da820, 0x12c571c, 
            0xd0c744, 0xa2c844, 0x942c08, 0x2f808c, 0x56b350, 0x57f210, 0x517b60, 0x5eec00, 0xe001b4, 0x9790c8, 0xacdfdc, 0x7c4994, 0xfdfa5c, 0xc02a88, 0x6ceb34, 0x718f7c, 
            0x9013fc, 0x6cb290, 0x753258, 0x49d978, 0x48de4c, 0x3d2b60, 0x452d60, 0x3915ac, 0x3e5440, 0x28f910, 0x3475b0, 0x2bcce4
         };
        private ArrayList quotesHolders = new ArrayList();

        private void Connection_Bar(object sender, BarUpdateEventArgs e)
        {
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 900", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 901", e.Quotes.Bars.Count == 0);
            QuotesHolder customLink = (QuotesHolder) e.Quotes.CustomLink;
            customLink.quotes = e.Quotes;
            lock (this.quotesHolders)
            {
                if (this.quotesHolders.Contains(customLink))
                {
                    this.quotesHolders.Remove(customLink);
                }
            }
        }

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new IndicatorTest();
        }

        /// <summary>
        /// Setup test environment.
        /// </summary>
        protected override void DoSetUp()
        {
        }

        /// <summary>
        /// Execute test.
        /// </summary>
        protected override void DoTest()
        {
            ArrayList list;
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Indicator.DoTest1");
            }
            base.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
            SymbolTemplate template = base.SymbolTemplates[0];
            Symbol symbol = base.Connection.GetSymbol(template.Name, template.Expiry, base.Connection.SymbolTypes[template.SymbolTypeId], base.Connection.Exchanges[template.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
             iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000a ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), template.Valid ? (symbol != null) : (symbol == null));
            Trace.Assert(symbol != null);
            QuotesHolder holder = new QuotesHolder();
            lock ((list = this.quotesHolders))
            {
                this.quotesHolders.Add(holder);
            }
            symbol.RequestQuotes(iTrading.Core.Kernel.Globals.MinDate, iTrading.Core.Kernel.Globals.MinDate, new Period(PeriodTypeId.Day, 1), false, LookupPolicyId.RepositoryOnly, holder);
            while (true)
            {
                lock ((list = this.quotesHolders))
                {
                    if (!this.quotesHolders.Contains(holder))
                    {
                        break;
                    }
                }
                Thread.Sleep(10);
                Application.DoEvents();
            }
            iTrading.Core.Data.Quotes quotes = holder.quotes;
            DateTime from = quotes.From;
            for (int i = 0; i < this.dataOpen.Length; i++)
            {
                quotes.Bars.Add(this.dataOpen[i], this.dataHigh[i], this.dataLow[i], this.dataClose[i], from, this.dataVolume[i], false);
                from = from.AddDays(1.0);
            }
            Sma sma = new Sma(quotes);
            sma.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma00", sma[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma01", sma[1], 93.1575, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma02", sma[2], 94.595, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma03", sma[3], 94.735, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma04", sma[0x10], 90.985, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma05", sma[0xfb], 108.31, 0.0005);
            sma = new Sma(quotes);
            sma.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma10", sma[0x1d], 85.497, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma11", sma[30], 85.184, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma12", sma[0x1f], 85.134, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma13", sma[0x3a], 86.435, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma14", sma[250], 109.244, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sma15", sma[0xfb], 109.112, 0.0005);
            Ema ema = new Ema(quotes);
            ema.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema00", ema[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema01", ema[1], 93.71, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema02", ema[2], 94.153, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema03", ema[0x10], 90.23, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema04", ema[0xfb], 108.216, 0.0005);
            ema = new Ema(quotes);
            ema.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema10", ema[9], 92.607, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema11", ema[10], 93.246, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema12", ema[0x1d], 86.452, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema13", ema[0xf3], 109.528, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Ema15", ema[0xfb], 108.975, 0.0005);
            Max max = new Max(quotes);
            max.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max00", max[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max01", max[1], 94.815, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max02", max[2], 94.815, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max03", max[0x10], 92.815, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max04", max[0xfb], 108.75, 0.0005);
            max = new Max(quotes);
            max.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max10", max[9], 95.095, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max11", max[10], 96.125, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max12", max[0x1d], 89.25, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max13", max[0xf3], 118.28, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Max15", max[0xfb], 110.12, 0.0005);
            Min min = new Min(quotes);
            min.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min00", min[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min01", min[1], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min02", min[2], 94.375, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min03", min[0x10], 89.155, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min04", min[0xfb], 107.87, 0.0005);
            min = new Min(quotes);
            min.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min10", min[9], 90.315, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min11", min[10], 90.315, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min12", min[0x1d], 81.375, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min13", min[0xf3], 107.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Min15", min[0xfb], 107.87, 0.0005);
            StdDev dev = new StdDev(quotes);
            dev.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev00", dev[0], 0.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev01", dev[1], 1.657, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev02", dev[2], 0.22, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev03", dev[0x10], 1.83, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev04", dev[0xfb], 0.44, 0.0005);
            dev = new StdDev(quotes);
            dev.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev10", dev[9], 1.495, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev11", dev[10], 1.601, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev12", dev[0x1d], 2.389, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev13", dev[0xf3], 3.496, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StdDev15", dev[0xfb], 0.766, 0.0005);
            Atr atr = new Atr(quotes);
            atr.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr00", atr[0], 2.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr01", atr[1], 3.017, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr02", atr[2], 2.571, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr03", atr[0x10], 4.207, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr04", atr[0xfb], 2.384, 0.0005);
            atr = new Atr(quotes);
            atr.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr10", atr[9], 2.797, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr11", atr[10], 2.895, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr12", atr[0x1d], 3.435, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr13", atr[0xf3], 4.209, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Atr15", atr[0xfb], 3.028, 0.0005);
            Bollinger bollinger = new Bollinger(quotes);
            bollinger.NumStdDev = 2.0;
            bollinger.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll00", bollinger.Lower[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll01", bollinger.Lower[1], 89.8425, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll02", bollinger.Lower[2], 94.155, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll03", bollinger.Lower[0x10], 87.325, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll04", bollinger.Lower[0xfb], 107.43, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll10", bollinger[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll11", bollinger[1], 96.4725, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll12", bollinger[2], 95.035, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll13", bollinger[0x10], 94.645, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll14", bollinger[0xfb], 109.19, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll20", bollinger.Upper[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll21", bollinger.Upper[1], 96.4725, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll22", bollinger.Upper[2], 95.035, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll23", bollinger.Upper[0x10], 94.645, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll24", bollinger.Upper[0xfb], 109.19, 0.0005);
            bollinger = new Bollinger(quotes);
            bollinger.NumStdDev = 1.0;
            bollinger.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll30", bollinger.Upper[9], 94.72, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll31", bollinger.Upper[10], 95.289, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll32", bollinger.Upper[0x1d], 87.886, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll33", bollinger.Upper[0xf3], 114.657, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll35", bollinger.Upper[0xfb], 109.878, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll40", bollinger.Lower[9], 91.731, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll41", bollinger.Lower[10], 92.087, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll42", bollinger.Lower[0x1d], 83.108, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll43", bollinger.Lower[0xf3], 107.665, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll45", bollinger.Lower[0xfb], 108.346, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll50", bollinger[9], 94.72, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll51", bollinger[10], 95.289, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll52", bollinger[0x1d], 87.886, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll53", bollinger[0xf3], 114.657, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Boll55", bollinger[0xfb], 109.878, 0.0005);
            Dmi dmi = new Dmi(quotes);
            dmi.Period = 2;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi00", dmi[0], 0.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi01", dmi[1], 1.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi02", dmi[3], 0.314, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi03", dmi[0x10], -0.025, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi04", dmi[0xfb], -1.0, 0.0005);
            dmi = new Dmi(quotes);
            dmi.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi10", dmi[9], -0.235, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi11", dmi[10], 0.114, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi12", dmi[0x1d], -0.196, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi13", dmi[0xf3], -0.23, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Dmi15", dmi[0xfb], -0.162, 0.0005);
            Macd macd = new Macd(quotes);
            macd.Fast = 12;
            macd.Slow = 0x1a;
            macd.Smooth = 9;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd00", macd[0], 0.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd01", macd[1], 0.264, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd02", macd[3], 0.618, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd03", macd[0x10], -0.023, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd04", macd[0xfb], 0.904, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd10", macd.MacdAvg[0], 0.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd11", macd.MacdAvg[1], 0.053, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd12", macd.MacdAvg[3], 0.227, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd13", macd.MacdAvg[0x10], 0.425, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd14", macd.MacdAvg[0xfb], 1.405, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd20", macd.MacdDiff[0], 0.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd21", macd.MacdDiff[1], 0.212, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd22", macd.MacdDiff[3], 0.392, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd23", macd.MacdDiff[0x10], -0.449, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Macd24", macd.MacdDiff[0xfb], -0.501, 0.0005);
            Momentum momentum = new Momentum(quotes);
            momentum.Period = 4;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Mom00", momentum[0], 0.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Mom01", momentum[1], 3.315, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Mom02", momentum[3], 3.595, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Mom03", momentum[0x10], -9.345, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Mom04", momentum[0xfb], -1.88, 0.0005);
            Rsi rsi = new Rsi(quotes);
            rsi.Period = 10;
            rsi.Smooth = 3;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi00", rsi[0], 50.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi01", rsi[1], 50.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi02", rsi[3], 90.168, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi03", rsi[0x10], 43.526, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi04", rsi[0xfb], 42.143, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi10", rsi.RsiAvg[0], 50.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi11", rsi.RsiAvg[1], 50.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi12", rsi.RsiAvg[3], 79.654, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi13", rsi.RsiAvg[0x10], 45.078, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Rsi14", rsi.RsiAvg[0xfb], 47.771, 0.0005);
            Sum sum = new Sum(quotes);
            sum.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sum00", sum[0], 91.5, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sum01", sum[1], 186.315, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sum02", sum[3], 375.785, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sum03", sum[0x10], 930.255, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Sum04", sum[0xfb], 1091.12, 0.0005);
            StochasticsFast fast = new StochasticsFast(quotes);
            fast.PeriodK = 10;
            fast.PeriodD = 3;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF00", fast.K[0], 30.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF01", fast.K[1], 97.017, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF02", fast.K[3], 77.244, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF03", fast.K[0x10], 18.68, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF04", fast.K[0xfb], 28.153, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF10", fast.D[0], 30.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF11", fast.D[1], 71.973, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF12", fast.D[3], 77.947, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF13", fast.D[0x10], 32.932, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF14", fast.D[0xfb], 53.648, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF20", fast[0], 30.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF21", fast[1], 71.973, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF22", fast[3], 77.947, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF23", fast[0x10], 32.932, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " StochF24", fast[0xfb], 53.648, 0.0005);
            Stochastics stochastics = new Stochastics(quotes);
            stochastics.PeriodK = 10;
            stochastics.PeriodD = 3;
            stochastics.Smooth = 6;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch00", stochastics.K[0], 30.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch01", stochastics.K[1], 71.973, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch02", stochastics.K[3], 77.947, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch03", stochastics.K[0x10], 32.932, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch04", stochastics.K[0xfb], 53.648, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch10", stochastics.D[0], 30.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch11", stochastics.D[1], 50.987, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch12", stochastics.D[3], 62.114, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch13", stochastics.D[0x10], 53.85, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch14", stochastics.D[0xfb], 54.242, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch20", stochastics[0], 30.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch21", stochastics[1], 50.987, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch22", stochastics[3], 62.114, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch23", stochastics[0x10], 53.85, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Stoch24", stochastics[0xfb], 54.242, 0.0005);
            Volume volume = new Volume(quotes);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Vol00", volume[0], 4077500.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Vol01", volume[1], 4955900.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Vol02", volume[3], 4155300.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Vol03", volume[0x10], 8920300.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Vol04", volume[0xfb], 2870500.0, 0.0005);
            Tsi tsi = new Tsi(quotes);
            tsi.Fast = 3;
            tsi.Slow = 14;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Tsi00", tsi[0], 0.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Tsi01", tsi[1], 100.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Tsi02", tsi[3], 80.208, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Tsi03", tsi[0x10], -15.3, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Tsi04", tsi[0xfb], -7.324, 0.0005);
            Adx adx = new Adx(quotes);
            adx.Period = 10;
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Adx00", adx[0], 50.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Adx01", adx[1], 100.0, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Adx02", adx[3], 61.29, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Adx03", adx[0x10], 23.991, 0.0005);
             iTrading.Test.Globals.AssertEquals(base.GetType().FullName + " Adx04", adx[0xfb], 8.349, 0.0005);
            base.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
        }

        private class QuotesHolder
        {
            public iTrading.Core.Data .Quotes quotes = null;
        }
    }
}

