using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CourseProject.Models.Entities;
using CourseProject.Models;

namespace CourseProject.Environment
{
    public class TagsParser
    {
        private string template = @"#([a-zёа-я0-9_]{1,20})";
        private Regex TagsRegex;

        private ApplicationDbContext localDb { get; set; }

        public TagsParser(ApplicationDbContext db)
        {
            localDb = db;
            TagsRegex = new Regex(template);
        }

        private Tag GetTag( string tagName)
        {
            Tag requiredTag = localDb.Tags.FirstOrDefault(t => t.Name == tagName);
            if (requiredTag != null)
            {
                return requiredTag;
            }
            Tag newTag = new Tag { Name = tagName };
            return newTag;
        }

        private void AddTagToSet(ref HashSet<Tag> set, string tagName)
        {
            Tag newTag = GetTag(tagName);
            if (!set.Contains(newTag))
                set.Add(newTag);
        }

        private bool PrepareString(ref string inputString)
        {
            if (String.IsNullOrEmpty(inputString) || String.IsNullOrWhiteSpace(inputString))
            {
                return false;
            }
            else
            {
                inputString = inputString.ToLower();
                return true;
            }
        }

        public HashSet<Tag> Parse(String inputString)
        {
            if (PrepareString(ref inputString))
            { 
                HashSet<Tag> parsedTags = new HashSet<Tag>();            
                foreach (Match tagMatch in TagsRegex.Matches(inputString))
                {
                    AddTagToSet(ref parsedTags, tagMatch.Groups[1].Value);
                }
                return parsedTags;
            }
            else
            {
                return null;
            }     
        }
    }
}