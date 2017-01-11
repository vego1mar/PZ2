using System;
using System.Collections.Generic;

namespace WebCrawler
    { 

    /// <summary>
    /// Allows to crawl through the given site concurrently using recursion.
    /// </summary>

    internal class SiteCrawler
        {

        internal const string CRAWLED_WEBSITES_FILE_EXTENSION = "html";

        private string siteURL;
        private string probedSiteContent;

        private uint levelOfDepth;
        private bool useAsynchronousDownload;
        private StdErrFlow.ExceptionInfo lastExceptionInfo;
        private int maximumRunningTasksPerDepthEntry;


        //______________________________________________________________________________________________________________________________

        public class SiteCrawlerEventArgs : EventArgs
            {
            private int numberOfFoundedLinks = 0;

            public SiteCrawlerEventArgs( int linksFounded ) {
                this.numberOfFoundedLinks = linksFounded;
                }

            public int geNumberOfFoundedLinks() {
                return ( this.numberOfFoundedLinks );
                }
            }

        public delegate void SiteCrawlerEventHandler( object source, SiteCrawlerEventArgs e );
        public event SiteCrawlerEventHandler NewSetOfLinksFounded;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. Initializes the fields.
        /// </summary>

        public SiteCrawler()
            {
            this.siteURL = string.Empty;
            this.probedSiteContent = string.Empty;
            this.levelOfDepth = 2;
            this.useAsynchronousDownload = false;

            this.lastExceptionInfo.typeName = string.Empty;
            this.lastExceptionInfo.methodName = string.Empty;
            this.lastExceptionInfo.argName = string.Empty;
            this.lastExceptionInfo.argValue = string.Empty;
            this.lastExceptionInfo.message = string.Empty;
            this.lastExceptionInfo.id = string.Empty;

            this.maximumRunningTasksPerDepthEntry = 5;
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
                this.probedSiteContent = string.Empty;
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
        /// <returns>The website content for the given URL if the connection has been probed.</returns>

        public string getProbedSiteContent()
            {
            return ( this.probedSiteContent );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>'true' if websites will be downloaded asynchronously, 'false' otherwise.</returns>

        public bool getAsynchronousDownloadUse()
            {
            return ( this.useAsynchronousDownload );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Setter. Using asynchronous download may cause an abruptly termination of the crawling procedure.
        /// </summary>
        /// <param name="state">A new state of the flag for the crawling procedure.</param>

        public void setAsynchronousDownloadUse( bool state )
            {
            this.useAsynchronousDownload = state;
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
        /// On success, the site content will be saved to the proper object's field.
        /// </summary>
        /// <returns>'true' if connection has been successfully established, 'false' otherwise.</returns>

        public bool probeNetworkConnection()
            {
            string url = this.getSiteURL();

            try {
                if ( url == string.Empty ) {
                    throw ( new ArgumentException( "The site URL is an empty string." ) );
                    }

                string content = new System.Net.WebClient().DownloadString( url );
                this.probedSiteContent = content;
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                return ( false );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                return ( false );
                }
            catch ( System.Net.WebException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                return ( false );
                }
            catch ( NotSupportedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                return ( false );
                }            
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Retrieves only the links from tags of the given argument. It absorbses only the distinct matches.
        /// </summary>
        /// <returns>A collection with links grabbed from the parameter.</returns>

        public static ISet<string> getOnlyTheLinks( string websiteContent )
            {
            // A regex for the whole <a> tags content: "<a.*?>(.*?)<\\/a>"
            var regex = new System.Text.RegularExpressions.Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");
            ISet<string> newLinks = new HashSet<string>();

            // Preventing an ArgumentNullException raising using Regex.Matches().
            if ( websiteContent != null ) {
                foreach ( var match in regex.Matches( websiteContent ) ) {
                    if ( newLinks.Contains( match.ToString() ) == false ) {
                        newLinks.Add( match.ToString() );
                        }
                    }
                }

            return ( newLinks );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Traverse the given collection and searches for absolute links. It absorbs only the distinct entries.
        /// </summary>
        /// <param name="hrefLinks">A set of links.</param>
        /// <returns>A collection with the absolute links only.</returns>

        public static ISet<string> retrieveAbsoluteLinks( ISet<string> hrefLinks )
            {
            ISet<string> absoluteLinks = new HashSet<string>();

            foreach ( var entry in hrefLinks ) {
                if (( isAbsoluteURL( entry ) == true ) && ( absoluteLinks.Contains( entry ) == false )) {
                    absoluteLinks.Add( entry );
                    }
                }

            return ( absoluteLinks );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Crawls through the site gained from the field using setted level of depth.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the website URL has not been gained yet.</exception>

        public void crawlThroughSite()
            {
            string mainSiteURL = this.getSiteURL();

            if ( mainSiteURL == string.Empty ) {
                throw ( new ArgumentNullException( mainSiteURL.GetType().Name, "The website URL is empty." ) );
                }

            string mainSiteDirectoryName = this.determineQualifiedPath( FileSystemFlow.createRootDirectory(), mainSiteURL );
            ISet<string> absoluteLinks0 = this.performBasicCrawlingStep( mainSiteURL, mainSiteDirectoryName );
            this.performLevelCrawlingStep( absoluteLinks0, mainSiteDirectoryName, 1 );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Performs the recursive crawling.
        /// </summary>
        /// <param name="absoluteLinks0">A set of absolute links from the crawling-starting URL website content.</param>
        /// <param name="rootDirectoryName">The root directory full path.</param>
        /// <param name="levelOfDepthEntry">The current depth of the recursive entry.</param>

        private void performLevelCrawlingStep( ISet<string> absoluteLinks0, string rootDirectoryName , uint levelOfDepthEntry = 0 )
            {
            if ( levelOfDepthEntry > this.getLevelOfDepth() ) {
                return;
                }

            if ( levelOfDepthEntry == this.getLevelOfDepth() ) {
                this.NewSetOfLinksFounded?.Invoke( this, new SiteCrawlerEventArgs( absoluteLinks0.Count ) );
                }

            try {
                // foreach ( var urlEntry in absoluteLinks0 ) {
                System.Threading.Tasks.Parallel.ForEach( 
                    absoluteLinks0, 
                    new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = this.getMaximumRunningTasksPerDepthEntry() }, 
                    urlEntry => {
                        string directoryName = System.IO.Path.Combine( rootDirectoryName, "lvl_" + levelOfDepthEntry.ToString() );
                        ISet<string> absoluteLinks1 = this.performBasicCrawlingStep( urlEntry, directoryName );
                        this.performLevelCrawlingStep( absoluteLinks1, rootDirectoryName, levelOfDepthEntry + 1 );
                        }
                    );
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = absoluteLinks0.GetType().Name + ".Count";
                this.lastExceptionInfo.argValue = absoluteLinks0.Count.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = absoluteLinks0.GetType().Name + ".Count";
                this.lastExceptionInfo.argValue = absoluteLinks0.Count.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = absoluteLinks0.GetType().Name + ".Count";
                this.lastExceptionInfo.argValue = absoluteLinks0.Count.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( AggregateException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = absoluteLinks0.GetType().Name + ".Count";
                this.lastExceptionInfo.argValue = absoluteLinks0.Count.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( OperationCanceledException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = absoluteLinks0.GetType().Name + ".Count";
                this.lastExceptionInfo.argValue = absoluteLinks0.Count.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = absoluteLinks0.GetType().Name + ".Count";
                this.lastExceptionInfo.argValue = absoluteLinks0.Count.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = absoluteLinks0.GetType().Name + ".Count";
                this.lastExceptionInfo.argValue = absoluteLinks0.Count.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Performes a basic step for the crawling procedure. 
        /// (1) Connects with the website using an absolute URL. 
        /// (2) Retrieves a set of an absolute links founded in the website content behind the given URL. 
        /// (3) Saves the website content to the specified directory and with the specified file name.
        /// </summary>
        /// <param name="url">An absolute URL to proceed for.</param>
        /// <param name="directoryName">A target directory full path for saving the website content.</param>
        /// <returns>A set of absolute links retrieved from the website content of the given URL.</returns>

        private ISet<string> performBasicCrawlingStep( string url, string directoryName )
            {
            string websiteContent = string.Empty;

            if ( this.getAsynchronousDownloadUse() == true ) {
                websiteContent = this.downloadWebsiteContentAsynchronously( url );
                }
            else {
                websiteContent = this.downloadWebsiteContent( url );
                }

            if ( websiteContent != string.Empty ) {
                FileSystemFlow.createDirectory( directoryName );
                string filePath = this.determineQualifiedPath( directoryName, url + "." + CRAWLED_WEBSITES_FILE_EXTENSION );
                FileSystemFlow.saveTextToFile( filePath, websiteContent );
                }

            ISet<string> absoluteLinks = this.extractAbsoluteLinksFrom( websiteContent );
            return ( absoluteLinks );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Extracts the absolute links from the given website content.
        /// </summary>
        /// <param name="websiteContent">The website content for retrieving the absolute links set from.</param>
        /// <returns>A set of absolute links retrieved from the given website content.</returns>

        private ISet<string> extractAbsoluteLinksFrom( string websiteContent )
            {
            ISet<string> hrefLinks = getOnlyTheLinks( websiteContent );
            ISet<string> absoluteLinks = retrieveAbsoluteLinks( hrefLinks );
            return ( absoluteLinks );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Downloads the HTML content from a website given as an URL using synchronous method.
        /// </summary>
        /// <param name="url">An absolute URL to connect with for downloading the website content.</param>
        /// <returns>The website content behind the passed URL.</returns>

        private string downloadWebsiteContent( string url )
            {
            string websiteContent = string.Empty;

            try {
                websiteContent = new System.Net.WebClient().DownloadString( url );
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( NotSupportedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( System.Net.WebException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }

            return ( websiteContent );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Downloads the HTML content from a website given as an URL using asynchronous method.
        /// </summary>
        /// <param name="url">An absolute URL to connect with for downloading the website content.</param>
        /// <returns>The website content behind the passed URL.</returns>

        private string downloadWebsiteContentAsynchronously( string url )
            {
            string websiteContent = string.Empty;

            try {
                Uri uri = new Uri( url );
                System.Net.WebClient client = new System.Net.WebClient();
                bool isDownloadFinished = false;
                System.Reflection.TargetInvocationException downloadException = null;

                client.DownloadStringCompleted += delegate ( object sender, System.Net.DownloadStringCompletedEventArgs e ) {
                    // Preventing an internal exception raising.
                    if ( ( e.Cancelled == false ) && ( e.Error == null ) ) {
                        websiteContent = e.Result;
                        isDownloadFinished = true;
                        }
                    else {
                        downloadException = new System.Reflection.TargetInvocationException( e.Error.Message, e.Error );
                        }
                    };

                client.DownloadStringAsync( uri );

                while ( isDownloadFinished == false ) {
                    if ( downloadException != null ) {
                        throw ( downloadException );
                        }
                    }
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( UriFormatException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( System.Net.WebException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( System.Reflection.TargetInvocationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = url.GetType().FullName + "~" + nameof( url );
                this.lastExceptionInfo.argValue = url.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }

            return ( websiteContent );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Determines a qualified path. 
        /// Qualified means without reserved to the Windows file system characters and with the proper length. 
        /// If a combined path would be too long, the website URL name will be trimmed from the left side.
        /// </summary>
        /// <param name="directoryName">A directory name of the target destination location.</param>
        /// <param name="websiteURL">An untrusted name.</param>
        /// <returns>The Windows operating system qualified directory path.</returns>

        private string determineQualifiedPath( string directoryName, string websiteURL )
            {
            string qualifiedLengthPath = string.Empty;
            string mainSiteDirectoryName = string.Empty;

            try {
                string websiteName = FileSystemFlow.removeWindowsFileSystemReservedCharacters( websiteURL.Substring( websiteURL.IndexOf( '.' ) + 1 ) );
                websiteName = FileSystemFlow.limitCharactersToFirst( FileSystemFlow.WINDOWS_QUALIFIED_FILENAME_LENGTH - 1, websiteName );
                mainSiteDirectoryName = System.IO.Path.Combine( directoryName, websiteName );

                if ( mainSiteDirectoryName.Length > (FileSystemFlow.WINDOWS_QUALIFIED_DIRECTORY_LENGTH - 1) ) {
                    int trimmingSize = mainSiteDirectoryName.Length - FileSystemFlow.WINDOWS_QUALIFIED_DIRECTORY_LENGTH;
                    websiteName = websiteName.Substring( trimmingSize + 1 );
                    mainSiteDirectoryName = System.IO.Path.Combine( directoryName, websiteName );
                    }

                qualifiedLengthPath = mainSiteDirectoryName;
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = mainSiteDirectoryName.GetType().FullName + "~" + nameof( mainSiteDirectoryName );
                this.lastExceptionInfo.argValue = mainSiteDirectoryName.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-5]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = mainSiteDirectoryName.GetType().FullName + "~" + nameof( mainSiteDirectoryName );
                this.lastExceptionInfo.argValue = mainSiteDirectoryName.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-5]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = mainSiteDirectoryName.GetType().FullName + "~" + nameof( mainSiteDirectoryName );
                this.lastExceptionInfo.argValue = mainSiteDirectoryName.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-5]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = mainSiteDirectoryName.GetType().FullName + "~" + nameof( mainSiteDirectoryName );
                this.lastExceptionInfo.argValue = mainSiteDirectoryName.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-5]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }

            return ( qualifiedLengthPath );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Setter.
        /// </summary>
        /// <param name="numberOfThreads">A number of maximum concurrently executed tasks for each depth entry in the recursive crawling procedure.</param>
        /// <returns>'true' if field has been modified, 'false' otherwise.</returns>

        public bool setMaximumRunningTasksPerDepthEntry( int numberOfThreads )
            {
            if ( numberOfThreads < -1 ) {
                return ( false );
                }

            this.maximumRunningTasksPerDepthEntry = numberOfThreads;
            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>The current content of the name corresponding field of the used object.</returns>

        public int getMaximumRunningTasksPerDepthEntry()
            {
            return ( this.maximumRunningTasksPerDepthEntry );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Debug test code.
        /// </summary>

        static void Main()
            {
            StdErrFlow.tryToRedirectStdErr();

            SiteCrawler sc = new SiteCrawler();
            sc.setLevelOfDepth( 2 );
            sc.setSiteURL( "http://www.a.pl" );
            sc.setAsynchronousDownloadUse( false );
            sc.setMaximumRunningTasksPerDepthEntry( 5 );

            try {
                sc.crawlThroughSite();
                }
            catch ( ArgumentNullException x ) {
                System.Diagnostics.Debug.WriteLine( x.ToString() );
                }
            catch ( Exception x ) {
                System.Diagnostics.Debug.WriteLine( x.ToString() );
                }

            StdErrFlow.tryToRetrievePreviousStdErr();
            StdErrFlow.closeStreams();
            }

        //______________________________________________________________________________________________________________________________

        }
    }