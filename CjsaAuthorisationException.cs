// -----------------------------------------------------------------------------
//  $Source: CjsaAuthorisationException.cs $
//  Description : 
// -----------------------------------------------------------------------------
//  Copyright 2015 NCR Corporation
//  Financial Software Engineering
// -----------------------------------------------------------------------------
//  All revision information is updated automatically from source code control
//  change records - please do not manually edit.
// -----------------------------------------------------------------------------

#region

using System.Runtime.InteropServices;

#endregion

namespace ncr.cjsa.WithdrawalTxBNC
{
    /// <summary>
    /// (COM visible)exception for signalling cjsa authorisation errors.
    /// </summary>
    [ComVisible(true)]
    public class CjsaAuthorisationException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="excptn"> The excptn. </param>
        /// <param name="msgId">  Identifier for the message. </param>
        public CjsaAuthorisationException(string excptn, string msgId)
        {
            exception = excptn;
            consumerMessageId = msgId;
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        ///
        /// <value>
        /// The exception.
        /// </value>
        public string exception { get; private set; }

        /// <summary>
        /// Gets or sets the identifier of the consumer message.
        /// </summary>
        ///
        /// <value>
        /// The identifier of the consumer message.
        /// </value>
        public string consumerMessageId { get; private set; }
    }
}
