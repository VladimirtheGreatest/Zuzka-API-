using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuzka.Data.Configuration
{
    /// <summary>
    /// These values can be changed eventually in the future so can be replaced with Azure App config, for different language/culture support can be stored in db table translations.
    /// </summary>
    public static class Constants
    {
        public const string publishedYearValidationMessage = "PublishedYear must be older than 1900";
        public const string ratingValidationMessage = "Rating must be between 0 and 100";
        public const string tagsNotNullValidationMessage = "Tags must not be null";
        public const string tagsFormatValidationMessage = "Tags must not contain empty or whitespace-only strings";
        public const string tagsCountValidationMessage = "Tags must not contain more than 100 items";
        public const string documentAlreadyExists = "This document already exists";
        public const string documentNotExists = "This document does not exist";
    }
}
