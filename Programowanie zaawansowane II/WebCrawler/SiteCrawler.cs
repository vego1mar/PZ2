using System;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebCrawler
    { 

    /// <summary>
    /// Allows to crawl through the given site concurrently using recursion.
    /// </summary>

    internal class SiteCrawler
        {

        private string siteURL;
        private string siteContent;

        private uint levelOfDepth;
        private StdErrFlow.ExceptionInfo lastExceptionInfo;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. Initializes the fields.
        /// </summary>

        public SiteCrawler()
            {
            this.siteURL = string.Empty;
            this.siteContent = string.Empty;
            this.levelOfDepth = 2;

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
                this.siteContent = string.Empty;
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

        public string getSiteContent()
            {
            return ( this.siteContent );
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

                WebClient website = new WebClient();
                string content = website.DownloadString( url );
                this.siteContent = content;
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Probing the network connection.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
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
                StdErrFlow.writeLine( Environment.NewLine );
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
                StdErrFlow.writeLine( Environment.NewLine );
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
                StdErrFlow.writeLine( Environment.NewLine );
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
                StdErrFlow.writeLine( Environment.NewLine );
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
            Regex regex = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");
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

        public void crawlThroughSite()
            {
            string rootDirectoryName = FileSystemFlow.createRootDirectory();
            ISet<string> absoluteLinks0 = this.performBasicCrawlingStep( this.getSiteURL(), rootDirectoryName );
            this.performLevelCrawlingStep( absoluteLinks0, rootDirectoryName, 1 );
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

            try {
                // foreach ( var urlEntry in absoluteLinks0 ) {
                System.Threading.Tasks.Parallel.ForEach( absoluteLinks0, urlEntry => {
                    string directoryName = System.IO.Path.Combine( rootDirectoryName, "lvl_" + levelOfDepthEntry.ToString() );
                    ISet<string> absoluteLinks1 = this.performBasicCrawlingStep( urlEntry, directoryName );
                    this.performLevelCrawlingStep( absoluteLinks1, rootDirectoryName, levelOfDepthEntry + 1 );
                    });
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = "ISet<string> absoluteLinks0.Count = " + absoluteLinks0.Count;
                this.lastExceptionInfo.causeEvent = "Performing the recursive crawling step model.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = "ISet<string> absoluteLinks0.Count = " + absoluteLinks0.Count;
                this.lastExceptionInfo.causeEvent = "Performing the recursive crawling step model.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( AggregateException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = "ISet<string> absoluteLinks0.Count = " + absoluteLinks0.Count;
                this.lastExceptionInfo.causeEvent = "Performing the recursive crawling step model.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = "ISet<string> absoluteLinks0.Count = " + absoluteLinks0.Count;
                this.lastExceptionInfo.causeEvent = "Performing the recursive crawling step model.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
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
            string websiteContent = this.downloadWebsiteContent( url );

            FileSystemFlow.createDirectory( directoryName );
            string fileName = FileSystemFlow.removeWindowsFileSystemReservedCharacters( url );
            string fileNameWithExtension = string.Concat( fileName, ".html" );
            FileSystemFlow.saveTextToFile( directoryName, fileNameWithExtension, websiteContent );

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
                WebClient client = new WebClient();
                websiteContent = client.DownloadString( url );
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Downloading the website content using given URL.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( NotSupportedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Downloading the website content using given URL.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( WebException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Downloading the website content using given URL.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = url.ToString();
                this.lastExceptionInfo.causeEvent = "Downloading the website content using given URL.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[SC-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }

            return ( websiteContent );
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
            sc.crawlThroughSite();

            StdErrFlow.tryToRetrievePreviousStdErr();
            StdErrFlow.closeStreams();
            }

        //______________________________________________________________________________________________________________________________

        }
    }