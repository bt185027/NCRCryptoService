using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace INCRCryptoService
{
    /// <summary>
    /// NCR crypto service or degital intarface can be used by the ATM applicaiton to register/connect 
    /// the current ATM user and perfrom transactions like sell users account crypto currency to encash for 
    /// a withdrawal or buy the crypto with convienience like a simple transfer.
    /// </summary>
    [XmlSerializerFormat]
    [ServiceContract]
    public interface INCRCryptoService
    {
        /// <summary>
        /// Connects the current ATM user to the crypto service.
        /// </summary>
        /// <param name="cardNumber">user card number</param>
        /// <returns></returns>
        [DataContractFormat]
        [OperationContract]
        UserCryptoAccountInfo ConnectUser(string cardNumber);

        /// <summary>
        /// Closes the user connection
        /// </summary>
        [OperationContract]
        void Close();

        /// <summary>
        /// Registers a new user into the NCR digital crypto service.
        /// </summary>
        /// <remarks> Need to be called by the banks regiatration webpage.</remarks>
        [OperationContract]
        Status RegisterUser(string cardNumber);

        
        /// <summary>
        /// Get the current price of the crypto
        /// </summary>
        /// <param name="coinId">coin id.</param>
        /// <returns></returns>
        [OperationContract]
        decimal GetCryptoPrice(string coinId);

        /// <summary>
        /// Buys the crypto currency for the current user.
        /// </summary>
        /// <param name="coinId">crypto currency id (ex : BTCUSD for bitcoin)</param>
        /// <param name="amount">crypto currency in equivalent amount</param>
        /// <returns>success/fail</returns>
        [OperationContract]
        Status BuyCrypto(string coinId, float amount);

        /// <summary>
        /// Sells the crypto currency for the current user.
        /// </summary>
        /// <param name="coinId">crypto currency id (ex : BTCUSD for bitcoin)</param>
        /// <param name="amount">crypto currency in equivalent amount</param>
        /// <returns>success/fail</returns>
        [OperationContract]
        Status SellCrypto(string coinId, float amount);
    }

    /// <summary>
    /// Users crypto account information.
    /// </summary>
    [DataContract]
    public class UserCryptoAccountInfo
    {
        UserHoldings currencyHoldings = new UserHoldings();
        
        /// <summary>
        /// Users current holdings
        /// </summary>
        [DataMember]
        public UserHoldings UserCurrentHoldings
        {
            get { return currencyHoldings; }
        }


        /// <summary>
        /// User card number.
        /// </summary>
        [DataMember]
        public string CardNumber
        { get; set; }
        
    }

    [DataContract]
    public class UserHoldings
    {

        [DataMember]
        public string UsersID;

        [DataMember]
        public List<CryptoInfo> Holdings;

    }

    [DataContract]
    public class CryptoInfo
    {   
        [DataMember]
        public string CryptoID { get; set; }

        [DataMember]
        public float HoldingValue { get; set; }
    }

    public enum Status
    {
       Success,
       Failed
    }
}
