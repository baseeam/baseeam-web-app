/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;

namespace BaseEAM.Services
{
    /// <summary>
    /// Language service interface
    /// </summary>
    public partial interface ILanguageService : IBaseService
    {
        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        void DeleteLanguage(Language language);

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <returns>Languages</returns>
        IList<Language> GetAllLanguages();

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Language</returns>
        Language GetLanguageById(long languageId);

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        void InsertLanguage(Language language);

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        void UpdateLanguage(Language language);

        IEnumerable<Language> GetLanguages(string expression,
            dynamic parameters,
            int pageIndex = 0, 
            int pageSize = 2147483647, 
            IEnumerable<Sort> sort = null); //Int32.MaxValue
    }
}
