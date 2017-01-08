using System;

namespace WebCrawler
    {

    /// <summary>
    /// Aggregates procedures for dealing with the user interface control and behavior of performed actions.
    /// </summary>

    internal class UIWork
        {

        private static StdErrFlow.ExceptionInfo lastExceptionInfo;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. There is no point to instantiate this class.
        /// </summary>

        private UIWork()
            {
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>A 'struct' with the data about the last exception occurence.</returns>

        public static StdErrFlow.ExceptionInfo getLastExceptionInfo()
            {
            return ( lastExceptionInfo );
            }

        //______________________________________________________________________________________________________________________________

        //______________________________________________________________________________________________________________________________

        //______________________________________________________________________________________________________________________________

        //______________________________________________________________________________________________________________________________

        }
    }
