using Ncr.Connections.Cjsa.Core;
using NCR.APTRA.ClassConfiguratorCore;
using NCR.APTRA.PDCollection;
using NCR.APTRA.WithdrawalTx;
using System.IO;
using System.Reflection;


namespace Ncr.Connections.Cjsa.CashWithdrawalTxBNC
{ [ConfigurableClass(ConfigurableClassAttribute.ClassCategory.ChannelApplication, ConfigurableClassAttribute.ClassSubType.ConsumerService)]
    public class NativeCashWithdrawalTxExt : BaseNativeService, IPublishCjsaServiceJavaScript
    {
        #region Problem Determination

        internal const string Responsibility = "Connections.Ncr.Cjsa";
        internal const string Owner = "CashWithdrawalTx";
        internal const string ServiceId = "ncr.cjsa.WithdrawalTxBNC";
        internal const string ServiceVersion = "1.0.0";
        private readonly StdSupplyPoint sp;

        #endregion

        #region MemberVariables

        private WithdrawalTransaction activteTransaction;


        #endregion

        #region Constructor

        public NativeCashWithdrawalTxExt() : base(ServiceId, ServiceVersion)
        {
            sp = new StdSupplyPoint(Responsibility, Owner, nameof(NativeCashWithdrawalTxExt));
            sp.InterfaceEntry(nameof(NativeCashWithdrawalTxExt));
            sp.AlwaysOnNamedValues(nameof(NativeCashWithdrawalTxExt), nameof(NativeCashWithdrawalTxExt), AlwaysOnCategory.Other,
                ServiceId, "ServiceId",
                ServiceVersion, "ServiceVersion");
            sp.InterfaceExit(nameof(NativeCashWithdrawalTxExt));
        }

        #endregion
      
        #region IPublishCjsaServiceJavaScript


        private const string nonMinimizedMixedMediaJavaScriptResource = "Ncr.Connections.Cjsa.CashWithdrawalTxBNC.html_resources.dist.withdrawalTxExtProxy.js";
        private const string minimizedMixedMediaJavaScriptResource = "Ncr.Connections.Cjsa.CashWithdrawalTxBNC.dist.withdrawalTxExtProxy.min.js";


        /// <summary>
        /// Returns the JavaScript that will be injected into the browser to access the service using json serialisation.
        /// </summary>
        /// <param name="useNonMinimized">Controls if the minimized or non-minimised service is returned. IF the service does not support the
        /// version requested the available version will be returned</param>
        /// <returns>JavaScript to inject into browser to access the service using json serialisation</returns>
        public string GetJavaScriptService(bool useNonMinimized)
        {
            sp.InterfaceEntry(nameof(GetJavaScriptService), useNonMinimized);

            string resourceName = useNonMinimized ? nonMinimizedMixedMediaJavaScriptResource : minimizedMixedMediaJavaScriptResource;

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            StreamReader streamReader = new StreamReader(thisAssembly.GetManifestResourceStream(resourceName));
            var script = string.Format(@"({0})('{1}','{2}');", streamReader.ReadToEnd(), GetServiceId(), GetServiceVersion());

            sp.InterfaceExit(nameof(GetJavaScriptService));

            return script;
        }
        #endregion


        #region Configurable Properties

        [ConfigurationProperty, Mandatory]
        public WithdrawalTransaction ActivteTransaction { get; set; }


        #endregion

        #region BNC Custom functions



        public object validateAmount(int amount)
        {
            sp.InterfaceEntry(nameof(validateAmount));

            //Validation produces the NoteMixes
            ActivteTransaction.Amount = amount;
            object outcome = ActivteTransaction.ValidateAmount();

            sp.InterfaceExit(nameof(validateAmount));

            return outcome;
        }

        /// <summary>
        ///     Gets the number of recommended note mixes.
        /// </summary>
        /// <value>
        ///     The recommended note mixes.
        /// </value>
        public int recommendedNoteMixCount
        {
            get
            {
                sp.InterfaceEntry(nameof(recommendedNoteMixCount));
                int count = (ActivteTransaction.RecommendedNoteMixes == null) ? 0 : ActivteTransaction.RecommendedNoteMixes.Length;
                sp.InterfaceExit(nameof(recommendedNoteMixCount),count);
                return count;
            }
        }

        /// <summary>
        ///     Gets a specific recommended note mix
        /// </summary>
        /// <value>
        ///     The recommended note mix.
        /// </value>
        public object recommendedNoteMix(int position)
        {
            sp.InterfaceEntry(nameof(recommendedNoteMix));
            
            var noteMixes = ActivteTransaction.RecommendedNoteMixes[position];

            sp.InterfaceExit(nameof(recommendedNoteMix));

            return noteMixes;
        }

        /// <summary>
        /// Sets ReceiptRequested on TX object.
        /// </summary>
        public void  setReceiptRequested(bool receiptRequested)
        {
            sp.InterfaceEntry(nameof(setReceiptRequested));

            ActivteTransaction.ReceiptRequested = receiptRequested;
           
            sp.InterfaceExit(nameof(setReceiptRequested));
        }

        #endregion
    }
}
