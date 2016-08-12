using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Environment
{
    public static class CloudinaryInitializer
    {
        public static Cloudinary Cloudinary { get; private set; }
        static CloudinaryInitializer()
        {
            Cloudinary = new Cloudinary(
            new Account(
                "website-maker",
                "746939985299262",
                "ma6_4ccKsAk2Q_CiXcDszMKn7A4"));
        }
    }
}