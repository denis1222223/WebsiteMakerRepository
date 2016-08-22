using CourseProject.Models.Entities;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.LuceneInfrastructure
{
    public class SiteWriter
    {
        internal readonly LuceneService luceneService;

        public SiteWriter(LuceneService luceneService)
        {
            this.luceneService = luceneService;
        }

        internal void UpdateItemsToIndex(Site updatedSite, string userName)
        {
            var standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(luceneService.LuceneDirectory, standardAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (Page page in updatedSite.Pages)
                {
                    DeleteItemFromIndex(userName, updatedSite.Url, page, writer);
                    AddDocumentToWriter(userName, updatedSite, page, writer);
                }
                standardAnalyzer.Close();
                writer.Dispose();

            }
        }

        private void AddDocumentToWriter(string userName, Site site, Page page, IndexWriter writer)
        {
            string tags = string.Join(" ", site.Tags.Select(x => x.Name).ToArray());
            Document newDocument = new Document();
            newDocument.Add(new Field(luceneService.allFieldsOfDocument[0], userName + site.Url + page.Url, Field.Store.YES, Field.Index.NOT_ANALYZED));
            newDocument.Add(new Field(luceneService.allFieldsOfDocument[4], page.ContentJson, Field.Store.YES, Field.Index.ANALYZED));
            newDocument.Add(new Field(luceneService.allFieldsOfDocument[5], tags, Field.Store.YES, Field.Index.ANALYZED));
            newDocument.Add(new Field(luceneService.allFieldsOfDocument[3], page.Url, Field.Store.YES, Field.Index.NOT_ANALYZED));
            newDocument.Add(new Field(luceneService.allFieldsOfDocument[2], site.Url, Field.Store.YES, Field.Index.NOT_ANALYZED));
            newDocument.Add(new Field(luceneService.allFieldsOfDocument[1], userName, Field.Store.YES, Field.Index.NOT_ANALYZED));
            writer.AddDocument(newDocument);
        }

        internal void AddItemsToIndex(Site newSite, string userName)
        {
            var standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            using (var writer = new IndexWriter(luceneService.LuceneDirectory, standardAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (Page page in newSite.Pages)
                {
                    AddDocumentToWriter(userName, newSite, page, writer);
                }
                standardAnalyzer.Close();
                writer.Dispose();
            }

        }

        private void DeleteItemFromIndex( string userName, string siteUrl, Page page, IndexWriter writer)
        {
            var query = new TermQuery(new Term("Id", userName + siteUrl + page.Url));
            writer.DeleteDocuments(query);
        }

        internal void DeleteItemsFromIndex(Site deletedSite, string userName)
        {
            var standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            using (var writer = new IndexWriter( luceneService.LuceneDirectory, standardAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (Page page in deletedSite.Pages)
                {
                    DeleteItemFromIndex(userName, deletedSite.Url, page, writer);
                }

                standardAnalyzer.Close();
                writer.Dispose();
            }

        }

        public void Optimize()
        {
            var standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            using (var writer = new IndexWriter(luceneService.LuceneDirectory, standardAnalyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                standardAnalyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }

        }

    }
}