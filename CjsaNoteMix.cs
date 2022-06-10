
using System.Runtime.InteropServices;


using Ncr.Connections.Client.Browser;
using NCR.APTRA.IBusDat;

namespace Ncr.Connections.Cjsa.CashWithdrawalTxBNC
{
    [ComVisible(true)]
    public class CjsaNoteMix
    {
        public CjsaNoteMix ( IDispensableCashNoteMix noteMix, IJavaScriptInterop jsInterop)
        {
            mixId = noteMix.MixID;

            CjsaDenomCount[] cjsaMix = new CjsaDenomCount[noteMix.NumberOfDenominations];
            for (int i = 0; i < noteMix.NumberOfDenominations; i++)
            {
                cjsaMix[i] = new CjsaDenomCount(noteMix.GetDenominationValue(i), noteMix.GetDenominationCount(i));
            }

            mix = jsInterop.ConvertToJsArray(cjsaMix);

        }
        public string mixId { get; private set; }

        public object mix { get; private set; }
    }

    [ComVisible(true)]
    public class CjsaDenomCount
    {
        public CjsaDenomCount(IAmount denom, int number)
        {
            demomination = new CjsaAmount(denom.CurrencyID,denom.Value);
            count = number;
        }
        
        public CjsaAmount demomination { get; private set; }

        public int count { get; private set; }
    }

}
