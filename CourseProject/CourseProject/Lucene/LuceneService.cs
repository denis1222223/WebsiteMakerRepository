using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CourseProject.LuceneInfrastructure
{
    public class LuceneService
    {
        private const string LuceneIndexFolder = "LuceneIndex";
        private string[] fieldsToSearchOn = { "Content", "PageUrl", "Tags" };
        internal string[] allFieldsOfDocument = { "Id", "UserName", "SiteUrl", "PageUrl", "Content", "Tags"};
        private FSDirectory luceneDirectory;
        private string dataFolder = "C:/Users/Vladislav/Desktop/WebsiteMakerRepository/CourseProject/CourseProject";
        private const int HitsLimit = 10;

        public LuceneService()
        {
            InitialiseLucene();
        }

        public string DataFolder
        {
            get { return dataFolder; }
        }

        public FSDirectory LuceneDirectory
        {
            get { return luceneDirectory; }
        }

        private void InitialiseLucene()
        {
            dataFolder = Path.Combine(dataFolder, LuceneIndexFolder);
            var luceneDirectoryInfo = new DirectoryInfo(dataFolder);

            if (!luceneDirectoryInfo.Exists)
            {

                luceneDirectoryInfo.Create();

            }

            luceneDirectory = FSDirectory.Open(luceneDirectoryInfo.FullName);
        }

        public string[] Search(string searchQuery)
        {
            try
            {
                var searcher = new IndexSearcher(LuceneDirectory);
                List<string> pageLinks = new List<string>();
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                ScoreDoc[] hits = MakeQuery(analyzer, searcher, searchQuery);
                if (hits != null)
                {
                    pageLinks.AddRange(MakeLinks(hits, searcher));                   
                }
                analyzer.Close();
                searcher.Dispose();
                return pageLinks.ToArray();
            }
            catch
            {
                return new string[0];
            }
        }

        private List<string> MakeLinks(ScoreDoc[] hits, IndexSearcher searcher)
        {
            List<string> pageLinks = new List<string>();
            foreach (var hit in hits)
            {
                var doc = searcher.Doc(hit.Doc);
                pageLinks.Add('/' + doc.Get(allFieldsOfDocument[1]) + '/'
                    + doc.Get(allFieldsOfDocument[2]) + '/'
                    + doc.Get(allFieldsOfDocument[3]));
            }
            return pageLinks;
        }

        private ScoreDoc[] MakeQuery(StandardAnalyzer analyzer, IndexSearcher searcher, string searchQuery)
        {
            QueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fieldsToSearchOn, analyzer);
            var query = ParseQuery(searchQuery, parser);
            return searcher.Search(query, HitsLimit).ScoreDocs;
        }

        private Query ParseQuery(string searchQuery, QueryParser parser)
        {
            parser.AllowLeadingWildcard = true;
            Query parsedQuery;
            try
            {
                parsedQuery = parser.Parse(searchQuery);
            }
            catch (ParseException exception)
            {
                parsedQuery = null;
            }

            if (parsedQuery == null || string.IsNullOrEmpty(parsedQuery.ToString()))
            {
                string cooked = Regex.Replace(searchQuery, @"[^\w\.\s\,@-]", " ");
                parsedQuery = parser.Parse(cooked);
            }

            return parsedQuery;
        }
    }
}