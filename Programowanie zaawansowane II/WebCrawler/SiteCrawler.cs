using System;
using System.Net;

namespace WebCrawler
    { 

    /// <summary>
    /// Allows to crawl through the given site concurrently.
    /// </summary>

    internal class SiteCrawler
        {

        private string siteURL;
        private uint levelOfDepth;
        private StdErrFlow.ExceptionInfo lastExceptionInfo;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. Initializes the fields.
        /// </summary>

        public SiteCrawler()
            {
            this.siteURL = string.Empty;
            this.levelOfDepth = 3;

            this.lastExceptionInfo.typeName = string.Empty;
            this.lastExceptionInfo.methodName = string.Empty;
            this.lastExceptionInfo.argument = string.Empty;
            this.lastExceptionInfo.causeEvent = string.Empty;
            this.lastExceptionInfo.message = string.Empty;
            this.lastExceptionInfo.id = string.Empty;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Setter. Alters the field only if the given string is an absolute URL.
        /// </summary>
        /// <param name="url">A string with link to check.</param>
        /// <returns>'true' if the field has been modified, 'false' otherwise.</returns>

        public bool setSiteURL( string url )
            {
            if ( isAbsoluteURL( url ) == true ) {
                this.siteURL = url;
                return ( true );
                }

            return ( false );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Checks whether the given string instance is a valid URL or not. Valid means an absolute URI.
        /// </summary>
        /// <param name="url">A string with the URI to check.</param>
        /// <returns>true if it is well formed URI, otherwise false.</returns>

        public static bool isAbsoluteURL( string url )
            {
            Uri uriResult;

            return ( Uri.TryCreate(url, UriKind.Absolute, out uriResult ) && 
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>The current content of the name corresponding field of the used object.</returns>

        public string getSiteURL()
            {
            return ( this.siteURL );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Setter.
        /// </summary>
        /// <param name="level">A number representing the level of depth for the crawling procedure.</param>

        public void setLevelOfDepth( uint level )
            {
            this.levelOfDepth = level;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>The current content of the name corresponding field of the used object.</returns>

        public uint getLevelOfDepth()
            {
            return ( this.levelOfDepth );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>A 'struct' with the data about the last exception occurence.</returns>

        public StdErrFlow.ExceptionInfo getLastExceptionInfo()
            {
            return ( this.lastExceptionInfo );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Probing the network connection with the object's given URL. This method works in a synchronous way.
        /// </summary>
        /// <returns>'true' if connection has been successfully established, 'false' otherwise.</returns>

        public bool probeNetworkConnection()
            {
            string url = this.getSiteURL();

            try {
                if ( url == string.Empty ) {
                    throw ( new ArgumentException( "The site URL is an empty string." ) );
                    }

                WebClient website = new WebClient();
                string content = website.DownloadString( url );
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Probing the network connection.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                return ( false );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Probing the network connection.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                return ( false );
                }
            catch ( WebException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Probing the network connection.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                return ( false );
                }
            catch ( NotSupportedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Probing the network connection.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                return ( false );
                }            
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Probing the network connection.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Debug test code.
        /// </summary>

        static void Main()
            {
            SiteCrawler sc = new SiteCrawler();
            sc.probeNetworkConnection();
            StdErrFlow.ExceptionInfo info = sc.getLastExceptionInfo();

            Console.WriteLine( "typeName = " + info.typeName );
            Console.WriteLine( "methodName = " + info.methodName );
            Console.WriteLine( "argument = " + info.argument );
            Console.WriteLine( "causeEvent = " + info.causeEvent );
            Console.WriteLine( "message = " + info.message );
            Console.WriteLine( "id = " + info.id );
            Console.ReadLine();
            }

        //______________________________________________________________________________________________________________________________

        }
    }