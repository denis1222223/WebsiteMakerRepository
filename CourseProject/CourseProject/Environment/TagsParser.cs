using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CourseProject.Models.Entities;
using CourseProject.Models;

namespace CourseProject.Environment
{
    public static class TagsParser
    {
        private static string template = @"#([a-zёа-я0-9_]{1,20})";
        private static Regex TagsRegex = new Regex(template);

        private static ApplicationDbContext localDb { get; set; }

        private static Tag GetTag( string tagName)
        {
            Tag requiredTag = localDb.Tags.FirstOrDefault(t => t.Name == tagName);
            if (requiredTag != null)
            {
                return requiredTag;
            }
            Tag newTag = new Tag { Name = tagName };
            return newTag;
        }

        private static void AddTagToSet(ref HashSet<Tag> set, string tagName)
        {
            Tag newTag = GetTag(tagName);
            if (!set.Contains(newTag))
                set.Add(newTag);
        }

        private static bool PrepareString(ref string inputString)
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

        public static HashSet<Tag> Parse(ApplicationDbContext db, String inputString)
        {
            if (PrepareString(ref inputString))
            { 
                localDb = db;
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