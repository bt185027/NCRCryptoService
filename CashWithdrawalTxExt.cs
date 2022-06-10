// -----------------------------------------------------------------------------
//  $Source: WithdrawalAdapterBNC.cs $
//  Description : 
// -----------------------------------------------------------------------------
//  Copyright 2015 NCR Corporation
//  Financial Software Engineering
// -----------------------------------------------------------------------------
//  All revision information is updated automatically from source code control
//  change records - please do not manually edit.
// -----------------------------------------------------------------------------

#region

using Ncr.Connections.Cjsa.Core;
using Ncr.Connections.Client.Browser;
using NCR.APTRA.ActivateCjsa.Core;
using NCR.APTRA.ClassConfiguratorCore;
using NCR.APTRA.IBusDat;
using NCR.APTRA.IResMan;
using NCR.APTRA.ISTX;
using NCR.APTRA.PDCollection;
using NCR.APTRA.WithdrawalTx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion

namespace ncr.cjsa.WithdrawalTxBNC
{
    /// <summary>
    ///     (COM visible)a cjsa withdrawal transmit.
    /// </summary>
    [ConfigurableClass(ConfigurableClassAttribute.ClassCategory.ChannelApplication,
        ConfigurableClassAttribute.ClassSubType.Transaction)]
    [ComVisible(true)]
    public class CashWithdrawalTxExt : BaseService
    {
        #region MemberVairiables


        private NativeCashWithdrawalTxExt nativeCashWithdrawalTxExt;
        private readonly StdSupplyPoint sp;

        /// <summary>
        /// The cjsa record to be passed to connections
        /// </summary>
        private object cjsaRecord;

        /// <summary>
        ///     Identifier for the serv.
        /// </summary>
        private const string ServId = "ncr.cjsa.WithdrawalTx";

        /// <summary>
        ///     The serv version.
        /// </summary>
        private const string serviceVersion = "1.0.0";


        /// <summary>
        ///     The execute event handler.
        /// </summary>
        private object callback;

        /// <summary>
        ///     The transaction.
        /// </summary>
        private WithdrawalTransaction transaction;

        #endregion

        #region Constructor/Initializers

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public CashWithdrawalTxExt()
            : base(ServId, serviceVersion)
        {
            sp = new StdSupplyPoint(NativeCashWithdrawalTxExt.Responsibility, NativeCashWithdrawalTxExt.Owner, nameof(CashWithdrawalTxExt));
            sp.InterfaceEntry(nameof(CashWithdrawalTxExt));

            nativeCashWithdrawalTxExt = new NativeCashWithdrawalTxExt();

            sp.InterfaceExit(nameof(CashWithdrawalTxExt));
        }

        [Initialiser]
        public void Initialise()
        {
            sp.InterfaceEntry(nameof(Initialise));

            nativeCashWithdrawalTxExt.ActivteTransaction = transaction;

            sp.InterfaceExit(nameof(Initialise));
        }




        #endregion

        #region Configuration Properties

        /// <summary>
        /// Holds the BNC withdrawal Transaction obejct
        /// </summary>
        /// <value>
        ///     The transaction.
        /// </value>
        [ComVisible(false)]
        [ConfigurationProperty, Mandatory]
        public WithdrawalTransaction Transaction
        {
            set
            {
                if (transaction == null)
                {
                    transaction = value;
                }
            }
        }

        [ConfigurationProperty, Mandatory]
        [ComVisible(false)]
        public IJavaScriptInterop JavaScriptInterop { get; set; }

        [ConfigurationProperty, Mandatory]
        [ComVisible(false)]
        public IJavaScriptInputValidator JavaScriptInputValidator { get; set; }

        #endregion

        #region BNC Extension

        /// <summary>
        ///	Serves as a wrapper for the WithdrawalTransaction.ValidateAmount() which behind the scenes
        /// fills the RecommendedNoteMixes property
        /// </summary>
        /// <returns>
        /// 0  Valid amount
        /// 1  Too low to dispense
        /// 2  Too high to dispense
        /// 3  Not dispensable amount
        /// </returns>
        public object validateAmount()
        {
            //Validation produces the NoteMixes
            WithdrawalTxAmountCheckOutcome outcome = nativeCashWithdrawalTxExt.validateAmount();

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
                return (nativeCashWithdrawalTxExt.recommendedNoteMixCount);
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
            var noteMix = transaction.RecommendedNoteMixes[position];
            var mix = JavaScriptInterop.ConvertToJsArray(noteMix.CassetteNoteMix);

            return noteMix;
        }

        /// <summary>
        ///     Gets the recommended note mixes.
        /// </summary>
        public object recommendedNoteMixes
        {
            get
            {
                var mixArray = transaction.RecommendedNoteMixes;

                IEnumerable<CjsaNoteMix> mixList = null;

                if ((mixArray != null) && (mixArray.Length > 0))
                {
                    mixList = mixArray.Select(mix => new CjsaNoteMix(mix, JavaScriptInterop));
                }

                if (mixList == null) return DBNull.Value;

                var mixes = JavaScriptInterop.ConvertToJsArray(mixList.ToArray());
                return mixes;
            }
        }


        #endregion
    }
}