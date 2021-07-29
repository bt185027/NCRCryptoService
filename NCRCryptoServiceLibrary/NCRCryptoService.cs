using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using INCRCryptoService;
using Newtonsoft.Json;

namespace NCRCryptoServiceLibrary
{
    [Serializable]
    [ComVisible(true)]
    public class NCRCryptoService : INCRCryptoService.INCRCryptoService
    {
        Dictionary<string, UserCryptoAccountInfo> users = new Dictionary<string, UserCryptoAccountInfo>();

        UserCryptoAccountInfo currentUser = null;

        public Status NewToCryptoWorld(string cardNumber, string apiKey, string secretKey)
        {
            Status retVal = Status.Failed;

            if (!users.ContainsKey(cardNumber))
            {
                UserCryptoAccountInfo user = new UserCryptoAccountInfo();
                user.CardNumber = cardNumber;
                user.HashedApiKey = Utilities.EncryptIt(apiKey);
                user.HashedSecretKey = Utilities.EncryptIt(secretKey);
                users.Add(cardNumber, user);

                retVal = Status.Success;
            }
            else
            {
                // existing user
            }

            return retVal;
        }

        public string ConnectMeToCryptoWorld(string userIdentification)
        {
            string userDetailsjson = string.Empty;

            currentUser = users[userIdentification];

            if(currentUser.CurrencyHoldings!=null)
            {
              IDictionary<string,double> dict = currentUser.CurrencyHoldings;
              userDetailsjson =  JsonConvert.SerializeObject(dict);
            }
            return userDetailsjson;
        }

        public void ByeForNow()
        {
            currentUser.SaveDate();
            currentUser = null;
        }

        public Status GrabSomeCrypto(string coinId, double amount)
        {
            FetchCoinValue(coinId);
            double quantity = amount/currentPrice;
            Utilities.AddHoldings(currentUser, coinId, quantity);
            return Status.Success; ;
        }

        public Status EncashCrypto(string coinId, double amount)
        {
            FetchCoinValue(coinId);
            double quantity =  amount / currentPrice;
            Utilities.RemoveHoldings(currentUser, coinId, quantity);
            return Status.Success;
        }

        double currentPrice = 0;

        public double FetchCoinValue(string coinId)
        {
            string json;

            using (var web = new System.Net.WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var newurl = @"https://min-api.cryptocompare.com/data/price?fsym=" + coinId + "&tsyms=USD";
                json = web.DownloadString(newurl);
            }

            dynamic obj = JsonConvert.DeserializeObject(json);
            currentPrice = Convert.ToDouble(obj.USD);

            return currentPrice;
        }
    }

    //[DataContract]
    //public class UserCryptoAccountInfo : IUserCryptoAccountInfo
    //{
      
    //    IDictionary<string, decimal> currencyHoldings = new Dictionary<string, decimal>();

    //    //public int this[int index] 
    //    //{ get;set; }
    //    [DataMember]
    //    public IDictionary<string, decimal> CurrencyHoldings
    //    {
    //        get { return currencyHoldings; }
    //    }

    //    [DataMember]
    //    public string CardNumber
    //    { get; set; }

    //    internal void AddHoldings(string coinID, decimal value)
    //    {
    //        if (!currencyHoldings.ContainsKey(coinID))
    //        {
    //            currencyHoldings[coinID] = value;
    //        }
    //        else
    //        {
    //            currencyHoldings[coinID] += value;
    //        }
    //    }
    //    internal void RemoveHoldings(string coinID, decimal value)
    //    {
    //        if (!currencyHoldings.ContainsKey(coinID))
    //        {
    //            //do nothing
    //        }
    //        else
    //        {
    //            currencyHoldings[coinID] -= value;
    //        }
    //    }
    //}
}
